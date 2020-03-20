using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

		public Dictionary<Item, decimal> NetRates()
		{
			Dictionary<Item, decimal> netRates = new Dictionary<Item, decimal>();
			foreach (ProductionStep step in steps)
			{
				foreach (ItemCount ic in step.GetItemCounts())
				{
					if (netRates.ContainsKey(ic.ToItem()))
					{
						netRates[ic.ToItem()] += step.GetItemRate(ic.ToItem());
					}
					else
					{
						netRates.Add(ic.ToItem(), step.GetItemRate(ic.ToItem()));
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
				if (totalMachines.ContainsKey(step.GetMachine()))
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
	}
}
