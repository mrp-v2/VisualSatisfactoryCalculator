using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

using VisualSatisfactoryCalculator.util.collections;
using VisualSatisfactoryCalculator.util.extensions;

namespace VisualSatisfactoryCalculator.model.production
{
	public class ProcessedPlan<StepType, ItemType, RecipeType> where StepType : AbstractStep<ItemType, StepType, RecipeType> where ItemType : BasicItem
	{
		public readonly IReadOnlyDictionary<StepType, WellConnectedStepGroup> stepGroupMap;
		public readonly IReadOnlySet<Connection<ItemType, StepType, RecipeType>> connections;
		public readonly IReadOnlySet<Connection<ItemType, StepType, RecipeType>> multiconnections;
		public readonly IReadOnlyDictionary<int, IReadOnlySet<StepType>> tierStepsMap;

		public ProcessedPlan(IReadOnlySet<StepType> steps)
		{
			(stepGroupMap, multiconnections, connections) = ProcessConnections(steps);
			tierStepsMap = CalculateStepTiers(steps);
		}

		private IReadOnlyDictionary<int, IReadOnlySet<StepType>> CalculateStepTiers(IReadOnlySet<StepType> steps)
		{
			Dictionary<StepType, int> stepTiers = new Dictionary<StepType, int>();
			HashSet<StepType> remainingSteps = new HashSet<StepType>(steps);
			// tier 0
			HashSet<StepType> tier0 = new HashSet<StepType>();
			foreach (StepType step in remainingSteps)
			{
				if (!step.HasSingleConnectionProduct)
				{
					tier0.Add(step);
				}
			}
			foreach (StepType step in tier0)
			{
				stepTiers.Add(step, 0);
			}
			remainingSteps.ExceptWith(tier0);
			// the other tiers
			int currentTier = 1;
			HashSet<StepType> previousTier = tier0;
			while (remainingSteps.Count > 0)
			{
				HashSet<StepType> ingredientSteps = new HashSet<StepType>();
				foreach (StepType step in previousTier)
				{
					foreach (Connection<ItemType, StepType, RecipeType> connection in step.SingleConnectionIngredients)
					{
						ingredientSteps.UnionWith(connection.ProducerSteps);
					}
				}
				if (currentTier == 1)
				{
					foreach (Connection<ItemType, StepType, RecipeType> connection in multiconnections)
					{
						ingredientSteps.UnionWith(connection.ProducerSteps);
					}
				}
				foreach (StepType step in ingredientSteps)
				{
					if (stepTiers.ContainsKey(step))
					{
						stepTiers.Remove(step);
					}
					stepTiers.Add(step, currentTier);
				}
				if (ingredientSteps.Count == 0)
				{
					throw new InvalidOperationException();
				}
				previousTier = ingredientSteps;
				currentTier++;
				remainingSteps.ExceptWith(ingredientSteps);
			}
			// assemble final tier list
			Dictionary<int, ViewableSet<StepType>> tierStepsMap = new Dictionary<int, ViewableSet<StepType>>();
			foreach (StepType step in stepTiers.Keys)
			{
				int stepTier = stepTiers[step];
				if (!tierStepsMap.ContainsKey(stepTier))
				{
					tierStepsMap.Add(stepTier, new ViewableSet<StepType>());
				}
				tierStepsMap[stepTier].Add(step);
			}
			while (!tierStepsMap.ContainsKey(0) && tierStepsMap.Count > 0)
			{
				for (int i = 0; i < currentTier; i++)
				{
					if (tierStepsMap.ContainsKey(i + 1))
					{
						tierStepsMap.Add(i, tierStepsMap[i + 1]);
						tierStepsMap.Remove(i + 1);
					}
				}
			}
			ViewableDictionary<int, IReadOnlySet<StepType>> finalStepTiers = new ViewableDictionary<int, IReadOnlySet<StepType>>();
			foreach (int tier in tierStepsMap.Keys)
			{
				finalStepTiers.Add(tier, tierStepsMap[tier].ReadOnly);
			}
			return finalStepTiers.ReadOnly;
		}

		/// <returns>
		/// step group map
		/// multiconnections
		/// all connections
		/// </returns>
		private (IReadOnlyDictionary<StepType, WellConnectedStepGroup>, IReadOnlySet<Connection<ItemType, StepType, RecipeType>>, IReadOnlySet<Connection<ItemType, StepType, RecipeType>>) ProcessConnections(IReadOnlySet<StepType> steps)
		{
			ProcessSingleConnections(steps, out ViewableDictionary<StepType, HashSet<StepType>> stepGroupMap, out ViewableSet<HashSet<StepType>> wellConnectedStepGroups, out ViewableSet<Connection<ItemType, StepType, RecipeType>> multiconnections, out ViewableSet<Connection<ItemType, StepType, RecipeType>> allConnections);

			IReadOnlyDictionary<StepType, WellConnectedStepGroup> groupMap = ProcessMulticonnections(multiconnections.ReadOnly, wellConnectedStepGroups, stepGroupMap);

			return (groupMap, multiconnections.ReadOnly, allConnections.ReadOnly);
		}

		private void ProcessSingleConnections(IReadOnlySet<StepType> steps, out ViewableDictionary<StepType, HashSet<StepType>> stepGroupMap, out ViewableSet<HashSet<StepType>> wellConnectedStepGroups, out ViewableSet<Connection<ItemType, StepType, RecipeType>> multiconnections, out ViewableSet<Connection<ItemType, StepType, RecipeType>> processedConnections)
		{
			HashSet<StepType> stepsToExplore = new HashSet<StepType>(steps);

			stepGroupMap = new ViewableDictionary<StepType, HashSet<StepType>>();
			wellConnectedStepGroups = new ViewableSet<HashSet<StepType>>();
			multiconnections = new ViewableSet<Connection<ItemType, StepType, RecipeType>>();
			processedConnections = new ViewableSet<Connection<ItemType, StepType, RecipeType>>();

			HashSet<Connection<ItemType, StepType, RecipeType>> currentRound = new HashSet<Connection<ItemType, StepType, RecipeType>>();
			HashSet<Connection<ItemType, StepType, RecipeType>> nextRound = new HashSet<Connection<ItemType, StepType, RecipeType>>();

			while (stepsToExplore.Count > 0)
			{
				StepType firstStep = stepsToExplore.First();
				if (firstStep.Connections.Count() == 0)
				{
					stepsToExplore.Remove(firstStep);
					stepGroupMap.Add(firstStep, new HashSet<StepType> { firstStep });
					wellConnectedStepGroups.Add(stepGroupMap[firstStep]);
					continue;
				}
				currentRound.UnionWith(stepsToExplore.First().Connections);

				while (currentRound.Count > 0)
				{
					foreach (Connection<ItemType, StepType, RecipeType> connection in currentRound)
					{
						ProcessSingleConnection(connection, stepGroupMap, wellConnectedStepGroups, multiconnections, processedConnections, nextRound, stepsToExplore);
					}
					(nextRound, currentRound) = (currentRound, nextRound);
					nextRound.Clear();
				}
			}
		}

		private void ProcessSingleConnection(Connection<ItemType, StepType, RecipeType> connection, Dictionary<StepType, HashSet<StepType>> stepGroupMap, HashSet<HashSet<StepType>> wellConnectedStepGroups, HashSet<Connection<ItemType, StepType, RecipeType>> multiconnections, HashSet<Connection<ItemType, StepType, RecipeType>> processedConnections, HashSet<Connection<ItemType, StepType, RecipeType>> nextRound, HashSet<StepType> stepsToExplore)
		{
			processedConnections.Add(connection);
			switch (connection.Type)
			{
				case ConnectionType.SINGLE:
					HashSet<HashSet<StepType>> connectedGroups = new HashSet<HashSet<StepType>>();
					foreach (StepType step in connection.Steps)
					{
						if (!stepGroupMap.ContainsKey(step))
						{
							stepGroupMap.Add(step, new HashSet<StepType> { step });
						}
						connectedGroups.Add(stepGroupMap[step]);
					}
					if (connectedGroups.Count > 1)
					{
						HashSet<StepType> combined = new HashSet<StepType>();
						foreach (HashSet<StepType> group in connectedGroups)
						{
							combined.AddRange(group);
							wellConnectedStepGroups.Remove(group);
						}
						wellConnectedStepGroups.Add(combined);
						foreach (StepType step in combined)
						{
							stepGroupMap[step] = combined;
						}
					}
					break;
				case ConnectionType.MULTI:
					foreach (StepType step in connection.Steps)
					{
						if (!stepGroupMap.ContainsKey(step))
						{
							stepGroupMap.Add(step, new HashSet<StepType> { step });
							wellConnectedStepGroups.Add(stepGroupMap[step]);
						}
					}
					multiconnections.Add(connection);
					break;
			}
			foreach (StepType step in connection.Steps)
			{
				stepsToExplore.Remove(step);
				foreach (Connection<ItemType, StepType, RecipeType> other in step.Connections)
				{
					if (!processedConnections.Contains(other))
					{
						nextRound.Add(other);
					}
				}
			}
		}

		private IReadOnlyDictionary<StepType, WellConnectedStepGroup> ProcessMulticonnections(IReadOnlySet<Connection<ItemType, StepType, RecipeType>> multiconnections, ViewableSet<HashSet<StepType>> wellConnectedStepGroups, ViewableDictionary<StepType, HashSet<StepType>> stepGroupMap)
		{
			ViewableDictionary<HashSet<StepType>, HashSet<Connection<ItemType, StepType, RecipeType>>> groupMulticonnections = new ViewableDictionary<HashSet<StepType>, HashSet<Connection<ItemType, StepType, RecipeType>>>();
			HashSet<Connection<ItemType, StepType, RecipeType>> remainingMulticonnections = new HashSet<Connection<ItemType, StepType, RecipeType>>(multiconnections);
			bool changed;
			do
			{
				changed = false;
				groupMulticonnections.Clear();
				HashSet<Connection<ItemType, StepType, RecipeType>> solvedMulticonnections = new HashSet<Connection<ItemType, StepType, RecipeType>>();
				foreach (Connection<ItemType, StepType, RecipeType> connection in remainingMulticonnections)
				{
					HashSet<HashSet<StepType>> connectedGroups = new HashSet<HashSet<StepType>>();
					foreach (StepType step in connection.Steps)
					{
						connectedGroups.Add(stepGroupMap[step]);
					}
					switch (connectedGroups.Count)
					{
						case 1:
							solvedMulticonnections.Add(connection);
							changed = true;
							break;
						case 2:
							HashSet<StepType> merged = new HashSet<StepType>();
							foreach (HashSet<StepType> group in connectedGroups)
							{
								merged.UnionWith(group);
								wellConnectedStepGroups.Remove(group);
							}
							wellConnectedStepGroups.Add(merged);
							foreach (StepType step in merged)
							{
								stepGroupMap[step] = merged;
							}
							goto case 1;
						default:
							foreach (HashSet<StepType> group in connectedGroups)
							{
								if (!groupMulticonnections.ContainsKey(group))
								{
									groupMulticonnections.Add(group, new HashSet<Connection<ItemType, StepType, RecipeType>>());
								}
								groupMulticonnections[group].Add(connection);
							}
							break;
					}
				}
				remainingMulticonnections.ExceptWith(solvedMulticonnections);
			} while (changed);
			return FinalizeGroupMap(wellConnectedStepGroups.ReadOnly, groupMulticonnections.ReadOnly, stepGroupMap.ReadOnly);
		}

		private IReadOnlyDictionary<StepType, WellConnectedStepGroup> FinalizeGroupMap(IReadOnlySet<HashSet<StepType>> wellConnectedStepGroups, IReadOnlyDictionary<HashSet<StepType>, HashSet<Connection<ItemType, StepType, RecipeType>>> groupMulticonnections, IReadOnlyDictionary<StepType, HashSet<StepType>> oldStepGroupMap)
		{
			Dictionary<StepType, WellConnectedStepGroup> stepGroupMap = new Dictionary<StepType, WellConnectedStepGroup>();
			foreach (HashSet<StepType> group in wellConnectedStepGroups)
			{
				bool editable = true;
				if (groupMulticonnections.ContainsKey(group))
				{
					foreach (Connection<ItemType, StepType, RecipeType> multiconnection in groupMulticonnections[group])
					{
						(HashSet<HashSet<StepType>>, HashSet<HashSet<StepType>>) result = multiconnection.GetStepGroupsAsProducersAndConsumers(oldStepGroupMap);
						if (result.Item1.Contains(group) && result.Item2.Count > 1)
						{
							editable = false;
							break;
						}
						else if (result.Item2.Contains(group) && result.Item1.Count > 1)
						{
							editable = false;
							break;
						}
					}
				}
				WellConnectedStepGroup stepGroup = new WellConnectedStepGroup(editable);
				foreach (StepType step in group)
				{
					stepGroupMap.Add(step, stepGroup);
				}
			}
			return stepGroupMap.ToImmutableDictionary();
		}
	}
}
