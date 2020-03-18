using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.code
{
	class ProductionPlan
	{
		List<ProductionStep> steps;
		List<ProductionStepPair> pairs;

		public ProductionPlan()
		{
			steps = new List<ProductionStep>();
			pairs = new List<ProductionStepPair>();
		}

		public void AddStep(ProductionStep step)
		{
			steps.Add(step);
		}

		public void AddStepPair(ProductionStepPair pair)
		{
			pairs.Add(pair);
		}
	}
}
