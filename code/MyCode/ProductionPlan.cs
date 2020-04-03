using System;
using System.Collections.Generic;
using VisualSatisfactoryCalculator.forms;

namespace VisualSatisfactoryCalculator.code
{
	public class ProductionPlan
	{
		private readonly List<ProductionStep> steps;
		private readonly MainForm mainForm;

		public ProductionPlan(MainForm mainForm)
		{
			this.mainForm = mainForm;
			steps = new List<ProductionStep>();
		}

		public void AddStep(ProductionStep step)
		{
			steps.Add(step);
			mainForm.PlanUpdated();
		}

		public List<ProductionStep> GetAllSteps()
		{
			return steps;
		}

		public Dictionary<JSONItem, decimal> NetRates()
		{
			Dictionary<JSONItem, decimal> netRates = new Dictionary<JSONItem, decimal>();
			foreach (ProductionStep step in steps)
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
			foreach (ProductionStep step in steps)
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
			Dictionary<JSONItem, decimal> netRates = NetRates();
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
			Dictionary<JSONItem, decimal> netRates = NetRates();
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
