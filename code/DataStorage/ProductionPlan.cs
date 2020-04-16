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

		public Dictionary<string, decimal> GetNetRates(List<IEncoder> encodings)
		{
			return GetAllProductRates().Merge(GetAllIngredientRates(encodings));
		}

		public Dictionary<string, decimal> GetAllProductRates()
		{
			Dictionary<string, decimal> rates = new Dictionary<string, decimal>();
			foreach (ProductionStep step in GetAllSteps())
			{
				foreach (ItemCount itemCount in step.GetRecipe().GetItemCounts().GetProducts())
				{
					if (rates.ContainsKey(itemCount.GetItemUID()))
					{
						rates[itemCount.GetItemUID()] += step.GetItemRate(itemCount.GetItemUID());
					}
					else
					{
						rates.Add(itemCount.GetItemUID(), step.GetItemRate(itemCount.GetItemUID()));
					}
				}
			}
			return rates;
		}

		public Dictionary<string, decimal> GetAllIngredientRates(List<IEncoder> encodings)
		{
			Dictionary<string, decimal> rates = new Dictionary<string, decimal>()
			{
				{ Constants.MWItem.GetUID(), -GetRecursivePowerDraw(encodings) }
			};
			foreach (ProductionStep step in GetAllSteps())
			{
				foreach (ItemCount itemCount in step.GetRecipe().GetItemCounts().GetIngredients())
				{
					if (rates.ContainsKey(itemCount.GetItemUID()))
					{
						rates[itemCount.GetItemUID()] += step.GetItemRate(itemCount.GetItemUID());
					}
					else
					{
						rates.Add(itemCount.GetItemUID(), step.GetItemRate(itemCount.GetItemUID()));
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
				if (!totalMachines.ContainsKey(step.GetRecipe().GetMachineUID()))
				{
					totalMachines.Add(step.GetRecipe().GetMachineUID(), step.CalculateMachineCount());
				}
				else
				{
					totalMachines[step.GetRecipe().GetMachineUID()] += step.CalculateMachineCount();
				}
			}
			return totalMachines;
		}

		public string GetTotalMachineString(List<IEncoder> encodings)
		{
			string total = "";
			Dictionary<string, int> machines = TotalMachineCount();
			foreach (string machineUID in machines.Keys)
			{
				total += machines[machineUID] + " " + encodings.GetDisplayNameFor(machineUID) + "\n";
			}
			return total;
		}

		public string GetProductsString(List<IEncoder> encodings)
		{
			string str = "Net Products: ";
			Dictionary<string, decimal> netRates = GetNetRates(encodings);
			foreach (string itemUID in netRates.Keys)
			{
				if (netRates[itemUID] > 0 && Math.Round(netRates[itemUID], 5) != 0)
				{
					if ((encodings.FindByID(itemUID) as IItem).IsLiquid()) str += Math.Round(netRates[itemUID] / 1000, 5) + " " + encodings.GetDisplayNameFor(itemUID) + ", ";
					else str += Math.Round(netRates[itemUID], 5) + " " + encodings.GetDisplayNameFor(itemUID) + ", ";
				}
			}
			str += "\nAll Products: ";
			Dictionary<string, decimal> rates = GetAllProductRates();
			foreach (string itemUID in rates.Keys)
			{
				if ((encodings.FindByID(itemUID) as IItem).IsLiquid()) str += Math.Round(rates[itemUID] / 1000, 5) + " " + encodings.GetDisplayNameFor(itemUID) + ", ";
				else str += Math.Round(rates[itemUID], 5) + " " + encodings.GetDisplayNameFor(itemUID) + ", ";
			}
			return str;
		}

		public string GetIngredientsString(List<IEncoder> encodings)
		{
			string str = "All Ingredients: ";
			Dictionary<string, decimal> rates = GetAllIngredientRates(encodings);
			foreach (string itemUID in rates.Keys)
			{
				if ((encodings.FindByID(itemUID) as IItem).IsLiquid()) str += Math.Round(-rates[itemUID] / 1000, 5) + " " + encodings.GetDisplayNameFor(itemUID) + ", ";
				else str += Math.Round(-rates[itemUID], 5) + " " + encodings.GetDisplayNameFor(itemUID) + ", ";
			}
			str += "\nNet Ingredients: ";
			Dictionary<string, decimal> netRates = GetNetRates(encodings);
			foreach (string itemUID in netRates.Keys)
			{
				if (netRates[itemUID] < 0 && Math.Round(netRates[itemUID], 5) != 0)
				{
					if ((encodings.FindByID(itemUID) as IItem).IsLiquid()) str += Math.Round(-netRates[itemUID] / 1000, 5) + " " + encodings.GetDisplayNameFor(itemUID) + ", ";
					else str += Math.Round(-netRates[itemUID], 5) + " " + encodings.GetDisplayNameFor(itemUID) + ", ";
				}
			}
			return str;
		}
	}
}
