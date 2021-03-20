using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.code.DataStorage
{
	public class ProductionStepConnection
	{
		private Dictionary<ProductionStep, ConnectionType> Consumers { get; }
		private Dictionary<ProductionStep, ConnectionType> Producers { get; }
		public string ItemUID { get; }

		public ProductionStepConnection(string itemUID)
		{
			Consumers = new Dictionary<ProductionStep, ConnectionType>();
			Producers = new Dictionary<ProductionStep, ConnectionType>();
			ItemUID = itemUID;
		}

		public ProductionStepConnection AddConsumer(ProductionStep consumer, ConnectionType connectionType)
		{
			if (!consumer.GetRecipe().Ingredients.Keys.Contains(ItemUID))
			{
				throw new ArgumentException("Cannot add " + consumer + " as a consumer because it does not consume " + ItemUID);
			}
			Consumers.Add(consumer, connectionType);
			return this;
		}

		public ProductionStepConnection AddProducer(ProductionStep producer, ConnectionType connectionType)
		{
			if (!producer.GetRecipe().Products.Keys.Contains(ItemUID))
			{
				throw new ArgumentException("Cannot add " + producer + " as a producer because it does not produce " + ItemUID);
			}
			Producers.Add(producer, connectionType);
			return this;
		}

		public enum ConnectionType
		{
			NORMAL
		}
	}
}
