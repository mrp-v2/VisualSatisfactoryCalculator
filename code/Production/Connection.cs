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
		private Dictionary<Step, decimal> Consumers { get; }
		private Dictionary<Step, decimal> Producers { get; }
		public string ItemUID { get; }
		public CachedValue<OverallConnectionType> Type { get; }

		public IEnumerable<Step> GetProducerSteps()
		{
			return Producers.Keys;
		}

		public Connection(string itemUID)
		{
			Consumers = new Dictionary<Step, decimal>();
			Producers = new Dictionary<Step, decimal>();
			ItemUID = itemUID;
			Type = new CachedValue<OverallConnectionType>(CalculateOverallConnectionType);
		}

		public bool ContainsStep(Step step)
		{
			return Consumers.ContainsKey(step) || Producers.ContainsKey(step);
		}

		public HashSet<Step> GetSteps()
		{
			HashSet<Step> steps = new HashSet<Step>();
			steps.AddRange(Consumers.Keys);
			steps.AddRange(Producers.Keys);
			return steps;
		}

		public Connection AddConsumer(Step consumer)
		{
			if (!consumer.Recipe.Ingredients.Keys.Contains(ItemUID))
			{
				throw new ArgumentException("Cannot add " + consumer + " as a consumer because it does not consume " + ItemUID);
			}
			// how to calculate new rates
			// are there any producers that aren't connected to any other producer or consumer? if so, adjust it/them equally
			if (Producers.Count == 0)
			{
				Consumers.Add(consumer, consumer.GetItemRate(ItemUID, false));
				consumer.AddIngredientConnection(this);
				Type.Invalidate();
				return this;
			}
			else
			{
				// There is at least 1 Producer
				decimal consumerRate = -consumer.GetItemRate(ItemUID, false);
				if (Consumers.Count == 0)
				{
					decimal totalRate = consumerRate;
					decimal connectedRate = consumerRate;
					HashSet<Step> unconnectedProducers = new HashSet<Step>();
					foreach (Step step in Producers.Keys)
					{
						totalRate += Producers[step];
						if (AreStepsConnectedWithoutThisConnection(step, consumer))
						{
							connectedRate += Producers[step];
						}
						else
						{
							unconnectedProducers.Add(step);
						}
					}
					if (connectedRate == consumerRate)
					{
						decimal resultRate = 0;
						foreach (decimal d in Producers.Values)
						{
							resultRate += d;
						}
						Consumers.Add(consumer, resultRate);
						consumer.SetMultiplier(consumer.CalculateMultiplierForRate(ItemUID, resultRate, false));
						consumer.AddIngredientConnection(this);
						Type.Invalidate();
						return this;
					}
					if (unconnectedProducers.Count == 0)
					{
						throw new InvalidOperationException("Couldn't add consumer because it was connected to all the producers");
					}
					if (totalRate > 0)
					{
						if (totalRate - connectedRate <= totalRate)
						{
							throw new InvalidOperationException("Couldn't add consumer because the producers it was not connected to coudn't be decreased enough");
						}
						decimal unconnectedMultiplierMultiplier = 1 - (totalRate / (totalRate - connectedRate));
						foreach (Step step in unconnectedProducers)
						{
							step.SetMultiplier(step.Multiplier * unconnectedMultiplierMultiplier, new HashSet<Connection>() { this });
						}
						Consumers.Add(consumer, -consumerRate);
						consumer.AddIngredientConnection(this);
						Type.Invalidate();
						return this;
					}
					else
					{
						decimal unconnectedMultiplierMultiplier = -totalRate / (totalRate - connectedRate);
						foreach (Step step in unconnectedProducers)
						{
							step.SetMultiplier(step.Multiplier * unconnectedMultiplierMultiplier, new HashSet<Connection>() { this });
						}
						Consumers.Add(consumer, -consumerRate);
						consumer.AddIngredientConnection(this);
						Type.Invalidate();
						return this;

					}
				}
				else
				{
					// There is at least 1 Consumer and Producer
					decimal totalRate = consumerRate;
					decimal connectedRate = consumerRate;
					HashSet<Step> unconnectedProducers = new HashSet<Step>();
					HashSet<Step> hardStepsToAdjust = new HashSet<Step>();
					foreach (Step step in Producers.Keys)
					{
						totalRate += Producers[step];
						if (AreStepsConnectedWithoutThisConnection(step, consumer))
						{
							connectedRate += Producers[step];
						}
						else
						{
							unconnectedProducers.Add(step);
						}
						foreach (Step step2 in Consumers.Keys)
						{
							if (AreStepsConnectedWithoutThisConnection(step, step2))
							{
								hardStepsToAdjust.Add(step);
								hardStepsToAdjust.Add(step2);
							}
						}
					}
					HashSet<Step> unconnectedConsumers = new HashSet<Step>();
					foreach (Step step in Consumers.Keys)
					{
						totalRate += -Consumers[step];
						if (AreStepsConnectedWithoutThisConnection(step, consumer))
						{
							connectedRate += -Consumers[step];
						}
						else
						{
							unconnectedConsumers.Add(step);
						}
					}
					if (!hardStepsToAdjust.Overlaps(unconnectedProducers) || hardStepsToAdjust.IsSubsetOf(unconnectedProducers))
					{
						decimal unconnectedProducerRate = 0;
						foreach (Step step in unconnectedProducers)
						{
							unconnectedProducerRate += Producers[step];
						}
						decimal unconnectedProducerMultiplierMultiplier = -totalRate / unconnectedProducerRate;
						foreach (Step step in unconnectedProducers)
						{
							step.SetMultiplier(step.Multiplier * unconnectedProducerMultiplierMultiplier, new HashSet<Connection>() { this });
						}
						Consumers.Add(consumer, -consumerRate);
						consumer.AddIngredientConnection(this);
						Type.Invalidate();
						return this;
					}
					if (!hardStepsToAdjust.Overlaps(unconnectedConsumers) || hardStepsToAdjust.IsSubsetOf(unconnectedConsumers))
					{
						decimal unconnectedConsumerRate = 0;
						foreach (Step step in unconnectedConsumers)
						{
							unconnectedConsumerRate += Consumers[step];
						}
						decimal unconnectedConsumerMultiplierMultiplier = 1 - (-totalRate / unconnectedConsumerRate);
						foreach (Step step in unconnectedConsumers)
						{
							step.SetMultiplier(step.Multiplier * unconnectedConsumerMultiplierMultiplier, new HashSet<Connection>() { this });
							Consumers.Add(consumer, -consumerRate);
							consumer.AddIngredientConnection(this);
							Type.Invalidate();
							return this;
						}
					}
					// try to (increase any producers and decrease any consumers) that aren't connected
					// could also try adjusting the hard to adjust groups
					// this is the last section
					throw new InvalidOperationException("Couldn't add consumer");
				}
			}
		}


		public Connection AddProducer(Step producer)
		{
			if (!producer.Recipe.Products.Keys.Contains(ItemUID))
			{
				throw new ArgumentException("Cannot add " + producer + " as a producer because it does not produce " + ItemUID);
			}
			// how to calculate new rates
			// are there any consumers that aren't connected to any other producer or consumer? if so, adjust it/them equally
			if (Consumers.Count == 0)
			{
				Producers.Add(producer, producer.GetItemRate(ItemUID, true));
			}
			else
			{
				if (Producers.Count == 0)
				{
					decimal totalRate = 0;
					foreach (decimal d in Consumers.Values)
					{
						totalRate += d;
					}
					Producers.Add(producer, totalRate);
					producer.SetMultiplier(producer.CalculateMultiplierForRate(ItemUID, totalRate, true), false);
				}
				else
				{

				}
			}
			// TODO
			producer.AddProductConnection(this);
			Type.Invalidate();
			return this;
		}

		public void Delete()
		{
			foreach (Step step in Consumers.Keys)
			{
				step.RemoveIngredientConnection(this);
			}
			foreach (Step step in Producers.Keys)
			{
				step.RemoveProductConnection(this);
			}
		}

		public void Delete(Step step)
		{
			// recreate add connection logic but backwards
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
			foreach (Step step in GetSteps())
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

		private OverallConnectionType CalculateOverallConnectionType()
		{
			if (Consumers.Count == 1 && Producers.Count == 1)
			{
				return OverallConnectionType.NORMAL;
			}
			else if (Consumers.Count >= 1 && Producers.Count >= 1)
			{
				return OverallConnectionType.FIXED_RATIO;
			}
			throw new InvalidOperationException("Unable to match this connection to an OverallConnectionType");
		}

		public enum OverallConnectionType
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
			FIXED_RATIO
		}

		public bool IsConnectedNormallyTo(Connection other)
		{
			if (other.Type.Get() != OverallConnectionType.NORMAL)
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
			if (Type.Get() != OverallConnectionType.NORMAL)
			{
				return false;
			}
			HashSet<Step> connectedNormallySteps = GetSteps();
			foreach (Step step in GetSteps())
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
