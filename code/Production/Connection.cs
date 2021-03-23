using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VisualSatisfactoryCalculator.code.Extensions;

namespace VisualSatisfactoryCalculator.code.Production
{
	public class Connection
	{
		private Dictionary<Step, ConnectionType> Consumers { get; }
		private Dictionary<Step, ConnectionType> Producers { get; }
		public string ItemUID { get; }
		public OverallConnectionType Type { get; private set; }

		public IEnumerable<Step> GetProducerSteps()
		{
			return Producers.Keys;
		}

		public Connection(string itemUID)
		{
			Consumers = new Dictionary<Step, ConnectionType>();
			Producers = new Dictionary<Step, ConnectionType>();
			ItemUID = itemUID;
			Type = OverallConnectionType.NONE;
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

		public Connection AddNormalConsumer(Step consumer)
		{
			if (Consumers.Count > 0)
			{
				throw new InvalidOperationException("Cannot add a normal consumer when there are other consumers");
			}
			return AddConsumer(consumer, ConnectionType.NORMAL);
		}

		public Connection AddNormalProducer(Step producer)
		{
			if (Producers.Count > 0)
			{
				throw new InvalidOperationException("Cannot add a normal producer when there are other producers");
			}
			return AddProducer(producer, ConnectionType.NORMAL);
		}

		private Connection AddConsumer(Step consumer, ConnectionType connectionType)
		{
			if (!consumer.GetRecipe().Ingredients.Keys.Contains(ItemUID))
			{
				throw new ArgumentException("Cannot add " + consumer + " as a consumer because it does not consume " + ItemUID);
			}
			Consumers.Add(consumer, connectionType);
			consumer.AddIngredientConnection(this);
			CalculateOverallConnectionType();
			return this;
		}

		private Connection AddProducer(Step producer, ConnectionType connectionType)
		{
			if (!producer.GetRecipe().Products.Keys.Contains(ItemUID))
			{
				throw new ArgumentException("Cannot add " + producer + " as a producer because it does not produce " + ItemUID);
			}
			Producers.Add(producer, connectionType);
			producer.AddProductConnection(this);
			CalculateOverallConnectionType();
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

		public void UpdateMultipliers(List<Step> updated, Step from)
		{
			if (Type == OverallConnectionType.NORMAL)
			{
				bool isUpdateFromProducer = Producers.ContainsKey(from);
				foreach (Step step in GetSteps())
				{
					if (!updated.Contains(step))
					{
						if (step == from)
						{
							throw new ArgumentException("Cannot update from a step that itself isn't updated");
						}
						step.SetMultiplier(step.CalculateMultiplierForRate(ItemUID, from.GetItemRate(ItemUID, isUpdateFromProducer), !isUpdateFromProducer));
						updated.Add(step);
						foreach (Connection connection in step.Connections.Get())
						{
							connection.UpdateMultipliers(updated, step);
						}
					}
				}
			}
		}

		void CalculateOverallConnectionType()
		{
			if (Consumers.Count == 1 && Producers.Count == 1)
			{
				if (Producers.Values.ToArray()[0] == ConnectionType.NORMAL && Consumers.Values.ToArray()[0] == ConnectionType.NORMAL)
				{
					Type = OverallConnectionType.NORMAL;
					return;
				}
			}
			Type = OverallConnectionType.NONE;
		}

		public enum OverallConnectionType
		{
			NORMAL, NONE
		}

		public enum ConnectionType
		{
			NORMAL, NONE
		}

		public bool IsConnectedNormallyTo(Connection other)
		{
			if (other.Type != OverallConnectionType.NORMAL)
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
			if (Type != OverallConnectionType.NORMAL)
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
