using System.Collections.Generic;

using VisualSatisfactoryCalculator.satisfactory.Interfaces;
using VisualSatisfactoryCalculator.satisfactory.Numbers;
using VisualSatisfactoryCalculator.satisfactory.Utility;
using VisualSatisfactoryCalculator.model.production;
using VisualSatisfactoryCalculator.satisfactory.JSONClasses;

using ItemCount = VisualSatisfactoryCalculator.model.production.ItemCount<VisualSatisfactoryCalculator.satisfactory.JSONClasses.JSONItem>;

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

		public RateCollection GetNetRates(JsonEncodings encodings)
		{
			return GetProductRates().Subtract(GetIngredientRates(encodings));
		}

		public RateCollection GetProductRates()
		{
			RateCollection rates = new RateCollection(0);
			foreach (Step step in steps)
			{
				foreach (ItemCount<JSONItem> itemCount in step.productionRates.Get())
				{
					rates.Add(itemCount.item, step.GetRate(itemCount.item, true));
				}
			}
			return rates;
		}

		public double GetPowerDraw(JsonEncodings encodings)
		{
			double powerDraw = 0;
			foreach (Step step in steps)
			{
				powerDraw += step.GetPowerDraw(encodings);
			}
			return powerDraw;
		}

		public RateCollection GetIngredientRates(JsonEncodings encodings)
		{
			RateCollection rates = new RateCollection(GetPowerDraw(encodings));
			foreach (Step step in steps)
			{
				foreach (ItemCount itemCount in step.consumptionRates.Get())
				{
					rates.Add(itemCount.item, step.GetRate(itemCount.item, false));
				}
			}
			return rates;
		}

		public Dictionary<string, int> MachineCount()
		{
			Dictionary<string, int> totalMachines = new Dictionary<string, int>();
			foreach (Step step in steps)
			{
				if (!totalMachines.ContainsKey(step.recipe.MachineUID))
				{
					totalMachines.Add(step.recipe.MachineUID, step.CalculateMachineCount());
				}
				else
				{
					totalMachines[step.recipe.MachineUID] += step.CalculateMachineCount();
				}
			}
			return totalMachines;
		}

		public string GetMachinesString(JsonEncodings encodings)
		{
			string total = "";
			Dictionary<string, int> machines = MachineCount();
			foreach (string machineUID in machines.Keys)
			{
				total += machines[machineUID] + " " + encodings[machineUID].DisplayName + "\n";
			}
			return total;
		}

		public string GetProductsString(JsonEncodings encodings)
		{
			string str = "Net Products: ";
			RateCollection netRates = GetNetRates(encodings);
			bool first = true;
			foreach (JSONItem item in netRates.Items)
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
			foreach (JSONItem item in rates.Items)
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

		public string GetIngredientsString(JsonEncodings encodings)
		{
			string str = "All Ingredients: ";
			RateCollection rates = GetIngredientRates(encodings);
			bool first = true;
			foreach (JSONItem item in rates.Items)
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
			RateCollection netRates = GetNetRates(encodings);
			first = true;
			foreach (JSONItem item in netRates.Items)
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
