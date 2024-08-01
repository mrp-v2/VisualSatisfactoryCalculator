using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.model.production
{
	internal class CascadingUpdateHandler<ItemType> where ItemType : AbstractItem
	{
		private readonly HashSet<object> visited;
		private readonly HashSet<Connection<ItemType>> multiconnectionsToVisit;

		internal static void CascadeUpdates(AbstractStep<ItemType> origin)
		{
			Start(new StepOrigin(origin));
		}

		internal static void CascadeUpdates(Connection<ItemType> origin)
		{
			Start(new ConnectionOrigin(origin));
		}

		private static void Start(Origin origin)
		{
			IRound currentRound = origin.GetFirstRound();
			do
			{
				currentRound = currentRound.Execute(origin.visited, origin.multiconnectionsToVisit);
			} while (currentRound.HasNextRound);
		}

		private abstract class Origin : CascadingUpdateHandler<ItemType>
		{
			public abstract IRound GetFirstRound();
		}

		private class StepOrigin : Origin
		{
			private readonly AbstractStep<ItemType> origin;

			public StepOrigin(AbstractStep<ItemType> origin)
			{
				this.origin = origin;
			}

			public override IRound GetFirstRound()
			{
				HashSet<Connection<ItemType>> connections = new HashSet<Connection<ItemType>>();
				StepRound.FilterConnections(origin, new HashSet<object>(), connections, multiconnectionsToVisit);
				return new ConnectionRound(connections);
			}
		}

		private class ConnectionOrigin : Origin
		{
			private readonly Connection<ItemType> origin;

			public ConnectionOrigin(Connection<ItemType> origin)
			{
				this.origin = origin;
			}

			public override IRound GetFirstRound()
			{
				return new StepRound(new HashSet<AbstractStep<ItemType>>(origin.Steps));
			}
		}

		private abstract class Round<CurrentType, NextType> : IRound
		{
			private readonly HashSet<CurrentType> toVisit;
			protected readonly HashSet<NextType> nextRound;

			protected Round(HashSet<CurrentType> toVisit)
			{
				this.toVisit = toVisit;
				nextRound = new HashSet<NextType>();
			}

			public bool HasNextRound
			{
				get
				{
					return nextRound.Count > 0;
				}
			}

			protected abstract void Visit(CurrentType obj, HashSet<object> visited, HashSet<Connection<ItemType>> multiconnectionsToVisit);

			protected abstract Round<NextType, CurrentType> NextRound(HashSet<Connection<ItemType>> multiconnectionsToVisit);

			public IRound Execute(HashSet<object> visited, HashSet<Connection<ItemType>> multiconnectionsToVisit)
			{
				foreach (CurrentType obj in toVisit)
				{
					if (!visited.Contains(obj))
					{
						Visit(obj, visited, multiconnectionsToVisit);
						visited.Add(obj);
					}
				}
				return NextRound(multiconnectionsToVisit);
			}
		}

		private interface IRound
		{
			bool HasNextRound { get; }
			IRound Execute(HashSet<object> visited, HashSet<Connection<ItemType>> multiconnectionsToVisit);
		}

		private class ConnectionRound : Round<Connection<ItemType>, AbstractStep<ItemType>>
		{
			public ConnectionRound(HashSet<Connection<ItemType>> connections) : base(connections) { }

			protected override Round<AbstractStep<ItemType>, Connection<ItemType>> NextRound(HashSet<Connection<ItemType>> multiconnectionsToVisit)
			{
				return new StepRound(nextRound);
			}

			protected override void Visit(Connection<ItemType> obj, HashSet<object> visited, HashSet<Connection<ItemType>> multiconnectionsToVisit)
			{
				obj.UpdateRatesFrom(visited, nextRound);
			}
		}

		private class StepRound : Round<AbstractStep<ItemType>, Connection<ItemType>>
		{
			public StepRound(HashSet<AbstractStep<ItemType>> steps) : base(steps) { }

			protected override Round<Connection<ItemType>, AbstractStep<ItemType>> NextRound(HashSet<Connection<ItemType>> multiconnectionsToVisit)
			{
				if (nextRound.Count > 0)
				{
					return new ConnectionRound(nextRound);
				}
				else
				{
					return new ConnectionRound(multiconnectionsToVisit);
				}
			}

			protected override void Visit(AbstractStep<ItemType> obj, HashSet<object> visited, HashSet<Connection<ItemType>> multiconnectionsToVisit)
			{
				obj.UpdateRatesFrom(visited);
				FilterConnections(obj, visited, nextRound, multiconnectionsToVisit);
			}

			public static void FilterConnections(AbstractStep<ItemType> step, HashSet<object> visited, HashSet<Connection<ItemType>> toVisit, HashSet<Connection<ItemType>> multiconnectionsToVisit)
			{
				foreach (Connection<ItemType> connection in step.Connections)
				{
					if (!visited.Contains(connection))
					{
						switch (connection.Type)
						{
							case ConnectionType.SINGLE:
								toVisit.Add(connection);
								break;
							case ConnectionType.MULTI:
								multiconnectionsToVisit.Add(connection);
								break;
						}
					}
				}
			}
		}
	}
}
