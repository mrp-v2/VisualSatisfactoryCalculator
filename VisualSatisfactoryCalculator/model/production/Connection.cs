using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Bson;

using VisualSatisfactoryCalculator.satisfactory.Numbers;
using VisualSatisfactoryCalculator.satisfactory.Utility;

namespace VisualSatisfactoryCalculator.model.production
{
	public class Connection<ItemType, RecipeType> where ItemType : BasicItem
	{
		private static readonly string NO_VISITED_NEIGHBORS = "Cannot update rates from visited when no neighbors are visited.";

		public readonly ItemType item;
		/// <summary>
		/// Steps that produce items flowing into this connection.
		/// </summary>
		private readonly Dictionary<AbstractStep<ItemType, RecipeType>, ItemCount<ItemType>> _producers;
		/// <summary>
		/// Steps that consume items flowing out of this connection.
		/// </summary>
		private readonly Dictionary<AbstractStep<ItemType, RecipeType>, ItemCount<ItemType>> _consumers;

		private readonly CachedValue<IEnumerable<AbstractStep<ItemType, RecipeType>>> _steps;

		public IEnumerable<AbstractStep<ItemType, RecipeType>> Steps
		{
			get
			{
				return _steps.Get();
			}
		}

		public ConnectionType Type
		{
			get
			{
				if (_producers.Count == 1 && _consumers.Count == 1)
				{
					return ConnectionType.SINGLE;
				}
				else if (_producers.Count == 0 || _consumers.Count == 0)
				{
					return ConnectionType.INCOMPLETE;
				}
				else
				{
					return ConnectionType.MULTI;
				}
			}
		}

		public Connection(ItemType item)
		{
			this.item = item;
			_producers = new Dictionary<AbstractStep<ItemType, RecipeType>, ItemCount<ItemType>>();
			_consumers = new Dictionary<AbstractStep<ItemType, RecipeType>, ItemCount<ItemType>>();

			_steps = new CachedValue<IEnumerable<AbstractStep<ItemType, RecipeType>>>(() =>
			{
				return new HashSet<AbstractStep<ItemType, RecipeType>>(Enumerable.Concat(_producers.Keys, _consumers.Keys));
			});
		}

		public Connection<ItemType, RecipeType> AddProducer(AbstractStep<ItemType, RecipeType> step)
		{
			_producers.Add(step, step.GetRate(item, true));
			_steps.Invalidate();
			return this;
		}

		public Connection<ItemType, RecipeType> AddConsumer(AbstractStep<ItemType, RecipeType> step)
		{
			_consumers.Add(step, step.GetRate(item, false));
			_steps.Invalidate();
			return this;
		}

		public Connection<ItemType, RecipeType> RemoveProducer(AbstractStep<ItemType, RecipeType> step)
		{
			_producers.Remove(step);
			_steps.Invalidate();
			return this;
		}

		public Connection<ItemType, RecipeType> RemoveConsumer(AbstractStep<ItemType, RecipeType> step)
		{
			_consumers.Remove(step);
			_steps.Invalidate();
			return this;
		}

		public override bool Equals(object obj)
		{
			return this == obj;
		}

		public override int GetHashCode()
		{
			return item.GetHashCode() * _producers.Count * _consumers.Count;
		}

		public ItemCount<ItemType> GetRate(AbstractStep<ItemType, RecipeType> step, bool isConsuming)
		{
			if (isConsuming)
			{
				return _consumers[step];
			}
			else
			{
				return _producers[step];
			}
		}

		public bool IsStepProducer(AbstractStep<ItemType, RecipeType> step)
		{
			return _producers.ContainsKey(step);
		}

		public bool IsStepConsumer(AbstractStep<ItemType, RecipeType> step)
		{
			return _consumers.ContainsKey(step);
		}

		public uint GetNonUpdatedStepCount(HashSet<object> visited)
		{
			uint notVisited = 0;
			foreach (AbstractStep<ItemType, RecipeType> step in Steps)
			{
				if (!visited.Contains(step))
				{
					notVisited++;
				}
			}
			return notVisited;
		}

		private void ProcessVisitedSteps(HashSet<object> visited,
										 HashSet<AbstractStep<ItemType, RecipeType>> toVisit,
										 out HashSet<AbstractStep<ItemType, RecipeType>> notUpdatedConsumers,
										 out HashSet<AbstractStep<ItemType, RecipeType>> notUpdatedProducers,
										 out RationalNumber lockedRate)
		{
			notUpdatedConsumers = new HashSet<AbstractStep<ItemType, RecipeType>>();
			notUpdatedProducers = new HashSet<AbstractStep<ItemType, RecipeType>>();
			lockedRate = 0;
			foreach (AbstractStep<ItemType, RecipeType> step in Steps)
			{
				if (visited.Contains(step))
				{
					if (_producers.ContainsKey(step))
					{
						_producers[step] = step.GetRate(item, true);
						lockedRate += _producers[step].rate;
					}
					if (_consumers.ContainsKey(step))
					{
						_consumers[step] = step.GetRate(item, false);
						lockedRate -= _consumers[step].rate;
					}
				}
				else
				{
					if (_producers.ContainsKey(step))
					{
						notUpdatedProducers.Add(step);
					}
					if (_consumers.ContainsKey(step))
					{
						notUpdatedConsumers.Add(step);
					}
					toVisit.Add(step);
				}
			}
		}

		public void UpdateRatesFrom(HashSet<object> visited, HashSet<AbstractStep<ItemType, RecipeType>> toVisit)
		{
			if (Type != ConnectionType.INCOMPLETE)
			{
				ProcessVisitedSteps(visited,
									toVisit,
									out HashSet<AbstractStep<ItemType, RecipeType>> notUpdatedConsumers,
									out HashSet<AbstractStep<ItemType, RecipeType>> notUpdatedProducers,
									out RationalNumber netLockedRate);
				if (notUpdatedConsumers.Count == _consumers.Count && notUpdatedProducers.Count == _producers.Count)
				{
					throw new InvalidOperationException(NO_VISITED_NEIGHBORS);
				}
				if (notUpdatedProducers.Count == 0 && notUpdatedConsumers.Count == 0)
				{
					VerifyEqualRates();
				}
				else if (notUpdatedProducers.Count == 0)
				{
					if (notUpdatedConsumers.Count == 1)
					{
						if (netLockedRate < 0)
						{
							throw new InvalidOperationException("Unable to update consumer with deficient rate");
						}
						AbstractStep<ItemType, RecipeType> remaining = notUpdatedConsumers.First();
						_consumers[remaining] = new ItemCount<ItemType>(item, netLockedRate);
						toVisit.Add(remaining);
					}
					else
					{
						HashSet<HashSet<AbstractStep<ItemType, RecipeType>>> singleConnectedConsumers = GetSingleConnectedStepGroups(notUpdatedConsumers);
						if (singleConnectedConsumers.Count == 1)
						{
							RationalNumber groupRate = 0;
							foreach (AbstractStep<ItemType, RecipeType> step in notUpdatedConsumers)
							{
								groupRate -= step.GetRate(item, false).rate;
							}
							if (netLockedRate < 0)
							{
								throw new InvalidOperationException("Cannot update single connected consumer group when the net locked rates has deficiency");
							}
							RationalNumber multiplier = netLockedRate / groupRate.AbsoluteValue();
							foreach (AbstractStep<ItemType, RecipeType> step in notUpdatedConsumers)
							{
								_consumers[step] *= multiplier;
								toVisit.Add(step);
							}
						}
						else
						{
							throw new NotImplementedException();
							// TODO prompt user on how to distribute between consumers
						}
					}
				}
				else if (notUpdatedConsumers.Count == 0)
				{
					if (notUpdatedProducers.Count == 1)
					{
						if (netLockedRate > 0)
						{
							throw new InvalidOperationException("Unable to update producer with excess rate");
						}
						AbstractStep<ItemType, RecipeType> remaining = notUpdatedProducers.First();
						_producers[remaining] = new ItemCount<ItemType>(item, netLockedRate);
						toVisit.Add(remaining);
					}
					else
					{
						HashSet<HashSet<AbstractStep<ItemType, RecipeType>>> singleConnectedProducers = GetSingleConnectedStepGroups(notUpdatedProducers);
						if (singleConnectedProducers.Count == 1)
						{
							RationalNumber groupRate = 0;
							foreach (AbstractStep<ItemType, RecipeType> step in notUpdatedProducers)
							{
								groupRate += step.GetRate(item, true).rate;
							}
							if (netLockedRate > 0)
							{
								throw new InvalidOperationException("Cannot update single connected producer group when the net locked rates has excess");
							}
							RationalNumber multiplier = netLockedRate.AbsoluteValue() / groupRate;
							foreach (AbstractStep<ItemType, RecipeType> step in notUpdatedProducers)
							{
								_producers[step] *= multiplier;
								toVisit.Add(step);
							}
						}
						else
						{
							throw new NotImplementedException();
							// TODO prompt user on how to distribute between producers
						}
					}
				}
				else
				{
					HashSet<HashSet<AbstractStep<ItemType, RecipeType>>> singleConnectedStepGroups = GetSingleConnectedStepGroups(new HashSet<AbstractStep<ItemType, RecipeType>>(notUpdatedProducers.Concat(notUpdatedConsumers)));
					if (singleConnectedStepGroups.Count == 1)
					{
						RationalNumber groupRate = 0;
						foreach (AbstractStep<ItemType, RecipeType> step in notUpdatedProducers)
						{
							groupRate += step.GetRate(item, true).rate;
						}
						foreach (AbstractStep<ItemType, RecipeType> step in notUpdatedConsumers)
						{
							groupRate -= step.GetRate(item, false).rate;
						}
						if (groupRate.AreSignsEqual(netLockedRate))
						{
							throw new InvalidOperationException("Cannot adjust single connected step group when group sign and net locked rate sign are equal");
						}
						RationalNumber multiplier = netLockedRate.AbsoluteValue() / groupRate.AbsoluteValue();
						AbstractStep<ItemType, RecipeType> singleConsumer = notUpdatedConsumers.First();
						foreach (AbstractStep<ItemType, RecipeType> step in notUpdatedProducers)
						{
							_producers[step] *= multiplier;
							toVisit.Add(step);
						}
						foreach (AbstractStep<ItemType, RecipeType> step in notUpdatedConsumers)
						{
							_consumers[step] *= multiplier;
							toVisit.Add(step);
						}
						BreadthFirstSearchHandler<ItemType, RecipeType>.CascadeUpdates(singleConsumer, false);
					}
					else
					{
						throw new NotImplementedException();
						// oh dear, what will we do
						// TODO prompt user for what to do
					}
				}
			}
		}

		private HashSet<HashSet<AbstractStep<ItemType, RecipeType>>> GetSingleConnectedStepGroups(IEnumerable<AbstractStep<ItemType, RecipeType>> steps)
		{
			HashSet<HashSet<AbstractStep<ItemType, RecipeType>>> groups = new HashSet<HashSet<AbstractStep<ItemType, RecipeType>>>();
			foreach (AbstractStep<ItemType, RecipeType> step in steps)
			{
				foreach (HashSet<AbstractStep<ItemType, RecipeType>> group in groups)
				{
					if (group.Contains(step))
					{
						goto OuterContinue;
					}
				}
				groups.Add(BreadthFirstSearchHandler<ItemType, RecipeType>.GetSingleConnectedSteps(step));
			OuterContinue:
				continue;
			}
			return groups;
		}

		private void VerifyEqualRates()
		{
			RationalNumber producingRate = 0, consumingRate = 0;
			foreach (ItemCount<ItemType> rate in _producers.Values)
			{
				producingRate += rate.rate;
			}
			foreach (ItemCount<ItemType> rate in _consumers.Values)
			{
				consumingRate += rate.rate;
			}
			if (producingRate != consumingRate)
			{
				throw new InvalidOperationException("The rates of producers and consumers do not match.");
			}
		}

		public void CascadingSetRates(Dictionary<AbstractStep<ItemType, RecipeType>, ItemCount<ItemType>> producers, Dictionary<AbstractStep<ItemType, RecipeType>, ItemCount<ItemType>> consumers)
		{
			if (producers.Keys.Count != _producers.Keys.Count || !producers.Keys.All(key => _producers.ContainsKey(key)))
			{
				throw new InvalidOperationException("Producers do not match connection producers.");
			}
			if (consumers.Keys.Count != _consumers.Keys.Count || !consumers.Keys.All(key => _consumers.ContainsKey(key)))
			{
				throw new InvalidOperationException("Consumers do not match connection consumers.");
			}
			foreach (KeyValuePair<AbstractStep<ItemType, RecipeType>, ItemCount<ItemType>> producer in producers)
			{
				_producers[producer.Key] = producer.Value;
			}
			foreach (KeyValuePair<AbstractStep<ItemType, RecipeType>, ItemCount<ItemType>> consumer in consumers)
			{
				_consumers[consumer.Key] = consumer.Value;
			}
			BreadthFirstSearchHandler<ItemType, RecipeType>.CascadeUpdates(this);
		}
	}

	public enum ConnectionType
	{
		INCOMPLETE,
		SINGLE,
		MULTI,
	}
}
