using System;
using System.Collections.Generic;
using System.Linq;

using Connection = VisualSatisfactoryCalculator.model.production.Connection<VisualSatisfactoryCalculator.satisfactory.JSONClasses.JSONItem, VisualSatisfactoryCalculator.satisfactory.Production.Step, VisualSatisfactoryCalculator.satisfactory.DataStorage.BasicRecipe>;

namespace VisualSatisfactoryCalculator.satisfactory.Production
{
	public class ProcessedPlan
	{
		private readonly HashSet<HashSet<Connection>> _normalConnectionGroups;
		private readonly HashSet<Connection> _abnormalConnections;
		private readonly HashSet<Step> _steps;
		private readonly Dictionary<int, HashSet<Step>> _tierSteps;
		private readonly HashSet<Connection> _allConnections;

		public ProcessedPlan(Plan plan)
		{
			_steps = plan.steps;
			_normalConnectionGroups = new HashSet<HashSet<Connection>>();
			_abnormalConnections = new HashSet<Connection>();
			_tierSteps = new Dictionary<int, HashSet<Step>>();
			_allConnections = new HashSet<Connection>();
			CalculateConnectionGroups();
			CalculateStepTiers();
		}

		public IEnumerable<Connection> GetAbnormalConnections()
		{
			return _abnormalConnections;
		}

		public IEnumerable<Connection> GetAllConnections()
		{
			return _allConnections;
		}

		public int Tiers
		{
			get
			{
				return _tierSteps.Count;
			}
		}

		public IEnumerable<Step> GetStepsInTier(int tier)
		{
			if (!_tierSteps.ContainsKey(tier))
			{
				return new List<Step>();
			}
			return _tierSteps[tier];
		}

		private void CalculateStepTiers()
		{
			Dictionary<Step, int> stepTiers = new Dictionary<Step, int>();
			HashSet<Step> remainingSteps = new HashSet<Step>(_steps);
			// tier 0
			HashSet<Step> tier0 = new HashSet<Step>();
			foreach (Step step in remainingSteps)
			{
				if (!step.hasNormalProductConnections.Get())
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
					foreach (Connection connection in step.normalIngredientConnections.Get())
					{
						ingredientSteps.UnionWith(connection.ProducerSteps);
					}
				}
				if (currentTier == 1)
				{
					foreach (Connection connection in _abnormalConnections)
					{
						ingredientSteps.UnionWith(connection.ProducerSteps);
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
				if (!_tierSteps.ContainsKey(stepTier))
				{
					_tierSteps.Add(stepTier, new HashSet<Step>());
				}
				_tierSteps[stepTier].Add(step);
			}
			while (!_tierSteps.ContainsKey(0) && _tierSteps.Count > 0)
			{
				for (int i = 0; i < currentTier; i++)
				{
					if (_tierSteps.ContainsKey(i + 1))
					{
						_tierSteps.Add(i, _tierSteps[i + 1]);
						_tierSteps.Remove(i + 1);
					}
				}
			}
		}

		private void CalculateConnectionGroups()
		{
			foreach (Step step in _steps)
			{
				foreach (Connection connection in step.Connections)
				{
					_allConnections.Add(connection);
					if (_abnormalConnections.Contains(connection))
					{
						continue;
					}
					foreach (HashSet<Connection> connectionGroup in _normalConnectionGroups)
					{
						if (connectionGroup.Contains(connection))
						{
							goto Continue;
						}
					}
					if (connection.Type == model.production.ConnectionType.SINGLE)
					{
						foreach (HashSet<Connection> connections in _normalConnectionGroups)
						{
							if (connections.First().IsConnectedNormallyTo(connection))
							{
								if (connections.Add(connection))
								{
									goto Continue;
								}
							}
						}
						_normalConnectionGroups.Add(new HashSet<Connection>() { connection });
					}
					else
					{
						_abnormalConnections.Add(connection);
					}
				Continue:
					continue;
				}
			}
		}
	}
}
