using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using VisualSatisfactoryCalculator.satisfactory.Utility;

namespace VisualSatisfactoryCalculator.model.production
{
	public class Connection<ItemType, StepType, RecipeType> where ItemType : BasicItem where StepType : AbstractStep<ItemType, StepType, RecipeType>
	{
		private static readonly string NO_VISITED_NEIGHBORS = "Cannot update rates from visited when no neighbors are visited.";

		public readonly ItemType item;
		/// <summary>
		/// Steps that produce items flowing into this connection.
		/// </summary>
		private readonly Dictionary<StepType, decimal> _producers;
		/// <summary>
		/// Steps that consume items flowing out of this connection.
		/// </summary>
		private readonly Dictionary<StepType, decimal> _consumers;

		private readonly CachedValue<ImmutableHashSet<StepType>>.Managed _steps;

		/// <summary>
		/// The steps that are part of this connection.
		/// </summary>
		public ImmutableHashSet<StepType> Steps
		{
			get
			{
				return _steps.Get();
			}
		}

		public IEnumerable<StepType> ProducerSteps
		{
			get
			{
				return _producers.Keys;
			}
		}

		public IEnumerable<StepType> ConsumerSteps
		{
			get
			{
				return _consumers.Keys;
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
			_producers = new Dictionary<StepType, decimal>();
			_consumers = new Dictionary<StepType, decimal>();

			_steps = new CachedValue<ImmutableHashSet<StepType>>.Managed(() =>
			{
				return ImmutableHashSet<StepType>.Empty.Union(_producers.Keys).Union(_consumers.Keys);
			});
		}

		public Connection<ItemType, StepType, RecipeType> AddProducer(StepType step)
		{
			_producers.Add(step, step.GetRate(item, true));
			_steps.Invalidate();
			return this;
		}

		public Connection<ItemType, StepType, RecipeType> AddConsumer(StepType step)
		{
			_consumers.Add(step, step.GetRate(item, false));
			_steps.Invalidate();
			return this;
		}

		public Connection<ItemType, StepType, RecipeType> RemoveProducer(StepType step)
		{
			_producers.Remove(step);
			_steps.Invalidate();
			return this;
		}

		public Connection<ItemType, StepType, RecipeType> RemoveConsumer(StepType step)
		{
			_consumers.Remove(step);
			_steps.Invalidate();
			return this;
		}

		public bool SharesStep(Connection<ItemType, StepType, RecipeType> other)
		{
			return Steps.Intersect(other.Steps).Any();
		}

		public override bool Equals(object obj)
		{
			return this == obj;
		}

		public override int GetHashCode()
		{
			return item.GetHashCode() * _producers.Count * _consumers.Count;
		}

		public decimal GetRate(StepType step, bool isConsuming)
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

		public bool IsStepProducer(StepType step)
		{
			return _producers.ContainsKey(step);
		}

		public bool IsStepConsumer(StepType step)
		{
			return _consumers.ContainsKey(step);
		}

		public uint GetNonUpdatedStepCount(HashSet<object> visited)
		{
			uint notVisited = 0;
			foreach (StepType step in Steps)
			{
				if (!visited.Contains(step))
				{
					notVisited++;
				}
			}
			return notVisited;
		}

		/// <summary>
		/// Used by <see cref="UpdateRatesFrom(HashSet{object}, HashSet{AbstractStep{ItemType, RecipeType}})"/>.
		/// Identifies which consumers and producers have not been updated yet,
		/// and the total rate of the already updated consumers and producers.
		/// </summary>
		/// <param name="netAlreadyUpdatedRate">The total rate of the already updated consumers and producers</param>
		private void ProcessVisitedSteps(HashSet<object> visited,
										 HashSet<StepType> toVisit,
										 out HashSet<StepType> notUpdatedConsumers,
										 out HashSet<StepType> notUpdatedProducers,
										 out decimal netAlreadyUpdatedRate)
		{
			notUpdatedConsumers = new HashSet<StepType>();
			notUpdatedProducers = new HashSet<StepType>();
			netAlreadyUpdatedRate = 0;
			foreach (StepType step in Steps)
			{
				if (visited.Contains(step))
				{
					if (_producers.ContainsKey(step))
					{
						_producers[step] = step.GetRate(item, true);
						netAlreadyUpdatedRate += _producers[step];
					}
					if (_consumers.ContainsKey(step))
					{
						_consumers[step] = step.GetRate(item, false);
						netAlreadyUpdatedRate -= _consumers[step];
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

		/// <summary>
		/// Used during cascading updates. See <see cref="BreadthFirstSearchHandler{ItemType, RecipeType}"/>.
		/// </summary>
		public void UpdateRatesFrom(HashSet<object> visited, HashSet<StepType> toVisit, ProcessedPlan<StepType, ItemType, RecipeType> processedPlan)
		{
			if (Type != ConnectionType.INCOMPLETE)
			{
				ProcessVisitedSteps(visited,
									toVisit,
									out HashSet<StepType> notUpdatedConsumers,
									out HashSet<StepType> notUpdatedProducers,
									out decimal netAlreadyUpdatedRate);
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
						if (netAlreadyUpdatedRate < 0)
						{
							throw new InvalidOperationException("Unable to update consumer with deficient rate");
						}
						StepType remaining = notUpdatedConsumers.First();
						_consumers[remaining] = netAlreadyUpdatedRate;
						toVisit.Add(remaining);
					}
					else
					{
						HashSet<WellConnectedStepGroup> wellConnectedConsumerGroups = GetStepsGroups(notUpdatedConsumers, processedPlan);
						if (wellConnectedConsumerGroups.Count == 1)
						{
							decimal groupRate = 0;
							foreach (StepType consumer in notUpdatedConsumers)
							{
								groupRate -= consumer.GetRate(item, false);
							}
							if (netAlreadyUpdatedRate < 0)
							{
								throw new InvalidOperationException("Cannot update single connected consumer group when the net locked rates has deficiency");
							}
							decimal multiplier = netAlreadyUpdatedRate / Math.Abs(groupRate);
							StepType step = notUpdatedConsumers.First();
							_consumers[step] *= multiplier;
							toVisit.Add(step);
						}
						else
						{
							throw new InvalidOperationException("Cannot update when there are multiple non-updated consumer groups");
						}
					}
				}
				else if (notUpdatedConsumers.Count == 0)
				{
					if (notUpdatedProducers.Count == 1)
					{
						if (netAlreadyUpdatedRate > 0)
						{
							throw new InvalidOperationException("Unable to update producer with excess rate");
						}
						StepType remaining = notUpdatedProducers.First();
						_producers[remaining] = -netAlreadyUpdatedRate;
						toVisit.Add(remaining);
					}
					else
					{
						HashSet<WellConnectedStepGroup> wellConnectedProducerGroups = GetStepsGroups(notUpdatedProducers, processedPlan);
						if (wellConnectedProducerGroups.Count == 1)
						{
							decimal groupRate = 0;
							foreach (StepType producer in notUpdatedProducers)
							{
								groupRate += producer.GetRate(item, true);
							}
							if (netAlreadyUpdatedRate > 0)
							{
								throw new InvalidOperationException("Cannot update single connected producer group when the net locked rates has excess");
							}
							decimal multiplier = Math.Abs(netAlreadyUpdatedRate) / groupRate;
							StepType step = notUpdatedProducers.First();
							_producers[step] *= multiplier;
							toVisit.Add(step);
						}
						else
						{
							throw new InvalidOperationException("Cannot update when there are multiple nonupdated producer groups");
						}
					}
				}
				else
				{
					HashSet<WellConnectedStepGroup> notUpdatedStepGroups = GetStepsGroups(notUpdatedProducers.Concat(notUpdatedConsumers), processedPlan);
					if (notUpdatedStepGroups.Count == 1)
					{
						decimal groupRate = 0;
						foreach (StepType producerStep in notUpdatedProducers)
						{
							groupRate += producerStep.GetRate(item, true);
						}
						foreach (StepType consumerStep in notUpdatedConsumers)
						{
							groupRate -= consumerStep.GetRate(item, false);
						}
						if ((groupRate > 0) == (netAlreadyUpdatedRate > 0))
						{
							throw new InvalidOperationException("Cannot adjust single connected step group when group sign and net already updated rate sign are equal");
						}
						decimal multiplier = Math.Abs(netAlreadyUpdatedRate) / Math.Abs(groupRate);
						StepType step = notUpdatedConsumers.First();
						_consumers[step] *= multiplier;
						toVisit.Add(step);
					}
					HashSet<WellConnectedStepGroup> notUpdatedProducerGroups;
					HashSet<WellConnectedStepGroup> notUpdatedConsumerGroups;
					(notUpdatedProducerGroups, notUpdatedConsumerGroups) = GetStepGroupsAsProducersAndConsumers(processedPlan.stepGroupMap);
					notUpdatedProducerGroups.IntersectWith(notUpdatedStepGroups);
					notUpdatedConsumerGroups.IntersectWith(notUpdatedStepGroups);
					if (notUpdatedProducerGroups.Count > 1 && notUpdatedConsumerGroups.Count > 1)
					{
						throw new InvalidOperationException("Cannot update multiple connected step groups when more than one producer and consumer group is not updated");
					}
					// TODO
					if (netAlreadyUpdatedRate > 0)
					{
						if (notUpdatedConsumerGroups.Count == 1)
						{
							WellConnectedStepGroup consumerGroup = notUpdatedConsumerGroups.First();
							decimal groupRate = 0;
							decimal otherGroupsRate = 0;
							StepType step = null;
							bool stepIsConsumer = true;
							foreach (StepType consumerStep in notUpdatedConsumers)
							{
								if (processedPlan.stepGroupMap[consumerStep] == consumerGroup)
								{
									groupRate -= consumerStep.GetRate(item, false);
									step ??= consumerStep;
								}
								else
								{
									otherGroupsRate -= consumerStep.GetRate(item, false);
								}
							}
							foreach (StepType producerStep in notUpdatedProducers)
							{
								if (processedPlan.stepGroupMap[producerStep] == consumerGroup)
								{
									groupRate += producerStep.GetRate(item, true);
									if (step == null)
									{
										step = producerStep;
										stepIsConsumer = false;
									}
									else
									{
										otherGroupsRate += producerStep.GetRate(item, true);
									}
								}
							}
							decimal multiplier = Math.Abs(netAlreadyUpdatedRate + otherGroupsRate) / Math.Abs(groupRate);
							if (stepIsConsumer)
							{
								_consumers[step] *= multiplier;
							}
							else
							{
								_producers[step] *= multiplier;
							}
							toVisit.Add(step);
						}
						else
						{
							throw new InvalidOperationException("Cannot update multiple consumer groups to match net production.");
						}
					}
					else
					{
						if (notUpdatedProducerGroups.Count == 1)
						{
							WellConnectedStepGroup producerGroup = notUpdatedProducerGroups.First();
							decimal groupRate = 0;
							decimal otherGroupsRate = 0;
							StepType step = null;
							bool stepIsProducer = true;
							foreach (StepType producerStep in notUpdatedProducers)
							{
								if (processedPlan.stepGroupMap[producerStep] == producerGroup)
								{
									groupRate += producerStep.GetRate(item, true);
									step ??= producerStep;
								}
								else
								{
									otherGroupsRate += producerStep.GetRate(item, true);
								}
							}
							foreach (StepType consumerStep in notUpdatedConsumers)
							{
								if (processedPlan.stepGroupMap[consumerStep] == producerGroup)
								{
									groupRate -= consumerStep.GetRate(item, false);
									if (step == null)
									{
										step = consumerStep;
										stepIsProducer = false;
									}
								}
								else
								{
									otherGroupsRate -= consumerStep.GetRate(item, false);
								}
							}
							decimal multiplier = Math.Abs(netAlreadyUpdatedRate + otherGroupsRate) / Math.Abs(groupRate);
							if (stepIsProducer)
							{
								_producers[step] *= multiplier;
							}
							else
							{
								_consumers[step] *= multiplier;
							}
							toVisit.Add(step);
						}
						else
						{
							throw new InvalidOperationException("Cannot update multiple producer groups to match net consumption");
						}
					}
				}
			}
		}

		private HashSet<WellConnectedStepGroup> GetStepsGroups(IEnumerable<StepType> steps, ProcessedPlan<StepType, ItemType, RecipeType> processedPlan)
		{
			HashSet<WellConnectedStepGroup> groups = new HashSet<WellConnectedStepGroup>();
			foreach (StepType step in steps)
			{
				groups.Add(processedPlan.stepGroupMap[step]);
			}
			return groups;
		}

		public (HashSet<GroupType>, HashSet<GroupType>) GetStepGroupsAsProducersAndConsumers<GroupType>(IReadOnlyDictionary<StepType, GroupType> groups)
		{
			Dictionary<GroupType, decimal> groupRates = new Dictionary<GroupType, decimal>();
			foreach (StepType step in ConsumerSteps)
			{
				GroupType group = groups[step];
				if (!groupRates.ContainsKey(group))
				{
					groupRates.Add(group, 0);
				}
				groupRates[group] -= _consumers[step];
			}
			foreach (StepType step in ProducerSteps)
			{
				GroupType group = groups[step];
				if (!groupRates.ContainsKey(group))
				{
					groupRates.Add(group, 0);
				}
				groupRates[group] += _producers[step];
			}
			HashSet<GroupType> producerGroups = new HashSet<GroupType>();
			HashSet<GroupType> consumerGroups = new HashSet<GroupType>();
			foreach (KeyValuePair<GroupType, decimal> groupRate in groupRates)
			{
				if (groupRate.Value > 0)
				{
					producerGroups.Add(groupRate.Key);
				}
				else if (groupRate.Value < 0)
				{
					consumerGroups.Add(groupRate.Key);
				}
			}
			return (producerGroups, consumerGroups);
		}

		private void VerifyEqualRates()
		{
			decimal producingRate = 0, consumingRate = 0;
			foreach (decimal rate in _producers.Values)
			{
				producingRate += rate;
			}
			foreach (decimal rate in _consumers.Values)
			{
				consumingRate += rate;
			}
			if (producingRate != consumingRate)
			{
				throw new InvalidOperationException("The rates of producers and consumers do not match.");
			}
		}

		/// <summary>
		/// Updates the rates of the producers and consumers of this connection, and cascades updates.
		/// See <see cref="BreadthFirstSearchHandler{ItemType, RecipeType}"/>.
		/// </summary>
		public void CascadingSetRates(Dictionary<StepType, decimal> producers, Dictionary<StepType, decimal> consumers, ProcessedPlan<StepType, ItemType, RecipeType> processedPlan)
		{
			if (producers.Keys.Count != _producers.Keys.Count || !producers.Keys.All(key => _producers.ContainsKey(key)))
			{
				throw new InvalidOperationException("Producers do not match connection producers.");
			}
			if (consumers.Keys.Count != _consumers.Keys.Count || !consumers.Keys.All(key => _consumers.ContainsKey(key)))
			{
				throw new InvalidOperationException("Consumers do not match connection consumers.");
			}
			foreach (KeyValuePair<StepType, decimal> producer in producers)
			{
				_producers[producer.Key] = producer.Value;
			}
			foreach (KeyValuePair<StepType, decimal> consumer in consumers)
			{
				_consumers[consumer.Key] = consumer.Value;
			}
			BreadthFirstSearchHandler<ItemType, StepType, RecipeType>.CascadeUpdates(this, processedPlan);
		}
	}

	public enum ConnectionType
	{
		/// <summary>
		/// The connection lacks either producers or consumers.
		/// </summary>
		INCOMPLETE,
		/// <summary>
		/// The connnection has a single producer and a single consumer.
		/// </summary>
		SINGLE,
		/// <summary>
		/// The connection has multiple producers and/or multiple consumers.
		/// </summary>
		MULTI,
	}
}
