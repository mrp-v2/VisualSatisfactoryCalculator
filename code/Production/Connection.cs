using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VisualSatisfactoryCalculator.code.Extensions;
using VisualSatisfactoryCalculator.code.Utility;

namespace VisualSatisfactoryCalculator.code.Production
{
	public class Connection
	{
		/// <summary>
		/// Negative rates
		/// </summary>
		private Dictionary<Step, decimal> Consumers { get; }
		/// <summary>
		/// Positive Rates
		/// </summary>
		private Dictionary<Step, decimal> Producers { get; }
		public string ItemUID { get; }
		public CachedValue<ConnectionType> Type { get; }
		public CachedValue<IImmutableSet<Step>> ConnectedSteps { get; }

		public IEnumerable<Step> GetProducerSteps()
		{
			return Producers.Keys;
		}

		public Connection(string itemUID)
		{
			Consumers = new Dictionary<Step, decimal>();
			Producers = new Dictionary<Step, decimal>();
			ItemUID = itemUID;
			Type = new CachedValue<ConnectionType>(CalculateOverallConnectionType);
			ConnectedSteps = new CachedValue<IImmutableSet<Step>>(() =>
			{
				HashSet<Step> steps = new HashSet<Step>();
				steps.AddRange(Consumers.Keys);
				steps.AddRange(Producers.Keys);
				return ImmutableHashSet.CreateRange(steps);
			});
		}

		public bool ContainsStep(Step step)
		{
			return Consumers.ContainsKey(step) || Producers.ContainsKey(step);
		}

		public Connection AddConsumer(Step consumer)
		{
			if (!consumer.Recipe.Ingredients.Keys.Contains(ItemUID))
			{
				throw new ArgumentException("Cannot add " + consumer + " as a consumer because it does not consume " + ItemUID);
			}
			Consumers.Add(consumer, -consumer.GetItemRate(ItemUID, false));
			consumer.AddIngredientConnection(this);
			StepAdded();
			return this;
		}

		private void StepAdded()
		{
			Type.InvalidateIf(new ConnectionType[] { ConnectionType.NORMAL, ConnectionType.INCOMPLETE });
			foreach (Step step in Consumers.Keys)
			{
				step.NormalIngredientConnections.Invalidate();
			}
			ConnectedSteps.Invalidate();
			BalanceConnectionRates();
		}

		public Connection AddProducer(Step producer)
		{
			if (!producer.Recipe.Products.Keys.Contains(ItemUID))
			{
				throw new ArgumentException("Cannot add " + producer + " as a producer because it does not produce " + ItemUID);
			}
			Producers.Add(producer, producer.GetItemRate(ItemUID, true));
			producer.AddProductConnection(this);
			StepAdded();
			return this;
		}

		public void DeleteConsumer(Step step)
		{
			Consumers.Remove(step);
			if (Consumers.Count == 0)
			{
				foreach (Step producer in Producers.Keys)
				{
					producer.RemoveProductConnection(this);
				}
				return;
			}
			StepDeleted();
		}

		private void StepDeleted()
		{
			ConnectedSteps.Invalidate();
			Type.Invalidate();
			foreach (Step step in Consumers.Keys)
			{
				step.NormalIngredientConnections.Invalidate();
			}
			BalanceConnectionRates();
		}

		public void DeleteProducer(Step step)
		{
			Producers.Remove(step);
			if (Producers.Count == 0)
			{
				foreach (Step consumer in Consumers.Keys)
				{
					consumer.RemoveIngredientConnection(this);
				}
			}
			StepDeleted();
		}

		private void BalanceConnectionRates()
		{
			List<HashSet<Step>> stepGroups = new List<HashSet<Step>>();
			Dictionary<HashSet<Step>, decimal> stepGroupRates = new Dictionary<HashSet<Step>, decimal>();
			Dictionary<HashSet<Step>, HashSet<Step>> stepGroupsRelevantSteps = new Dictionary<HashSet<Step>, HashSet<Step>>();
			foreach (Step step in ConnectedSteps.Get())
			{
				foreach (HashSet<Step> group in stepGroups)
				{
					if (group.Contains(step))
					{
						goto Continue;
					}
				}
				HashSet<Step> stepGroup = step.GetAllNormallyConnectedSteps(this);
				stepGroups.Add(stepGroup);
				stepGroupRates.Add(stepGroup, 0);
				stepGroupsRelevantSteps.Add(stepGroup, new HashSet<Step>());
			Continue:
				continue;
			}
			stepGroups.Sort((x, y) => x.Count - y.Count);
			decimal totalRate = 0, producersRate = 0, consumersRate = 0;
			foreach (HashSet<Step> group in stepGroups)
			{
				foreach (Step step in group)
				{
					if (Producers.ContainsKey(step))
					{
						decimal rate = Producers[step];
						stepGroupRates[group] += rate; // broke, modifies iterating collection
						totalRate += rate;
						producersRate += rate;
						stepGroupsRelevantSteps[group].Add(step);
					}
					if (Consumers.ContainsKey(step))
					{
						decimal rate = Consumers[step];
						stepGroupRates[group] += rate;
						totalRate += rate;
						consumersRate += rate;
						stepGroupsRelevantSteps[group].Add(step);
					}
				}
			}
			if (totalRate != 0 && Type.Get() != ConnectionType.INCOMPLETE)
			{
				if (totalRate < 0)
				{
					for (int i = 0; i < stepGroups.Count; i++)
					{
						if (stepGroupRates[stepGroups[i]] > 0)
						{
							decimal multiplier = 1 + (-totalRate / stepGroupRates[stepGroups[i]]);
							foreach (Step step in stepGroupsRelevantSteps[stepGroups[i]])
							{
								step.SetMultiplier(step.Multiplier * multiplier, new HashSet<Connection>() { this });
								foreach (Step step2 in stepGroupsRelevantSteps[stepGroups[i]])
								{
									if (Consumers.ContainsKey(step2))
									{
										Consumers[step2] = step2.GetItemRate(ItemUID, false);
									}
									if (Producers.ContainsKey(step2))
									{
										Producers[step2] = step2.GetItemRate(ItemUID, true);
									}
								}
								return;
							}
						}
					}
					throw new InvalidOperationException("Unable to increase a producing step group");
				}
				else
				{
					for (int i = 0; i < stepGroups.Count; i++)
					{
						if (stepGroupRates[stepGroups[i]] > 0 && stepGroupRates[stepGroups[i]] > totalRate)
						{
							decimal multiplier = 1 - (totalRate / stepGroupRates[stepGroups[i]]);
							foreach (Step step in stepGroupsRelevantSteps[stepGroups[i]])
							{
								step.SetMultiplier(step.Multiplier * multiplier, new HashSet<Connection>() { this });
								foreach (Step step2 in stepGroupsRelevantSteps[stepGroups[i]])
								{
									if (Consumers.ContainsKey(step2))
									{
										Consumers[step2] = step2.GetItemRate(ItemUID, false);
									}
									if (Producers.ContainsKey(step2))
									{
										Producers[step2] = step2.GetItemRate(ItemUID, true);
									}
								}

								return;
							}
						}
					}
					throw new InvalidOperationException("Unable to decrease a producing step group");
				}
			}
		}

		public void UpdateMultipliers(HashSet<Step> updated, Step from, HashSet<Connection> excludedConnections)
		{
			if (!updated.Contains(from))
			{
				throw new ArgumentException("Cannot update from a step that itself isn't updated");
			}
			decimal oldRate = 0, newRate = 0;
			if (Consumers.ContainsKey(from))
			{
				oldRate = Consumers[from];
				newRate = -from.GetItemRate(ItemUID, false);
				Consumers[from] = newRate;
			}
			if (Producers.ContainsKey(from))
			{
				oldRate = Producers[from];
				newRate = from.GetItemRate(ItemUID, true);
				Producers[from] = newRate;
			}
			decimal multiplier = newRate / oldRate;
			foreach (Step step in ConnectedSteps.Get())
			{
				if (updated.Contains(step))
				{
					continue;
				}
				excludedConnections.Add(this);
				updated.Add(step);
				step.SetMultiplier(step.Multiplier * multiplier, excludedConnections);
			}
		}

		private ConnectionType CalculateOverallConnectionType()
		{
			if (Consumers.Count == 1 && Producers.Count == 1)
			{
				return ConnectionType.NORMAL;
			}
			else if (Consumers.Count >= 1 && Producers.Count >= 1)
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
					if (other.Consumers.ContainsKey(step))
					{
						return true;
					}
					if (other.Producers.ContainsKey(step))
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
