using System.Collections.Generic;

using VisualSatisfactoryCalculator.code.Interfaces;
using VisualSatisfactoryCalculator.code.Numbers;
using VisualSatisfactoryCalculator.code.Utility;
using VisualSatisfactoryCalculator.model.production;

namespace VisualSatisfactoryCalculator.code.Production
{
	public class Plan
	{
		public readonly HashSet<Step> Steps;
		public readonly CachedValue<ProcessedPlan> ProcessedPlan;

		public Plan()
		{
			Steps = new HashSet<Step>();
			ProcessedPlan = new CachedValue<ProcessedPlan>(() => new ProcessedPlan(this));
		}

		public RateCollection GetNetRates(Encodings encodings)
		{
			return GetProductRates().Subtract(GetIngredientRates(encodings));
		}

		public RateCollection GetProductRates()
		{
			RateCollection rates = new RateCollection(0);
			foreach (Step step in Steps)
			{
				foreach (ItemRate itemCount in step.ProductionRates.Get())
				{
					rates.Add(itemCount.ItemUID, step.GetItemRate(itemCount.ItemUID, true));
				}
			}
			return rates;
		}

		public double GetPowerDraw(Encodings encodings)
		{
			double powerDraw = 0;
			foreach (Step step in Steps)
			{
				powerDraw += step.GetPowerDraw(encodings);
			}
			return powerDraw;
		}

		public RateCollection GetIngredientRates(Encodings encodings)
		{
			RateCollection rates = new RateCollection(GetPowerDraw(encodings));
			foreach (Step step in Steps)
			{
				foreach (ItemRate itemCount in step.ConsumptionRates.Get())
				{
					rates.Add(itemCount.ItemUID, step.GetItemRate(itemCount.ItemUID, false));
				}
			}
			return rates;
		}

		public Dictionary<string, int> MachineCount()
		{
			Dictionary<string, int> totalMachines = new Dictionary<string, int>();
			foreach (Step step in Steps)
			{
				if (!totalMachines.ContainsKey(step.Recipe.MachineUID))
				{
					totalMachines.Add(step.Recipe.MachineUID, step.CalculateMachineCount());
				}
				else
				{
					totalMachines[step.Recipe.MachineUID] += step.CalculateMachineCount();
				}
			}
			return totalMachines;
		}

		public string GetMachinesString(Encodings encodings)
		{
			string total = "";
			Dictionary<string, int> machines = MachineCount();
			foreach (string machineUID in machines.Keys)
			{
				total += machines[machineUID] + " " + encodings[machineUID].DisplayName + "\n";
			}
			return total;
		}

		public string GetProductsString(Encodings encodings)
		{
			string str = "Net Products: ";
			RateCollection netRates = GetNetRates(encodings);
			bool first = true;
			foreach (string itemUID in netRates.ItemUIDs)
			{
				RationalNumber rate = netRates[itemUID];
				IItem item = encodings[itemUID] as IItem;
				string rateStr = rate.ToString();
				if (rate.IsPositive && rate.IsNonZero)
				{
					if (first)
					{
						first = false;
					}
					else
					{
						str += ", ";
					}
					str += rateStr + " " + item.DisplayName;
				}
			}
			str += "\nAll Products: ";
			RateCollection rates = GetProductRates();
			first = true;
			foreach (string itemUID in rates.ItemUIDs)
			{
				IItem item = encodings[itemUID] as IItem;
				if (first)
				{
					first = false;
				}
				else
				{
					str += ", ";
				}
				str += rates[itemUID].ToString() + " " + item.DisplayName;
			}
			return str;
		}

		public string GetIngredientsString(Encodings encodings)
		{
			string str = "All Ingredients: ";
			RateCollection rates = GetIngredientRates(encodings);
			bool first = true;
			foreach (string itemUID in rates.ItemUIDs)
			{
				IItem item = encodings[itemUID] as IItem;
				if (first)
				{
					first = false;
				}
				else
				{
					str += ", ";
				}
				str += rates[itemUID].ToString() + " " + item.DisplayName;
			}
			str += "\nNet Ingredients: ";
			RateCollection netRates = GetNetRates(encodings);
			first = true;
			foreach (string itemUID in netRates.ItemUIDs)
			{
				RationalNumber rate = netRates[itemUID];
				IItem item = encodings[itemUID] as IItem;
				string rateStr = (-rate).ToString();
				if (!rate.IsPositive && rate.IsNonZero)
				{
					if (first)
					{
						first = false;
					}
					else
					{
						str += ", ";
					}
					str += rateStr + " " + item.DisplayName;
				}
			}
			return str;
		}
	}
}
