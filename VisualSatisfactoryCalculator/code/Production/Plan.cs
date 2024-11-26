using System.Collections.Generic;

using VisualSatisfactoryCalculator.model.production;
using VisualSatisfactoryCalculator.model.util;
using VisualSatisfactoryCalculator.satisfactory.model.production;
using VisualSatisfactoryCalculator.satisfactory.Utility;
using VisualSatisfactoryCalculator.util.collections;

using ProcessedPlan = VisualSatisfactoryCalculator.model.production.ProcessedPlan<VisualSatisfactoryCalculator.satisfactory.Production.Step, VisualSatisfactoryCalculator.satisfactory.model.production.Item, VisualSatisfactoryCalculator.satisfactory.model.production.Recipe>;

namespace VisualSatisfactoryCalculator.satisfactory.Production
{
	public class Plan
	{
		private readonly ViewableSet<Step> _steps;
		public readonly IReadOnlySet<Step> steps;
		private readonly CachedValue<ProcessedPlan>.Managed _processedPlan;
		public static readonly Mutable<int> VERSION = new Mutable<int>();

		public ProcessedPlan ProcessedPlan
		{
			get
			{
				return _processedPlan.Get();
			}
		}

		public Plan()
		{
			_steps = new ViewableSet<Step>();
			steps = _steps.ReadOnly;
			_processedPlan = new CachedValue<ProcessedPlan>.Managed(() => new ProcessedPlan(_steps.ReadOnly));
			_processedPlan.AddInvalidationCallback((sender, args) => VERSION.value++);
		}

		public void AddStep(Step step)
		{
			AddStep(step, false);
		}

		public void AddStep(Step step, bool updateRequired)
		{
			_steps.Add(step);
			InvalidateProcessedPlan();
			if (updateRequired)
			{
				BreadthFirstSearchHandler<Item, Step, Recipe>.CascadeUpdates(step, ProcessedPlan);
			}
		}

		public void RemoveStep(Step step)
		{
			_steps.Remove(step);
			_processedPlan.Invalidate();
		}

		public void InvalidateProcessedPlan()
		{
			_processedPlan.Invalidate();
		}

		public void ClearSteps()
		{
			_steps.Clear();
			InvalidateProcessedPlan();
		}

		public RateCollection GetNetRates()
		{
			return GetProductRates().Subtract(GetIngredientRates());
		}

		public RateCollection GetProductRates()
		{
			RateCollection rates = new RateCollection(0);
			foreach (Step step in _steps)
			{
				foreach (KeyValuePair<Item, decimal> pair in step.productionRates.Get())
				{
					rates.Add(pair.Key, pair.Value);
				}
			}
			return rates;
		}

		public double GetPowerDraw()
		{
			double powerDraw = 0;
			foreach (Step step in _steps)
			{
				powerDraw += step.GetPowerDraw();
			}
			return powerDraw;
		}

		public RateCollection GetIngredientRates()
		{
			RateCollection rates = new RateCollection(GetPowerDraw());
			foreach (Step step in _steps)
			{
				foreach (KeyValuePair<Item, decimal> pair in step.consumptionRates.Get())
				{
					rates.Add(pair.Key, pair.Value);
				}
			}
			return rates;
		}

		public Dictionary<Building, uint> MachineCount()
		{
			Dictionary<Building, uint> totalMachines = new Dictionary<Building, uint>();
			foreach (Step step in _steps)
			{
				if (!totalMachines.ContainsKey(step.recipe.building))
				{
					totalMachines.Add(step.recipe.building, step.MachineCount);
				}
				else
				{
					totalMachines[step.recipe.building] += step.MachineCount;
				}
			}
			return totalMachines;
		}

		public string GetMachinesString()
		{
			string total = "";
			Dictionary<Building, uint> machines = MachineCount();
			foreach (Building building in machines.Keys)
			{
				total += machines[building] + " " + building.displayName + "\n";
			}
			return total;
		}

		public string GetProductsString()
		{
			string str = "Net Products: ";
			RateCollection netRates = GetNetRates();
			bool first = true;
			foreach (Item item in netRates.Items)
			{
				decimal rate = netRates[item];
				if (rate > 0)
				{
					if (first)
					{
						first = false;
					}
					else
					{
						str += ", ";
					}
					str += item.ToString(rate);
				}
			}
			str += "\nAll Products: ";
			RateCollection rates = GetProductRates();
			first = true;
			foreach (Item item in rates.Items)
			{
				decimal rate = rates[item];
				if (first)
				{
					first = false;
				}
				else
				{
					str += ", ";
				}
				str += item.ToString(rate);
			}
			return str;
		}

		public string GetIngredientsString()
		{
			string str = "All Ingredients: ";
			RateCollection rates = GetIngredientRates();
			bool first = true;
			foreach (Item item in rates.Items)
			{
				if (first)
				{
					first = false;
				}
				else
				{
					str += ", ";
				}
				str += item.ToString(rates[item]);
			}
			str += "\nNet Ingredients: ";
			RateCollection netRates = GetNetRates();
			first = true;
			foreach (Item item in netRates.Items)
			{
				decimal rate = netRates[item];
				if (rate < 0)
				{
					if (first)
					{
						first = false;
					}
					else
					{
						str += ", ";
					}
					str += item.ToString(-rate);
				}
			}
			return str;
		}
	}
}
