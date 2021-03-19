using System;
using System.Collections.Generic;

using VisualSatisfactoryCalculator.code.Extensions;
using VisualSatisfactoryCalculator.code.Interfaces;
using VisualSatisfactoryCalculator.code.Utility;

namespace VisualSatisfactoryCalculator.code.DataStorage
{
	public class ProductionPlan : ProductionStep
	{
		public ProductionPlan(IRecipe recipe) : base(recipe, 1) { }
		public ProductionPlan(IRecipe recipe, decimal multiplier) : base(recipe, multiplier) { }

		public List<ProductionStep> GetAllSteps()
		{
			List<ProductionStep> allSteps = GetAllStepsRecursively();
			allSteps.Add(this);
			return allSteps;
		}

		public Dictionary<string, decimal> GetNetRates(Dictionary<string, IEncoder> encodings)
		{
			return GetAllProductRates().Subtract(GetAllIngredientRates(encodings));
		}

		public Dictionary<string, decimal> GetAllProductRates()
		{
			Dictionary<string, decimal> rates = new Dictionary<string, decimal>();
			foreach (ProductionStep step in GetAllSteps())
			{
				foreach (ItemCount itemCount in step.GetRecipe().Products.Values)
				{
					if (rates.ContainsKey(itemCount.ItemUID))
					{
						rates[itemCount.ItemUID] += step.GetItemRate(itemCount.ItemUID, true);
					}
					else
					{
						rates.Add(itemCount.ItemUID, step.GetItemRate(itemCount.ItemUID, true));
					}
				}
			}
			return rates;
		}

		public Dictionary<string, decimal> GetAllIngredientRates(Dictionary<string, IEncoder> encodings)
		{
			Dictionary<string, decimal> rates = new Dictionary<string, decimal>()
			{
				{ Constants.MWItem.UID, GetRecursivePowerDraw(encodings) }
			};
			foreach (ProductionStep step in GetAllSteps())
			{
				foreach (ItemCount itemCount in step.GetRecipe().Ingredients.Values)
				{
					if (rates.ContainsKey(itemCount.ItemUID))
					{
						rates[itemCount.ItemUID] += step.GetItemRate(itemCount.ItemUID, false);
					}
					else
					{
						rates.Add(itemCount.ItemUID, step.GetItemRate(itemCount.ItemUID, false));
					}
				}
			}
			return rates;
		}

		public Dictionary<string, int> TotalMachineCount()
		{
			Dictionary<string, int> totalMachines = new Dictionary<string, int>();
			foreach (ProductionStep step in GetAllSteps())
			{
				if (!totalMachines.ContainsKey(step.GetRecipe().MachineUID))
				{
					totalMachines.Add(step.GetRecipe().MachineUID, step.CalculateMachineCount());
				}
				else
				{
					totalMachines[step.GetRecipe().MachineUID] += step.CalculateMachineCount();
				}
			}
			return totalMachines;
		}

		public string GetTotalMachineString(Dictionary<string, IEncoder> encodings)
		{
			string total = "";
			Dictionary<string, int> machines = TotalMachineCount();
			foreach (string machineUID in machines.Keys)
			{
				total += machines[machineUID] + " " + encodings[machineUID].DisplayName + "\n";
			}
			return total;
		}

		public string GetProductsString(Dictionary<string, IEncoder> encodings)
		{
			string str = "Net Products: ";
			Dictionary<string, decimal> netRates = GetNetRates(encodings);
			foreach (string itemUID in netRates.Keys)
			{
				decimal rate = netRates[itemUID];
				IItem item = encodings[itemUID] as IItem;
				string rateStr = item.ToString(rate);
				if (rate > 0 && decimal.Parse(rateStr) != 0)
				{
					str += rateStr + " " + item.DisplayName + ", ";
				}
			}
			str += "\nAll Products: ";
			Dictionary<string, decimal> rates = GetAllProductRates();
			foreach (string itemUID in rates.Keys)
			{
				IItem item = encodings[itemUID] as IItem;
				str += item.ToString(rates[itemUID]) + " " + item.DisplayName + ", ";
			}
			return str;
		}

		public string GetIngredientsString(Dictionary<string, IEncoder> encodings)
		{
			string str = "All Ingredients: ";
			Dictionary<string, decimal> rates = GetAllIngredientRates(encodings);
			foreach (string itemUID in rates.Keys)
			{
				IItem item = encodings[itemUID] as IItem;
				str += item.ToString(rates[itemUID]) + " " + item.DisplayName + ", ";
			}
			str += "\nNet Ingredients: ";
			Dictionary<string, decimal> netRates = GetNetRates(encodings);
			foreach (string itemUID in netRates.Keys)
			{
				decimal rate = netRates[itemUID];
				IItem item = encodings[itemUID] as IItem;
				string rateStr = item.ToString(-rate);
				if (rate < 0 && decimal.Parse(rateStr) != 0)
				{
					str += rateStr + " " + item.DisplayName + ", ";
				}
			}
			return str;
		}
	}
}
