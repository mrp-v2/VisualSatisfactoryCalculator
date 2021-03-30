using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.code.Production
{
	public class ProcessedPlan
	{
		private readonly HashSet<HashSet<Connection>> normalConnectionGroups;
		private readonly HashSet<Connection> abnormalConnections;
		private readonly HashSet<Step> steps;
		private readonly Dictionary<int, HashSet<Step>> tierSteps;

		public ProcessedPlan(Plan plan)
		{
			steps = plan.Steps;
			normalConnectionGroups = new HashSet<HashSet<Connection>>();
			abnormalConnections = new HashSet<Connection>();
			tierSteps = new Dictionary<int, HashSet<Step>>();
			CalculateConnectionGroups();
			CalculateStepTiers();
		}

		public IEnumerable<Connection> GetAbnormalConnections()
		{
			return abnormalConnections;
		}

		public int Tiers
		{
			get
			{
				return tierSteps.Count;
			}
		}

		public IEnumerable<Step> GetStepsInTier(int tier)
		{
			if (!tierSteps.ContainsKey(tier))
			{
				return new List<Step>();
			}
			return tierSteps[tier];
		}

		private void CalculateStepTiers()
		{
			Dictionary<Step, int> stepTiers = new Dictionary<Step, int>();
			HashSet<Step> remainingSteps = new HashSet<Step>(steps);
			// tier 0
			HashSet<Step> tier0 = new HashSet<Step>();
			foreach (Step step in remainingSteps)
			{
				if (!step.HasNormalProductConnections.Get())
				{
					tier0.Add(step);
				}
			}
			foreach (Step step in tier0)
			{
				stepTiers.Add(step, 0);
			}
			remainingSteps.ExceptWith(tier0);
			// the other tiers
			int currentTier = 1;
			HashSet<Step> previousTier = tier0;
			while (remainingSteps.Count > 0)
			{
				HashSet<Step> ingredientSteps = new HashSet<Step>();
				foreach (Step step in previousTier)
				{
					foreach (Connection connection in step.NormalIngredientConnections.Get())
					{
						ingredientSteps.UnionWith(connection.GetProducerSteps());
					}
				}
				if (currentTier == 1)
				{
					foreach (Connection connection in abnormalConnections)
					{
						ingredientSteps.UnionWith(connection.GetProducerSteps());
					}
				}
				foreach (Step step in ingredientSteps)
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
			foreach (Step step in stepTiers.Keys)
			{
				int stepTier = stepTiers[step];
				if (!tierSteps.ContainsKey(stepTier))
				{
					tierSteps.Add(stepTier, new HashSet<Step>());
				}
				tierSteps[stepTier].Add(step);
			}
			while (!tierSteps.ContainsKey(0) && tierSteps.Count > 0)
			{
				for (int i = 0; i < currentTier; i++)
				{
					if (tierSteps.ContainsKey(i + 1))
					{
						tierSteps.Add(i, tierSteps[i + 1]);
						tierSteps.Remove(i + 1);
					}
				}
			}
		}

		private void CalculateConnectionGroups()
		{
			foreach (Step step in steps)
			{
				foreach (Connection connection in step.Connections.Get())
				{
					if (abnormalConnections.Contains(connection))
					{
						continue;
					}
					foreach (HashSet<Connection> connectionGroup in normalConnectionGroups)
					{
						if (connectionGroup.Contains(connection))
						{
							goto Continue;
						}
					}
					if (connection.Type.Get() == Connection.ConnectionType.NORMAL)
					{
						foreach (HashSet<Connection> connections in normalConnectionGroups)
						{
							if (connections.First().IsConnectedNormallyTo(connection))
							{
								if (connections.Add(connection))
								{
									goto Continue;
								}
							}
						}
						normalConnectionGroups.Add(new HashSet<Connection>() { connection });
					}
					else
					{
						abnormalConnections.Add(connection);
					}
				Continue:
					continue;
				}
			}
		}
	}
}
