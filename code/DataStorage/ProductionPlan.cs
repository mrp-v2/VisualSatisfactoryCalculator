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
			Dictionary<string, decimal> netRates = new Dictionary<string, decimal>
			{
				{ Constants.MWItem.GetUID(), -GetRecursivePowerDraw(encodings) }
			};
			foreach (ProductionStep step in GetAllSteps())
			{
				foreach (ItemCount itemCount in step.GetRecipe().GetItemCounts())
				{
					string itemUID = itemCount.GetItemUID();
					if (netRates.ContainsKey(itemUID))
					{
						netRates[itemUID] += step.GetItemRate(itemUID);
					}
					else
					{
						netRates.Add(itemUID, step.GetItemRate(itemUID));
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

		public string GetNetProductsString(List<IEncoder> encodings)
		{
			string str = "Products: ";
			Dictionary<string, decimal> netRates = GetNetRates(encodings);
			foreach (string itemUID in netRates.Keys)
			{
				if (netRates[itemUID] > 0 && Math.Round(netRates[itemUID], 5) != 0)
				{
					if ((encodings.FindByID(itemUID) as IItem).IsLiquid()) str += Math.Round(netRates[itemUID] / 1000, 5) + " " + encodings.GetDisplayNameFor(itemUID) + ", ";
					else str += Math.Round(netRates[itemUID], 5) + " " + encodings.GetDisplayNameFor(itemUID) + ", ";
				}
			}
			return str;
		}

		public string GetNetIngredientsString(List<IEncoder> encodings)
		{
			string str = "Ingredients: ";
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
