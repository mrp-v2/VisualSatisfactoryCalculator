using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.model.production
{
	internal class BreadthFirstSearchHandler<ItemType> where ItemType : AbstractItem
	{
		internal static void CascadeUpdates(AbstractStep<ItemType> origin, bool includeMulticonnections = true)
		{
			StartCascadingUpdate(new StepOrigin(origin, includeMulticonnections));
		}

		internal static void CascadeUpdates(Connection<ItemType> origin, bool includeMulticonnections = true)
		{
			StartCascadingUpdate(new ConnectionOrigin(origin, includeMulticonnections));
		}

		internal static HashSet<AbstractStep<ItemType>> GetSingleConnectedSteps(AbstractStep<ItemType> origin)
		{
			BreadthFirstSearchOrigin bfsOrigin = new BreadthFirstSearchOrigin(origin);
			BreadthFirstSearchOrigin.IRound currentRound = bfsOrigin.GetFirstRound();
			do
			{
				currentRound = currentRound.Execute(bfsOrigin);
			} while (currentRound.GetHasNextRound(bfsOrigin));
			return bfsOrigin.visited;
		}

		private static void StartCascadingUpdate(CascadingUpdatesOrigin origin)
		{
			CascadingUpdatesOrigin.IRound currentRound = origin.GetFirstRound();
			do
			{
				currentRound = currentRound.Execute(origin);
			} while (currentRound.GetHasNextRound(origin));
		}

		private abstract class AbstractOrigin<OriginType, VisitedType> where OriginType : AbstractOrigin<OriginType, VisitedType>
		{
			public readonly HashSet<VisitedType> visited;
			public readonly bool includeMulticonnections;

			public AbstractOrigin(bool includeMulticonnections)
			{
				visited = new HashSet<VisitedType>();
				this.includeMulticonnections = includeMulticonnections;
			}

			public abstract IRound GetFirstRound();

			public interface IRound
			{
				bool GetHasNextRound(OriginType data);
				IRound Execute(OriginType data);
			}

			public abstract class Round<CurrentType, NextType> : IRound where CurrentType : VisitedType where NextType : VisitedType
			{
				private readonly HashSet<CurrentType> toVisit;
				protected readonly HashSet<NextType> nextRound;

				protected Round(HashSet<CurrentType> toVisit)
				{
					this.toVisit = toVisit;
					nextRound = new HashSet<NextType>();
				}

				public abstract bool GetHasNextRound(OriginType data);

				protected abstract void Visit(CurrentType obj, OriginType data);

				protected abstract Round<NextType, CurrentType> NextRound(OriginType data);

				public IRound Execute(OriginType data)
				{
					foreach (CurrentType obj in toVisit)
					{
						if (!data.visited.Contains(obj))
						{
							Visit(obj, data);
							data.visited.Add(obj);
						}
					}
					return NextRound(data);
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

		private class BreadthFirstSearchOrigin : AbstractOrigin<BreadthFirstSearchOrigin, AbstractStep<ItemType>>
		{
			private readonly AbstractStep<ItemType> origin;

			public BreadthFirstSearchOrigin(AbstractStep<ItemType> origin) : base(false)
			{
				this.origin = origin;
			}

			public override IRound GetFirstRound()
			{
				return new StepsOnlyRound(new HashSet<AbstractStep<ItemType>> { origin });
			}
		}

		private abstract class CascadingUpdatesOrigin : AbstractOrigin<CascadingUpdatesOrigin, object>
		{
			public readonly HashSet<Connection<ItemType>> multiconnectionsToVisit;

			public CascadingUpdatesOrigin(bool includeMulticonnections) : base(includeMulticonnections) { }

			public override abstract IRound GetFirstRound();
		}

		private class StepOrigin : CascadingUpdatesOrigin
		{
			private readonly AbstractStep<ItemType> origin;

			public StepOrigin(AbstractStep<ItemType> origin, bool includeMulticonnections) : base(includeMulticonnections)
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

		private class ConnectionOrigin : CascadingUpdatesOrigin
		{
			private readonly Connection<ItemType> origin;

			public ConnectionOrigin(Connection<ItemType> origin, bool includeMulticonnections) : base(includeMulticonnections)
			{
				this.origin = origin;
			}

			public override IRound GetFirstRound()
			{
				return new StepRound(new HashSet<AbstractStep<ItemType>>(origin.Steps));
			}
		}

		private class ConnectionRound : CascadingUpdatesOrigin.Round<Connection<ItemType>, AbstractStep<ItemType>>
		{
			public ConnectionRound(HashSet<Connection<ItemType>> connections) : base(connections) { }

			public override bool GetHasNextRound(CascadingUpdatesOrigin data)
			{
				return nextRound.Count > 0;
			}

			protected override CascadingUpdatesOrigin.Round<AbstractStep<ItemType>, Connection<ItemType>> NextRound(CascadingUpdatesOrigin data)
			{
				return new StepRound(nextRound);
			}

			protected override void Visit(Connection<ItemType> obj, CascadingUpdatesOrigin data)
			{
				obj.UpdateRatesFrom(data.visited, nextRound);
			}
		}

		private class StepsOnlyRound : BreadthFirstSearchOrigin.Round<AbstractStep<ItemType>, AbstractStep<ItemType>>
		{
			public StepsOnlyRound(HashSet<AbstractStep<ItemType>> steps) : base(steps) { }

			public override bool GetHasNextRound(BreadthFirstSearchOrigin data)
			{
				return nextRound.Count > 0;
			}

			protected override BreadthFirstSearchOrigin.Round<AbstractStep<ItemType>, AbstractStep<ItemType>> NextRound(BreadthFirstSearchOrigin data)
			{
				return new StepsOnlyRound(nextRound);
			}

			protected override void Visit(AbstractStep<ItemType> obj, BreadthFirstSearchOrigin data)
			{
				foreach (Connection<ItemType> connection in obj.Connections)
				{
					if (connection.Type != ConnectionType.SINGLE)
					{
						continue;
					}
					foreach (AbstractStep<ItemType> step in connection.Steps)
					{
						if (!data.visited.Contains(step))
						{
							nextRound.Add(step);
						}
					}
				}
			}
		}

		private class StepRound : CascadingUpdatesOrigin.Round<AbstractStep<ItemType>, Connection<ItemType>>
		{
			public StepRound(HashSet<AbstractStep<ItemType>> steps) : base(steps) { }

			public override bool GetHasNextRound(CascadingUpdatesOrigin data)
			{
				return nextRound.Count > 0 || (data.includeMulticonnections && data.multiconnectionsToVisit.Count > 0);
			}

			protected override CascadingUpdatesOrigin.Round<Connection<ItemType>, AbstractStep<ItemType>> NextRound(CascadingUpdatesOrigin data)
			{
				if (nextRound.Count > 0 || !data.includeMulticonnections)
				{
					return new ConnectionRound(nextRound);
				}
				else
				{
					HashSet<Connection<ItemType>> fewestRemainingStepMulticonnections = new HashSet<Connection<ItemType>>();
					uint fewestRemainingSteps = uint.MaxValue;
					foreach (Connection<ItemType> multiconnection in data.multiconnectionsToVisit)
					{
						uint remaining = multiconnection.GetNonUpdatedStepCount(data.visited);
						if (remaining < fewestRemainingSteps)
						{
							fewestRemainingStepMulticonnections.Clear();
							fewestRemainingSteps = remaining;
							fewestRemainingStepMulticonnections.Add(multiconnection);
						}
						else if (remaining == fewestRemainingSteps)
						{
							fewestRemainingStepMulticonnections.Add(multiconnection);
						}
					}
					return new ConnectionRound(fewestRemainingStepMulticonnections);
				}
			}

			protected override void Visit(AbstractStep<ItemType> obj, CascadingUpdatesOrigin data)
			{
				obj.UpdateRatesFrom(data.visited);
				FilterConnections(obj, data.visited, nextRound, data.multiconnectionsToVisit);
			}
		}
	}
}
