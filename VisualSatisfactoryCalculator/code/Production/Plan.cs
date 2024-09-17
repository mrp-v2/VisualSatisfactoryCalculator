using System.Collections.Generic;

using VisualSatisfactoryCalculator.satisfactory.Numbers;
using VisualSatisfactoryCalculator.satisfactory.Utility;
using VisualSatisfactoryCalculator.model.production;

using ItemCount = VisualSatisfactoryCalculator.model.production.ItemCount<VisualSatisfactoryCalculator.satisfactory.model.production.Item>;
using VisualSatisfactoryCalculator.satisfactory.model.production;

namespace VisualSatisfactoryCalculator.satisfactory.Production
{
	public class Plan
	{
		public readonly HashSet<Step> steps;
		public readonly CachedValue<ProcessedPlan> processedPlan;

		public Plan()
		{
			steps = new HashSet<Step>();
			processedPlan = new CachedValue<ProcessedPlan>(() => new ProcessedPlan(this));
		}

		public RateCollection GetNetRates()
		{
			return GetProductRates().Subtract(GetIngredientRates());
		}

		public RateCollection GetProductRates()
		{
			RateCollection rates = new RateCollection(0);
			foreach (Step step in steps)
			{
				foreach (ItemCount<Item> itemCount in step.productionRates.Get())
				{
					rates.Add(itemCount.item, step.GetRate(itemCount.item, true));
				}
			}
			return rates;
		}

		public double GetPowerDraw()
		{
			double powerDraw = 0;
			foreach (Step step in steps)
			{
				powerDraw += step.GetPowerDraw();
			}
			return powerDraw;
		}

		public RateCollection GetIngredientRates()
		{
			RateCollection rates = new RateCollection(GetPowerDraw());
			foreach (Step step in steps)
			{
				foreach (ItemCount itemCount in step.consumptionRates.Get())
				{
					rates.Add(itemCount.item, step.GetRate(itemCount.item, false));
				}
			}
			return rates;
		}

		public Dictionary<Building, uint> MachineCount()
		{
			Dictionary<Building, uint> totalMachines = new Dictionary<Building, uint>();
			foreach (Step step in steps)
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
				RationalNumber rate = netRates[item];
				string rateStr = rate.ToString();
				if (rate.isPositive && rate.isNonZero)
				{
					if (first)
					{
						first = false;
					}
					else
					{
						str += ", ";
					}
					str += rateStr + " " + item.displayName;
				}
			}
			str += "\nAll Products: ";
			RateCollection rates = GetProductRates();
			first = true;
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
				str += rates[item].ToString() + " " + item.displayName;
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
				str += rates[item].ToString() + " " + item.displayName;
			}
			str += "\nNet Ingredients: ";
			RateCollection netRates = GetNetRates();
			first = true;
			foreach (Item item in netRates.Items)
			{
				RationalNumber rate = netRates[item];
				string rateStr = (-rate).ToString();
				if (!rate.isPositive && rate.isNonZero)
				{
					if (first)
					{
						first = false;
					}
					else
					{
						str += ", ";
					}
					str += rateStr + " " + item.displayName;
				}
			}
			return str;
		}
	}
}
