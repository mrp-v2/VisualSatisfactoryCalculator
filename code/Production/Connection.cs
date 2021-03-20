using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VisualSatisfactoryCalculator.code.Extensions;

namespace VisualSatisfactoryCalculator.code.DataStorage
{
	public class Connection
	{
		private Dictionary<Step, ConnectionType> Consumers { get; }
		private Dictionary<Step, ConnectionType> Producers { get; }
		public string ItemUID { get; }

		public Connection(string itemUID)
		{
			Consumers = new Dictionary<Step, ConnectionType>();
			Producers = new Dictionary<Step, ConnectionType>();
			ItemUID = itemUID;
		}

		public bool ContainsStep(Step step)
		{
			return Consumers.ContainsKey(step) || Producers.ContainsKey(step);
		}

		private HashSet<Step> GetLocalSteps()
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
			consumer.Connections.Add(this);
			return this;
		}

		private Connection AddProducer(Step producer, ConnectionType connectionType)
		{
			if (!producer.GetRecipe().Products.Keys.Contains(ItemUID))
			{
				throw new ArgumentException("Cannot add " + producer + " as a producer because it does not produce " + ItemUID);
			}
			Producers.Add(producer, connectionType);
			producer.Connections.Add(this);
			return this;
		}

		public void Delete()
		{
			foreach (Step step in Consumers.Keys)
			{
				step.Connections.Remove(this);
			}
			foreach (Step step in Producers.Keys)
			{
				step.Connections.Remove(this);
			}
		}

		public HashSet<Step> GetAllSteps(HashSet<Step> visited)
		{
			foreach (Step step in GetLocalSteps())
			{
				if (!visited.Contains(step))
				{
					visited.Add(step);
					foreach (Connection connection in step.Connections)
					{
						connection.GetAllSteps(visited);
					}
				}
			}
			return visited;
		}

		public void UpdateMultipliers(List<Step> updated, Step from)
		{
			if (VerifyOverallConnectionType() == OverallConnectionType.NORMAL)
			{
				bool isUpdateFromProducer = Producers.ContainsKey(from);
				foreach (Step step in GetLocalSteps())
				{
					if (!updated.Contains(step))
					{
						if (step == from)
						{
							throw new ArgumentException("Cannot update from a step that itself isn't updated");
						}
						step.SetMultiplier(step.CalculateMultiplierForRate(ItemUID, from.GetItemRate(ItemUID, isUpdateFromProducer), !isUpdateFromProducer));
						updated.Add(step);
						foreach (Connection connection in step.Connections)
						{
							connection.UpdateMultipliers(updated, step);
						}
					}
				}
			}
		}

		OverallConnectionType VerifyOverallConnectionType()
		{
			// Test 1: NORMAL
			if (Consumers.Count == 1 && Producers.Count == 1)
			{
				if (Producers.Values.ToArray()[0] == ConnectionType.NORMAL && Consumers.Values.ToArray()[0] == ConnectionType.NORMAL)
				{
					return OverallConnectionType.NORMAL;
				}
			}
			throw new InvalidOperationException("Illegal state detected - could not match any OverallConnectionType");
		}

		private enum OverallConnectionType
		{
			NORMAL
		}

		private enum ConnectionType
		{
			NORMAL
		}
	}
}
