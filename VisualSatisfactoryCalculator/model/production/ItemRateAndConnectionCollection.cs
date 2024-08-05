using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VisualSatisfactoryCalculator.code.Numbers;

namespace VisualSatisfactoryCalculator.model.production
{
	public class ItemRateAndConnectionCollection<ItemType> where ItemType : AbstractItem
	{
		private readonly ItemRateCollection<ItemType> rates;
		private readonly Dictionary<ItemType, Connection<ItemType>> connections;

		public ItemRateAndConnectionCollection()
		{
			rates = new ItemRateCollection<ItemType>();
			connections = new Dictionary<ItemType, Connection<ItemType>>();
		}

		public IEnumerable<ItemType> ItemsWithConnections
		{
			get
			{
				return connections.Keys;
			}
		}

		public IEnumerable<Connection<ItemType>> Connections
		{
			get
			{
				return connections.Values;
			}
		}

		public ItemRate<ItemType> GetRate(ItemType item)
		{
			return rates[item];
		}

		public void SetRate(ItemType item, ItemRate<ItemType> rate)
		{
			rates[item] = rate;
		}

		public Connection<ItemType> GetConnection(ItemType item)
		{
			return connections[item];
		}

		public ItemRate<ItemType> GetItemRate(ItemType item)
		{
			return rates[item];
		}
	}
}
