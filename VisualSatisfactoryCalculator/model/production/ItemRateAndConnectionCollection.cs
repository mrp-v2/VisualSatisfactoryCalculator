using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VisualSatisfactoryCalculator.satisfactory.Numbers;

namespace VisualSatisfactoryCalculator.model.production
{
	public sealed class ItemRateAndConnectionCollection<ItemType, RecipeType> where ItemType : BasicItem
	{
		public delegate void OnConnectionChanged();

		private readonly ItemRateCollection<ItemType> _rates;
		private readonly Dictionary<ItemType, Connection<ItemType, RecipeType>> _connections;
		private OnConnectionChanged _connectionsChangedListener;

		public ItemRateAndConnectionCollection()
		{
			_rates = new ItemRateCollection<ItemType>();
			_connections = new Dictionary<ItemType, Connection<ItemType, RecipeType>>();
		}

		public void SetConnectionsChangedListener(OnConnectionChanged listener)
		{
			_connectionsChangedListener = listener;
		}

		public IEnumerable<ItemType> ItemsWithConnections
		{
			get
			{
				return _connections.Keys;
			}
		}

		public bool HasConnection(ItemType item)
		{
			return _connections.ContainsKey(item);
		}

		public IEnumerable<Connection<ItemType, RecipeType>> Connections
		{
			get
			{
				return _connections.Values;
			}
		}

		public ItemCount<ItemType> GetRate(ItemType item)
		{
			return _rates[item];
		}

		public void SetRate(ItemType item, ItemCount<ItemType> rate)
		{
			_rates[item] = rate;
		}

		public Connection<ItemType, RecipeType> GetConnection(ItemType item)
		{
			return _connections[item];
		}

		public void AddConnection(Connection<ItemType, RecipeType> connection)
		{
			_connections[connection.item] = connection;
			_connectionsChangedListener?.Invoke();
		}

		public void RemoveConnection(Connection<ItemType, RecipeType> connection)
		{
			_connections.Remove(connection.item);
			_connectionsChangedListener?.Invoke();
		}
	}
}
