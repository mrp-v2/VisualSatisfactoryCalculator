using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.code
{
	public class ProductionPlan
	{
		private readonly List<ProductionStep> steps;

		public ProductionPlan()
		{
			steps = new List<ProductionStep>();
		}

		public void AddStep(ProductionStep step)
		{
			steps.Add(step);
		}
	}
}
