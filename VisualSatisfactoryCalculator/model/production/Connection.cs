using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VisualSatisfactoryCalculator.code.Numbers;
using VisualSatisfactoryCalculator.code.Utility;

namespace VisualSatisfactoryCalculator.model.production
{
	public class Connection<ItemType> where ItemType : AbstractItem
	{
		private static readonly string NO_VISITED_NEIGHBORS = "Cannot update rates from visited when no neighbors are visited.";

		public readonly ItemType Item;
		/// <summary>
		/// Steps that produce items flowing into this connection.
		/// </summary>
		private readonly Dictionary<AbstractStep<ItemType>, ItemRate<ItemType>> producers;
		/// <summary>
		/// Steps that consume items flowing out of this connection.
		/// </summary>
		private readonly Dictionary<AbstractStep<ItemType>, ItemRate<ItemType>> consumers;

		private readonly CachedValue<IEnumerable<AbstractStep<ItemType>>> steps;

		public IEnumerable<AbstractStep<ItemType>> Steps
		{
			get
			{
				return steps.Get();
			}
		}

		public ConnectionType Type
		{
			get
			{
				if (producers.Count == 1 && consumers.Count == 1)
				{
					return ConnectionType.SINGLE;
				}
				else if (producers.Count == 0 || consumers.Count == 0)
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
			Item = item;
			producers = new Dictionary<AbstractStep<ItemType>, ItemRate<ItemType>>();
			consumers = new Dictionary<AbstractStep<ItemType>, ItemRate<ItemType>>();

			steps = new CachedValue<IEnumerable<AbstractStep<ItemType>>>(() =>
			{
				return new HashSet<AbstractStep<ItemType>>(Enumerable.Concat(producers.Keys, consumers.Keys));
			});
		}

		public override bool Equals(object obj)
		{
			return this == obj;
		}

		public override int GetHashCode()
		{
			return Item.GetHashCode() * producers.Count * consumers.Count;
		}

		public ItemRate<ItemType> GetRate(AbstractStep<ItemType> step, bool isConsuming)
		{
			if (isConsuming)
			{
				return consumers[step];
			}
			else
			{
				return producers[step];
			}
		}

		public uint GetNonUpdatedStepCount(HashSet<object> visited)
		{
			uint notVisited = 0;
			foreach (AbstractStep<ItemType> step in Steps)
			{
				if (!visited.Contains(step))
				{
					notVisited++;
				}
			}
			return notVisited;
		}

		private void ProcessVisitedSteps(HashSet<object> visited,
										 HashSet<AbstractStep<ItemType>> toVisit,
										 out HashSet<AbstractStep<ItemType>> notUpdatedConsumers,
										 out HashSet<AbstractStep<ItemType>> notUpdatedProducers,
										 out RationalNumber lockedRate)
		{
			notUpdatedConsumers = new HashSet<AbstractStep<ItemType>>();
			notUpdatedProducers = new HashSet<AbstractStep<ItemType>>();
			lockedRate = 0;
			foreach (AbstractStep<ItemType> step in Steps)
			{
				if (visited.Contains(step))
				{
					if (producers.ContainsKey(step))
					{
						producers[step] = step.GetRate(Item, true);
						lockedRate += producers[step].Rate;
					}
					if (consumers.ContainsKey(step))
					{
						consumers[step] = step.GetRate(Item, false);
						lockedRate -= consumers[step].Rate;
					}
				}
				else
				{
					if (producers.ContainsKey(step))
					{
						notUpdatedProducers.Add(step);
					}
					if (consumers.ContainsKey(step))
					{
						notUpdatedConsumers.Add(step);
					}
					toVisit.Add(step);
				}
			}
		}

		public void UpdateRatesFrom(HashSet<object> visited, HashSet<AbstractStep<ItemType>> toVisit)
		{
			if (Type != ConnectionType.INCOMPLETE)
			{
				ProcessVisitedSteps(visited,
									toVisit,
									out HashSet<AbstractStep<ItemType>> notUpdatedConsumers,
									out HashSet<AbstractStep<ItemType>> notUpdatedProducers,
									out RationalNumber lockedRate);
				if (notUpdatedConsumers.Count == consumers.Count && notUpdatedProducers.Count == producers.Count)
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
						if (lockedRate < 0)
						{
							throw new InvalidOperationException("Unable to update consumer with deficient rate");
						}
						AbstractStep<ItemType> remaining = notUpdatedConsumers.First();
						consumers[remaining] = new ItemRate<ItemType>(Item, lockedRate);
						toVisit.Add(remaining);
					}
					else
					{
						throw new NotImplementedException();
						// TODO distribute between consumers
					}
				}
				else if (notUpdatedConsumers.Count == 0)
				{
					if (notUpdatedProducers.Count == 1)
					{
						if (lockedRate > 0)
						{
							throw new InvalidOperationException("Unable to update producer with excess rate");
						}
						AbstractStep<ItemType> remaining = notUpdatedProducers.First();
						producers[remaining] = new ItemRate<ItemType>(Item, lockedRate);
						toVisit.Add(remaining);
					}
					else
					{
						throw new NotImplementedException();
						// TODO distribute between producers
					}
				}
				else
				{
					throw new NotImplementedException();
					// oh dear, what will we do
				}
			}
		}

		private void VerifyEqualRates()
		{
			RationalNumber producingRate = 0, consumingRate = 0;
			foreach (ItemRate<ItemType> rate in producers.Values)
			{
				producingRate += rate.Rate;
			}
			foreach (ItemRate<ItemType> rate in consumers.Values)
			{
				consumingRate += rate.Rate;
			}
			if (producingRate != consumingRate)
			{
				throw new InvalidOperationException("The rates of producers and consumers do not match.");
			}
		}

		public void CascadingSetRates(Dictionary<AbstractStep<ItemType>, ItemRate<ItemType>> producers, Dictionary<AbstractStep<ItemType>, ItemRate<ItemType>> consumers)
		{
			if (producers.Keys.Count != this.producers.Keys.Count || producers.Keys.All(key => this.producers.ContainsKey(key)))
			{
				throw new InvalidOperationException("Producers do not match connection producers.");
			}
			if (consumers.Keys.Count != this.consumers.Keys.Count || consumers.Keys.All(key => this.consumers.ContainsKey(key)))
			{
				throw new InvalidOperationException("Consumers do not match connection consumers.");
			}
			foreach (KeyValuePair<AbstractStep<ItemType>, ItemRate<ItemType>> producer in producers)
			{
				this.producers[producer.Key] = producer.Value;
			}
			foreach (KeyValuePair<AbstractStep<ItemType>, ItemRate<ItemType>> consumer in consumers)
			{
				this.consumers[consumer.Key] = consumer.Value;
			}
			CascadingUpdateHandler<ItemType>.CascadeUpdates(this);
		}
	}

	public enum ConnectionType
	{
		INCOMPLETE,
		SINGLE,
		MULTI,
	}
}
