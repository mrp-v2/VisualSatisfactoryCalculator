﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using VisualSatisfactoryCalculator.code.Extensions;
using VisualSatisfactoryCalculator.code.Numbers;
using VisualSatisfactoryCalculator.code.Utility;
using VisualSatisfactoryCalculator.controls.user;
using VisualSatisfactoryCalculator.forms;

namespace VisualSatisfactoryCalculator.code.Production
{
	public class Connection
	{
		/// <summary>
		/// Negative rates
		/// </summary>
		private readonly Dictionary<Step, RationalNumber> consumers;
		/// <summary>
		/// Positive Rates
		/// </summary>
		private readonly Dictionary<Step, RationalNumber> producers;
		public string ItemID { get; }
		public readonly CachedValue<ConnectionType> Type;
		public readonly CachedValue<IImmutableSet<Step>> ConnectedSteps;
		public readonly CachedValue<IImmutableList<HashSet<Step>>> ConnectedStepGroups;
		public readonly CachedValue<IImmutableList<HashSet<Step>>> RelevantConnectedStepGroups;
		private SplitAndMergeControl control;

		public IImmutableSet<Step> GetProducerSteps()
		{
			return ImmutableHashSet.CreateRange(producers.Keys);
		}

		public void SetControl(SplitAndMergeControl control)
		{
			this.control = control;
		}

		public void UpdateRates(BalancingPrompt prompt)
		{
			foreach (Step step in prompt.ConsumingStepMap.Keys)
			{
				if (step.GetItemRate(ItemID, false) != prompt.ConsumingStepMap[step].Rate)
				{
					step.SetMultiplier(step.CalculateMultiplierForRate(ItemID, prompt.ConsumingStepMap[step].Rate, false), new HashSet<Connection>() { this });
				}
				consumers[step] = step.GetItemRate(ItemID, false);
			}
			foreach (Step step in prompt.ProducingStepMap.Keys)
			{
				if (step.GetItemRate(ItemID, true) != prompt.ProducingStepMap[step].Rate)
				{
					step.SetMultiplier(step.CalculateMultiplierForRate(ItemID, prompt.ProducingStepMap[step].Rate, true), new HashSet<Connection>() { this });
				}
				producers[step] = step.GetItemRate(ItemID, true);
			}
			UpdateControl();
		}

		public Connection(string itemUID)
		{
			consumers = new Dictionary<Step, RationalNumber>();
			producers = new Dictionary<Step, RationalNumber>();
			ItemID = itemUID;
			Type = new CachedValue<ConnectionType>(CalculateOverallConnectionType);
			ConnectedSteps = new CachedValue<IImmutableSet<Step>>(() =>
			{
				HashSet<Step> steps = new HashSet<Step>();
				steps.AddRange(consumers.Keys);
				steps.AddRange(producers.Keys);
				return ImmutableHashSet.CreateRange(steps);
			});
			ConnectedStepGroups = new CachedValue<IImmutableList<HashSet<Step>>>(() =>
			{
				List<HashSet<Step>> groups = new List<HashSet<Step>>();
				foreach (Step step in ConnectedSteps.Get())
				{
					foreach (HashSet<Step> group in groups)
					{
						if (group.Contains(step))
						{
							goto Continue;
						}
					}
					groups.Add(step.GetAllConnectedSteps(this));
				Continue:
					continue;
				}
				return ImmutableList.CreateRange(groups);
			});
			RelevantConnectedStepGroups = new CachedValue<IImmutableList<HashSet<Step>>>(() =>
			{
				ConnectedStepGroups.Get();
				List<HashSet<Step>> relevantStepGroups = new List<HashSet<Step>>();
				foreach (HashSet<Step> group in ConnectedStepGroups.Get())
				{
					HashSet<Step> relevantGroup = new HashSet<Step>();
					relevantStepGroups.Add(relevantGroup);
					foreach (Step step in group)
					{
						if (consumers.ContainsKey(step) || producers.ContainsKey(step))
						{
							relevantGroup.Add(step);
						}
					}
				}
				return ImmutableList.CreateRange(relevantStepGroups);
			});
		}

		public Connection(CondensedPlan.CondensedConnection condensedConnection, CondensedPlan.ExpandingContext context, HashSet<CondensedPlan.CondensedStep> condensedSteps) : this(condensedConnection.ItemID)
		{
			foreach (CondensedPlan.CondensedStep condensedStep in condensedSteps)
			{
				if (condensedConnection.Consumers.ContainsKey(condensedStep.ID))
				{
					Step step = context.stepIDs[condensedStep.ID];
					step.AddIngredientConnection(this);
					consumers.Add(step, -condensedConnection.Consumers[condensedStep.ID]);
				}
				if (condensedConnection.Producers.ContainsKey(condensedStep.ID))
				{
					Step step = context.stepIDs[condensedStep.ID];
					step.AddProductConnection(this);
					producers.Add(step, condensedConnection.Producers[condensedStep.ID]);
				}
			}
		}

		public bool ContainsStep(Step step)
		{
			return consumers.ContainsKey(step) || producers.ContainsKey(step);
		}

		public Connection AddConsumer(Step consumer)
		{
			if (!consumer.Recipe.Ingredients.Keys.Contains(ItemID))
			{
				throw new ArgumentException("Cannot add " + consumer + " as a consumer because it does not consume " + ItemID);
			}
			consumers.Add(consumer, -consumer.GetItemRate(ItemID, false));
			consumer.AddIngredientConnection(this);
			StepAdded();
			return this;
		}

		private void StepAdded()
		{
			Type.InvalidateIf(new ConnectionType[] { ConnectionType.NORMAL, ConnectionType.INCOMPLETE });
			foreach (Step step in consumers.Keys)
			{
				step.NormalIngredientConnections.Invalidate();
			}
			StepsChanged();
		}

		public void VerifyConnection()
		{
			RationalNumber producersTotal = 0, consumersTotal = 0;
			foreach (RationalNumber d in producers.Values)
			{
				producersTotal += d;
			}
			foreach (RationalNumber d in consumers.Values)
			{
				consumersTotal += d;
			}
			if (producersTotal + consumersTotal != 0)
			{
				throw new InvalidOperationException("This connection is invalid!");
			}
		}

		private void StepsChanged()
		{
			ConnectedSteps.Invalidate();
			ConnectedStepGroups.Invalidate();
			RelevantConnectedStepGroups.Invalidate();
			BalanceConnectionRates();
		}

		public IImmutableSet<Step> GetConsumerSteps()
		{
			return ImmutableHashSet.CreateRange(consumers.Keys);
		}

		public RationalNumber GetProducerRate(Step producer)
		{
			return producers[producer];
		}

		public RationalNumber GetConsumerRate(Step consumer)
		{
			return -consumers[consumer];
		}

		public Connection AddProducer(Step producer)
		{
			if (!producer.Recipe.Products.Keys.Contains(ItemID))
			{
				throw new ArgumentException("Cannot add " + producer + " as a producer because it does not produce " + ItemID);
			}
			producers.Add(producer, producer.GetItemRate(ItemID, true));
			producer.AddProductConnection(this);
			StepAdded();
			return this;
		}

		public void DeleteConsumer(Step step)
		{
			consumers.Remove(step);
			if (consumers.Count == 0)
			{
				foreach (Step producer in producers.Keys)
				{
					producer.RemoveProductConnection(this);
				}
				return;
			}
			StepDeleted();
		}

		public void MergeWith(Connection other)
		{
			if (this == other)
			{
				return;
			}
			if (other.ItemID != ItemID)
			{
				throw new ArgumentException("Cannot merge connections with different items");
			}
			foreach (Step step in other.producers.Keys)
			{
				producers.Add(step, other.producers[step]);
				step.RemoveProductConnection(other);
				step.AddProductConnection(this);
			}
			foreach (Step step in other.consumers.Keys)
			{
				consumers.Add(step, other.consumers[step]);
				step.RemoveIngredientConnection(other);
				step.AddIngredientConnection(this);
			}
			StepAdded();
		}

		private void StepDeleted()
		{
			Type.Invalidate();
			foreach (Step step in consumers.Keys)
			{
				step.NormalIngredientConnections.Invalidate();
			}
			foreach (Step step in producers.Keys)
			{
				step.HasNormalProductConnections.Invalidate();
			}
			StepsChanged();
		}

		public void DeleteProducer(Step step)
		{
			producers.Remove(step);
			if (producers.Count == 0)
			{
				foreach (Step consumer in consumers.Keys)
				{
					consumer.RemoveIngredientConnection(this);
				}
			}
			StepDeleted();
		}

		private void BalanceConnectionRates()
		{
			if (Type.Get() != ConnectionType.INCOMPLETE)
			{
				if (Type.Get() == ConnectionType.NORMAL)
				{
					// do nothing, should already match	
				}
				else
				{
					Dictionary<HashSet<Step>, (RationalNumber, RationalNumber)> stepGroupRates = new Dictionary<HashSet<Step>, (RationalNumber, RationalNumber)>();
					RationalNumber producersRate = 0, consumersRate = 0;
					foreach (HashSet<Step> stepGroup in RelevantConnectedStepGroups.Get())
					{
						stepGroupRates.Add(stepGroup, (0, 0));
						foreach (Step step in stepGroup)
						{
							if (consumers.ContainsKey(step))
							{
								consumersRate -= consumers[step];
								stepGroupRates[stepGroup] = (stepGroupRates[stepGroup].Item1, stepGroupRates[stepGroup].Item2 - consumers[step]);
							}
							if (producers.ContainsKey(step))
							{
								producersRate += producers[step];
								stepGroupRates[stepGroup] = (stepGroupRates[stepGroup].Item1 + producers[step], stepGroupRates[stepGroup].Item2);
							}
						}
					}
					RationalNumber producersBasedTotalRate = 0;
					foreach (RationalNumber d in producers.Values)
					{
						producersBasedTotalRate += d;
					}
					RationalNumber consumersBasedTotalRate = 0;
					foreach (RationalNumber d in consumers.Values)
					{
						consumersBasedTotalRate -= d;
					}
					if (Util.TryBalanceRates(stepGroupRates, consumersBasedTotalRate, consumersBasedTotalRate, out (RationalNumber, RationalNumber, RationalNumber) multipliers)) { }
					else if (Util.TryBalanceRates(stepGroupRates, producersBasedTotalRate, producersBasedTotalRate, out multipliers)) { }
					else
					{
						throw new InvalidOperationException("Could not balance the rates");
					}
					foreach (HashSet<Step> group in stepGroupRates.Keys)
					{
						foreach (Step step in group)
						{
							(RationalNumber, RationalNumber) rates = stepGroupRates[group];
							if (rates.Item1 == 0 || rates.Item2 == 0)
							{
								if (consumers.ContainsKey(step))
								{
									step.SetMultiplier(step.Multiplier * multipliers.Item2, new HashSet<Connection>() { this });
									goto Continue;
								}
								else
								{
									step.SetMultiplier(step.Multiplier * multipliers.Item1, new HashSet<Connection>() { this });
									goto Continue;
								}
							}
							else
							{
								step.SetMultiplier(step.Multiplier * multipliers.Item3, new HashSet<Connection>() { this });
								goto Continue;
							}
						}
					Continue:
						continue;
					}
					UpdateRatesFromSteps();
				}
			}
		}

		private void UpdateRatesFromSteps()
		{
			foreach (Step step in GetConsumerSteps())
			{
				consumers[step] = -step.GetItemRate(ItemID, false);
			}
			foreach (Step step in GetProducerSteps())
			{
				producers[step] = step.GetItemRate(ItemID, true);
			}
		}

		private void UpdateControl()
		{
			if (control != null)
			{
				control.UpdateNumerics();
			}
		}

		public class UpdateException : InvalidOperationException
		{
			public UpdateException()
			{
			}

			public UpdateException(string message) : base(message)
			{
			}
		}

		public void UpdateMultipliers(HashSet<Step> updated, Step from, HashSet<Connection> excludedConnections)
		{
			if (!updated.Contains(from))
			{
				throw new ArgumentException("Cannot update from a step that itself isn't updated");
			}
			RationalNumber oldRate = 0, newRate = 0;
			if (consumers.ContainsKey(from))
			{
				oldRate = consumers[from];
				newRate = -from.GetItemRate(ItemID, false);
				if (!oldRate.AreSignsEqual(newRate))
				{
					throw new UpdateException();
				}
			}
			if (producers.ContainsKey(from))
			{
				oldRate = producers[from];
				newRate = from.GetItemRate(ItemID, true);
				if (!oldRate.AreSignsEqual(newRate))
				{
					throw new UpdateException();
				}
			}
			RationalNumber multiplier = newRate / oldRate;
			foreach (Step step in ConnectedSteps.Get())
			{
				if (updated.Contains(step))
				{
					if (producers.ContainsKey(step))
					{
						producers[step] = step.GetItemRate(ItemID, true);
					}
					if (consumers.ContainsKey(step))
					{
						consumers[step] = -step.GetItemRate(ItemID, false);
					}
					continue;
				}
				excludedConnections.Add(this);
				step.SetMultiplier(step.Multiplier * multiplier, excludedConnections, updated);
				if (producers.ContainsKey(step))
				{
					producers[step] = step.GetItemRate(ItemID, true);
				}
				if (consumers.ContainsKey(step))
				{
					consumers[step] = -step.GetItemRate(ItemID, false);
				}
			}
			UpdateControl();
		}

		private ConnectionType CalculateOverallConnectionType()
		{
			if (consumers.Count == 1 && producers.Count == 1)
			{
				return ConnectionType.NORMAL;
			}
			else if (consumers.Count >= 1 && producers.Count >= 1)
			{
				return ConnectionType.FIXED_RATIO;
			}
			return ConnectionType.INCOMPLETE;
		}

		public enum ConnectionType
		{
			/// <summary>
			/// The connection has 1 producer and 1 consumer
			/// </summary>
			NORMAL,
			/// <summary>
			/// The connection has multiple producers and/or multiple consumers,
			/// <br></br>
			/// and a fixed ratio between all the producers and consumers.
			/// </summary>
			FIXED_RATIO,
			/// <summary>
			/// The connection has no producers and/or no consumers
			/// </summary>
			INCOMPLETE
		}

		public bool IsConnectedNormallyTo(Connection other)
		{
			if (other.Type.Get() != ConnectionType.NORMAL)
			{
				return false;
			}
			if (other.Equals(this))
			{
				return true;
			}
			return IsConnectedNormallyToRecursive(other, new HashSet<Connection> { this });
		}

		private bool IsConnectedNormallyToRecursive(Connection other, HashSet<Connection> visited)
		{
			if (Type.Get() != ConnectionType.NORMAL)
			{
				return false;
			}
			IEnumerable<Step> connectedNormallySteps = ConnectedSteps.Get();
			foreach (Step step in ConnectedSteps.Get())
			{
				if (step.Connections.Get().Contains(other))
				{
					if (other.consumers.ContainsKey(step))
					{
						return true;
					}
					if (other.producers.ContainsKey(step))
					{
						return true;
					}
				}
			}
			foreach (Step step in connectedNormallySteps)
			{
				foreach (Connection connection in step.Connections.Get())
				{
					if (!visited.Contains(connection))
					{
						visited.Add(connection);
						if (connection.IsConnectedNormallyToRecursive(other, visited))
						{
							return true;
						}
					}
				}
			}
			return false;
		}
	}
}
