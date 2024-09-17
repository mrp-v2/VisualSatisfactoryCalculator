using System;
using System.Collections.Generic;
using System.Linq;

using VisualSatisfactoryCalculator.model.production;
using VisualSatisfactoryCalculator.satisfactory.Extensions;

using Connection = VisualSatisfactoryCalculator.model.production.Connection<VisualSatisfactoryCalculator.satisfactory.model.production.Item, VisualSatisfactoryCalculator.satisfactory.Production.Step, VisualSatisfactoryCalculator.satisfactory.model.production.Recipe>;

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
			HashSet<Connection> visitedConnections = new HashSet<Connection>();
			HashSet<Connection> currentConnections = new HashSet<Connection>(_steps.First().Connections);
			HashSet<Connection> nextConnections = new HashSet<Connection>();
			while (currentConnections.Count > 0)
			{
				foreach (Connection connection in currentConnections)
				{
					visitedConnections.Add(connection);
					switch (connection.Type)
					{
						case ConnectionType.SINGLE:
							HashSet<HashSet<Connection>> connectedGroups = new HashSet<HashSet<Connection>>();
							foreach (HashSet<Connection> connectionGroup in _normalConnectionGroups)
							{
								foreach (Connection potential in connectionGroup)
								{
									if (potential.SharesStep(connection))
									{
										connectedGroups.Add(connectionGroup);
										break;
									}
								}
							}
							switch (connectedGroups.Count)
							{
								case 0:
									_normalConnectionGroups.Add(new HashSet<Connection> { connection });
									break;
								case 1:
									connectedGroups.First().Add(connection);
									break;
								default:
									HashSet<Connection> combined = new HashSet<Connection>() { connection };
									foreach (HashSet<Connection> group in connectedGroups)
									{
										combined.AddRange(group);
										_normalConnectionGroups.Remove(group);
									}
									_normalConnectionGroups.Add(combined);
									break;
							}
							break;
						case ConnectionType.MULTI:
							_abnormalConnections.Add(connection);
							break;
					}
					foreach (Step step in connection.Steps)
					{
						foreach (Connection other in step.Connections)
						{
							if (!visitedConnections.Contains(other))
							{
								nextConnections.Add(other);
							}
						}
					}
				}
				currentConnections = nextConnections;
				nextConnections = new HashSet<Connection>();
			}
		}
	}
}
