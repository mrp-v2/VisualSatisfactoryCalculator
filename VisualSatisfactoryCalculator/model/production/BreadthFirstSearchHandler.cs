using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.model.production
{
	internal class BreadthFirstSearchHandler<ItemType, RecipeType> where ItemType : BasicItem
	{
		/// <summary>
		/// Starts cascading updates from an origin step.
		/// </summary>
		/// <param name="includeMulticonnections">Whether to include connections that are <see cref="ConnectionType.MULTI"/></param>
		internal static void CascadeUpdates(AbstractStep<ItemType, RecipeType> origin, bool includeMulticonnections = true)
		{
			StartCascadingUpdate(new StepOrigin(origin, includeMulticonnections));
		}

		/// <summary>
		/// Starts cascading updates from an origin connection
		/// </summary>
		/// <param name="includeMulticonnections">Whether to include connections that are <see cref="ConnectionType.MULTI"/></param>
		internal static void CascadeUpdates(Connection<ItemType, RecipeType> origin, bool includeMulticonnections = true)
		{
			StartCascadingUpdate(new ConnectionOrigin(origin, includeMulticonnections));
		}

		/// <summary>
		/// Finds all steps that can be reached using only <see cref="ConnectionType.SINGLE"/> connections from an origin step.
		/// </summary>
		internal static HashSet<AbstractStep<ItemType, RecipeType>> GetSingleConnectedSteps(AbstractStep<ItemType, RecipeType> origin)
		{
			BreadthFirstSearchOrigin bfsOrigin = new BreadthFirstSearchOrigin(origin);
			BreadthFirstSearchOrigin.IRound currentRound = bfsOrigin.GetFirstRound();
			do
			{
				currentRound = currentRound.Execute(bfsOrigin);
			} while (currentRound.GetHasNextRound(bfsOrigin));
			return bfsOrigin.visited;
		}

		/// <summary>
		/// Generic entry point for cascading updates of all origin types.
		/// </summary>
		private static void StartCascadingUpdate(CascadingUpdatesOrigin origin)
		{
			CascadingUpdatesOrigin.IRound currentRound = origin.GetFirstRound();
			do
			{
				currentRound = currentRound.Execute(origin);
			} while (currentRound.GetHasNextRound(origin));
		}

		/// <summary>
		/// The base class for all origin types.
		/// </summary>
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

			/// <summary>
			/// The basic interface requiredments for a round of searching.
			/// </summary>
			public interface IRound
			{
				bool GetHasNextRound(OriginType data);
				IRound Execute(OriginType data);
			}

			/// <summary>
			/// The base class for all round types.
			/// </summary>
			public abstract class Round<CurrentType, NextType> : IRound where CurrentType : VisitedType where NextType : VisitedType
			{
				private readonly HashSet<CurrentType> _toVisit;
				protected readonly HashSet<NextType> nextRound;

				protected Round(HashSet<CurrentType> toVisit)
				{
					_toVisit = toVisit;
					nextRound = new HashSet<NextType>();
				}

				/// <summary>
				/// Whether this round has another round to execute afterwards.
				/// </summary>
				public abstract bool GetHasNextRound(OriginType data);

				/// <summary>
				/// Visits an object, likely updating it somehow.
				/// </summary>
				protected abstract void Visit(CurrentType obj, OriginType data);

				/// <summary>
				/// Gets the next round to execute.
				/// </summary>
				protected abstract Round<NextType, CurrentType> NextRound(OriginType data);

				public IRound Execute(OriginType data)
				{
					foreach (CurrentType obj in _toVisit)
					{
						if (!data.visited.Contains(obj))
						{
							Visit(obj, data);
							data.visited.Add(obj);
						}
					}
					return NextRound(data);
				}

				/// <summary>
				/// Filters unvisited connections of a step into <see cref="ConnectionType.SINGLE"/> and <see cref="ConnectionType.MULTI"/> connections.
				/// </summary>
				public static void FilterConnections(AbstractStep<ItemType, RecipeType> step, HashSet<object> visited, HashSet<Connection<ItemType, RecipeType>> toVisit, HashSet<Connection<ItemType, RecipeType>> multiconnectionsToVisit)
				{
					foreach (Connection<ItemType, RecipeType> connection in step.Connections)
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

		/// <summary>
		/// The origin for performng a breadth first search of <see cref="ConnectionType.SINGLE"/> connections.
		/// </summary>
		private class BreadthFirstSearchOrigin : AbstractOrigin<BreadthFirstSearchOrigin, AbstractStep<ItemType, RecipeType>>
		{
			private readonly AbstractStep<ItemType, RecipeType> _origin;

			public BreadthFirstSearchOrigin(AbstractStep<ItemType, RecipeType> origin) : base(false)
			{
				_origin = origin;
			}

			public override IRound GetFirstRound()
			{
				return new StepsOnlyRound(new HashSet<AbstractStep<ItemType, RecipeType>> { _origin });
			}
		}

		/// <summary>
		/// The base origin for cascading updates of rates.
		/// </summary>
		private abstract class CascadingUpdatesOrigin : AbstractOrigin<CascadingUpdatesOrigin, object>
		{
			public readonly HashSet<Connection<ItemType, RecipeType>> multiconnectionsToVisit;

			public CascadingUpdatesOrigin(bool includeMulticonnections) : base(includeMulticonnections) { }

			public override abstract IRound GetFirstRound();
		}

		/// <summary>
		/// The origin for cascading updates beginning at step.
		/// </summary>
		private class StepOrigin : CascadingUpdatesOrigin
		{
			private readonly AbstractStep<ItemType, RecipeType> _origin;

			public StepOrigin(AbstractStep<ItemType, RecipeType> origin, bool includeMulticonnections) : base(includeMulticonnections)
			{
				_origin = origin;
			}

			public override IRound GetFirstRound()
			{
				HashSet<Connection<ItemType, RecipeType>> connections = new HashSet<Connection<ItemType, RecipeType>>();
				StepRound.FilterConnections(_origin, new HashSet<object>(), connections, multiconnectionsToVisit);
				return new ConnectionRound(connections);
			}
		}

		/// <summary>
		/// The origin for cascading updates beginning at a connection.
		/// </summary>
		private class ConnectionOrigin : CascadingUpdatesOrigin
		{
			private readonly Connection<ItemType, RecipeType> _origin;

			public ConnectionOrigin(Connection<ItemType, RecipeType> origin, bool includeMulticonnections) : base(includeMulticonnections)
			{
				_origin = origin;
			}

			public override IRound GetFirstRound()
			{
				return new StepRound(new HashSet<AbstractStep<ItemType, RecipeType>>(_origin.Steps));
			}
		}

		/// <summary>
		/// A round of searching through connections, that yields a round of steps.
		/// </summary>
		private class ConnectionRound : CascadingUpdatesOrigin.Round<Connection<ItemType, RecipeType>, AbstractStep<ItemType, RecipeType>>
		{
			public ConnectionRound(HashSet<Connection<ItemType, RecipeType>> connections) : base(connections) { }

			public override bool GetHasNextRound(CascadingUpdatesOrigin data)
			{
				return nextRound.Count > 0;
			}

			protected override CascadingUpdatesOrigin.Round<AbstractStep<ItemType, RecipeType>, Connection<ItemType, RecipeType>> NextRound(CascadingUpdatesOrigin data)
			{
				return new StepRound(nextRound);
			}

			protected override void Visit(Connection<ItemType, RecipeType> obj, CascadingUpdatesOrigin data)
			{
				obj.UpdateRatesFrom(data.visited, nextRound);
			}
		}

		/// <summary>
		/// A round of searching through steps, that yields another round of steps.
		/// </summary>
		private class StepsOnlyRound : BreadthFirstSearchOrigin.Round<AbstractStep<ItemType, RecipeType>, AbstractStep<ItemType, RecipeType>>
		{
			public StepsOnlyRound(HashSet<AbstractStep<ItemType, RecipeType>> steps) : base(steps) { }

			public override bool GetHasNextRound(BreadthFirstSearchOrigin data)
			{
				return nextRound.Count > 0;
			}

			protected override BreadthFirstSearchOrigin.Round<AbstractStep<ItemType, RecipeType>, AbstractStep<ItemType, RecipeType>> NextRound(BreadthFirstSearchOrigin data)
			{
				return new StepsOnlyRound(nextRound);
			}

			protected override void Visit(AbstractStep<ItemType, RecipeType> obj, BreadthFirstSearchOrigin data)
			{
				foreach (Connection<ItemType, RecipeType> connection in obj.Connections)
				{
					if (connection.Type != ConnectionType.SINGLE)
					{
						continue;
					}
					foreach (AbstractStep<ItemType, RecipeType> step in connection.Steps)
					{
						if (!data.visited.Contains(step))
						{
							nextRound.Add(step);
						}
					}
				}
			}
		}

		/// <summary>
		/// A round of searching through steps, that yields a round of connections.
		/// </summary>
		private class StepRound : CascadingUpdatesOrigin.Round<AbstractStep<ItemType, RecipeType>, Connection<ItemType, RecipeType>>
		{
			public StepRound(HashSet<AbstractStep<ItemType, RecipeType>> steps) : base(steps) { }

			public override bool GetHasNextRound(CascadingUpdatesOrigin data)
			{
				return nextRound.Count > 0 || (data.includeMulticonnections && data.multiconnectionsToVisit.Count > 0);
			}

			protected override CascadingUpdatesOrigin.Round<Connection<ItemType, RecipeType>, AbstractStep<ItemType, RecipeType>> NextRound(CascadingUpdatesOrigin data)
			{
				if (nextRound.Count > 0 || !data.includeMulticonnections)
				{
					return new ConnectionRound(nextRound);
				}
				else
				{
					HashSet<Connection<ItemType, RecipeType>> fewestRemainingStepMulticonnections = new HashSet<Connection<ItemType, RecipeType>>();
					uint fewestRemainingSteps = uint.MaxValue;
					foreach (Connection<ItemType, RecipeType> multiconnection in data.multiconnectionsToVisit)
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

			protected override void Visit(AbstractStep<ItemType, RecipeType> obj, CascadingUpdatesOrigin data)
			{
				obj.UpdateRatesFrom(data.visited);
				FilterConnections(obj, data.visited, nextRound, data.multiconnectionsToVisit);
			}
		}
	}
}
