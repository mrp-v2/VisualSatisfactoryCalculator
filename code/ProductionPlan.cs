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
	}
}
