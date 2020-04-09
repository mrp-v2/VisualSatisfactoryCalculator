using System;
using System.Collections.Generic;
using VisualSatisfactoryCalculator.code.Interfaces;

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

		public Dictionary<IItem, decimal> GetNetRates()
		{
			Dictionary<IItem, decimal> netRates = new Dictionary<IItem, decimal>();
			foreach (ProductionStep step in GetAllSteps())
			{
				foreach (ItemCount itemCount in step.GetRecipe().GetItemCounts())
				{
					IItem item = itemCount.GetItem();
					if (netRates.ContainsKey(item))
					{
						netRates[item] += step.GetItemRate(item);
					}
					else
					{
						netRates.Add(item, step.GetItemRate(item));
					}
				}
			}
			return netRates;
		}

		public Dictionary<string, int> TotalMachineCount()
		{
			Dictionary<string, int> totalMachines = new Dictionary<string, int>();
			foreach (ProductionStep step in GetAllSteps())
			{
				if (!totalMachines.ContainsKey(step.GetRecipe().GetMachine()))
				{
					totalMachines.Add(step.GetRecipe().GetMachine(), step.CalculateMachineCount());
				}
				else
				{
					totalMachines[step.GetRecipe().GetMachine()] += step.CalculateMachineCount();
				}
			}
			return totalMachines;
		}

		public string GetTotalMachineString()
		{
			string total = "";
			Dictionary<string, int> machines = TotalMachineCount();
			foreach (string str in machines.Keys)
			{
				total += machines[str] + " " + str + "\n";
			}
			return total;
		}

		public string GetNetProductsString()
		{
			string str = "Products: ";
			Dictionary<IItem, decimal> netRates = GetNetRates();
			foreach (IItem item in netRates.Keys)
			{
				if (netRates[item] > 0 && Math.Round(netRates[item], 5) != 0)
				{
					str += Math.Round(netRates[item], 5) + " " + item.ToString() + ", ";
				}
			}
			return str;
		}

		public string GetNetIngredientsString()
		{
			string str = "Ingredients: ";
			Dictionary<IItem, decimal> netRates = GetNetRates();
			foreach (IItem i in netRates.Keys)
			{
				if (netRates[i] < 0 && Math.Round(netRates[i], 5) != 0)
				{
					str += Math.Round(-netRates[i], 5) + " " + i.ToString() + ", ";
				}
			}
			return str;
		}
	}
}
