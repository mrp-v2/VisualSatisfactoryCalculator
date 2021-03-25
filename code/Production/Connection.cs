using System;
using System.Collections.Generic;
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
		public CachedValue<IEnumerable<Step>> ConnectedSteps { get; }

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
			ConnectedSteps = new CachedValue<IEnumerable<Step>>(() =>
			{
				HashSet<Step> steps = new HashSet<Step>();
				steps.AddRange(Consumers.Keys);
				steps.AddRange(Producers.Keys);
				return steps;
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
			Type.InvalidateIf(ConnectionType.NORMAL);
			ConnectedSteps.Invalidate();
			return this;
		}

		public Connection AddProducer(Step producer)
		{
			if (!producer.Recipe.Products.Keys.Contains(ItemUID))
			{
				throw new ArgumentException("Cannot add " + producer + " as a producer because it does not produce " + ItemUID);
			}
			Producers.Add(producer, producer.GetItemRate(ItemUID, true));
			producer.AddProductConnection(this);
			Type.InvalidateIf(ConnectionType.NORMAL);
			ConnectedSteps.Invalidate();
			return this;
		}

		public void DeleteConsumer(Step step)
		{
			Consumers.Remove(step);
			StepDeleted();
		}

		private void StepDeleted()
		{
			ConnectedSteps.Invalidate();
			Type.Invalidate();
			BalanceConnectionRates();
		}

		public void DeleteProducer(Step step)
		{
			Producers.Remove(step);
			StepDeleted();
		}

		private void BalanceConnectionRates()
		{
			SortedSet<HashSet<Step>> stepGroups = new SortedSet<HashSet<Step>>(new Util.CompareByCount<HashSet<Step>, Step>());
			foreach (Step step in ConnectedSteps.Get())
			{
				foreach (HashSet<Step> group in stepGroups)
				{
					if (group.Contains(step))
					{
						goto Continue;
					}
				}
				stepGroups.Add(step.GetAllNormallyConnectedSteps());
			Continue:
				continue;
			}
			decimal totalRate = 0, producersRate = 0, consumersRate = 0;
			foreach (decimal d in Producers.Values)
			{
				producersRate += d;
				totalRate += d;
			}
			foreach (decimal d in Consumers.Values)
			{
				consumersRate += d;
				totalRate += d;
			}
			if (totalRate != 0 && Type.Get() != ConnectionType.INCOMPLETE)
			{
			}
		}

		public void UpdateMultipliers(HashSet<Step> updated, Step from, HashSet<Connection> excludedConnections)
		{
			if (!updated.Contains(from))
			{
				throw new ArgumentException("Cannot update from a step that itself isn't updated");
			}
			decimal oldRate, newRate;
			if (Consumers.ContainsKey(from))
			{
				oldRate = Consumers[from];
				newRate = from.GetItemRate(ItemUID, false);
			}
			else
			{
				oldRate = Producers[from];
				newRate = from.GetItemRate(ItemUID, true);
			}
			decimal multiplier = newRate / oldRate;
			foreach (Step step in ConnectedSteps.Get())
			{
				if (updated.Contains(step))
				{
					continue;
				}
				step.SetMultiplier(step.Multiplier * multiplier, false);
				updated.Add(step);
				foreach (Connection connection in step.Connections.Get())
				{
					if (excludedConnections.Contains(connection))
					{
						continue;
					}
					connection.UpdateMultipliers(updated, step, excludedConnections);
				}
			}
		}

		private bool AreStepsConnectedWithoutThisConnection(Step a, Step b)
		{
			if (a == b)
			{
				return true;
			}
			return AreStepsConnectedWithoutSpecificConnectionRecursive(this, a, b, new HashSet<Connection>() { this });
		}

		private bool AreStepsConnectedWithoutSpecificConnectionRecursive(Connection exclude, Step a, Step b, HashSet<Connection> visitedConnections)
		{
			if (Consumers.ContainsKey(b) || Producers.ContainsKey(b))
			{
				return true;
			}
			visitedConnections.Add(this);
			foreach (Connection connection in a.Connections.Get())
			{
				if (connection == exclude || visitedConnections.Contains(connection))
				{
					continue;
				}
				if (connection.AreStepsConnectedWithoutSpecificConnectionRecursive(exclude, a, b, visitedConnections))
				{
					return true;
				}
			}
			return false;
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
