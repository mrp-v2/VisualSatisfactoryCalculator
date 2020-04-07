using System;
using System.Collections.Generic;
using System.Linq;
using VisualSatisfactoryCalculator.code.JSONClasses;

namespace VisualSatisfactoryCalculator.code.DataStorage
{
	public class ProductionPlan : ProductionStep
	{
		public ProductionPlan(JSONRecipe recipe) : base(recipe, 1) { }

		public List<ProductionStep> GetAllSteps()
		{
			List<ProductionStep> allSteps = GetAllStepsRecursively(null);
			allSteps.Add(this);
			return allSteps;
		}

		public Dictionary<sbyte, List<ProductionStep>> GetTierList()
		{
			Dictionary<sbyte, List<ProductionStep>> tiers = GetRelativeTiersRecursively(null, 0);
			if (!tiers.ContainsKey(0))
			{
				tiers.Add(0, new List<ProductionStep>());
			}
			tiers[0].Add(this);
			sbyte additive = 0;
			additive -= tiers.Keys.Min();
			if (additive != 0)
			{
				if (additive > 0)
				{
					for (sbyte i = tiers.Keys.Max(); i >= tiers.Keys.Min(); i--)
					{
						sbyte newKey = i;
						newKey += additive;
						tiers.Add(newKey, tiers[i]);
						tiers.Remove(i);
					}
				}
				else
				{
					for (sbyte i = tiers.Keys.Min(); i <= tiers.Keys.Max(); i++)
					{
						sbyte newKey = i;
						newKey += additive;
						tiers.Add(newKey, tiers[i]);
						tiers.Remove(i);
					}
				}
			}
			return tiers;
		}

		public Dictionary<JSONItem, decimal> GetNetRates()
		{
			Dictionary<JSONItem, decimal> netRates = new Dictionary<JSONItem, decimal>();
			foreach (ProductionStep step in GetAllSteps())
			{
				foreach (ItemCount ic in step.GetItemCounts())
				{
					JSONItem castNCopy = ic.CastAndCopy();
					if (netRates.ContainsKey(castNCopy))
					{
						netRates[castNCopy] += step.GetItemRate(castNCopy);
					}
					else
					{
						netRates.Add(castNCopy, step.GetItemRate(castNCopy));
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
				if (!totalMachines.ContainsKey(step.GetMachine()))
				{
					totalMachines.Add(step.GetMachine(), step.CalculateMachineCount());
				}
				else
				{
					totalMachines[step.GetMachine()] += step.CalculateMachineCount();
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
			Dictionary<JSONItem, decimal> netRates = GetNetRates();
			foreach (JSONItem i in netRates.Keys)
			{
				if (netRates[i] > 0 && Math.Round(netRates[i], 5) != 0)
				{
					str += Math.Round(netRates[i], 5) + " " + i.ToString() + ", ";
				}
			}
			return str;
		}

		public string GetNetIngredientsString()
		{
			string str = "Ingredients: ";
			Dictionary<JSONItem, decimal> netRates = GetNetRates();
			foreach (JSONItem i in netRates.Keys)
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
