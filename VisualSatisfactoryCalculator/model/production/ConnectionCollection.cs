using System.Collections.Generic;

namespace VisualSatisfactoryCalculator.model.production
{
	/// <summary>
	/// A map of items to connections and counts.
	/// </summary>
	public sealed class ConnectionCollection<ItemType, StepType, RecipeType> where ItemType : BasicItem where StepType : AbstractStep<ItemType, StepType, RecipeType>
	{
		public delegate void OnConnectionChanged();

		private readonly Dictionary<ItemType, Connection<ItemType, StepType, RecipeType>> _connections;
		private OnConnectionChanged _connectionsChangedListener;

		public ConnectionCollection()
		{
			_connections = new Dictionary<ItemType, Connection<ItemType, StepType, RecipeType>>();
		}

		/// <summary>
		/// Add a callback for if a connection is added or removed.
		/// </summary>
		/// <param name="listener"></param>
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

		public IEnumerable<Connection<ItemType, StepType, RecipeType>> Connections
		{
			get
			{
				return _connections.Values;
			}
		}

		public Connection<ItemType, StepType, RecipeType> GetConnection(ItemType item)
		{
			return _connections[item];
		}

		public void AddConnection(Connection<ItemType, StepType, RecipeType> connection)
		{
			_connections[connection.item] = connection;
			_connectionsChangedListener?.Invoke();
		}

		public void RemoveConnection(Connection<ItemType, StepType, RecipeType> connection)
		{
			_connections.Remove(connection.item);
			_connectionsChangedListener?.Invoke();
		}
	}
}
