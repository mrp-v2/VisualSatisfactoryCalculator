using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.code
{
	public class ProductionStep : Recipe
	{
		protected double multiplier;

		public ProductionStep(Recipe recipe, double multiplier) : base(recipe)
		{
			this.multiplier = multiplier;
		}

		public int CalculateMachineCount()
		{
			return (int)Math.Ceiling(multiplier);
		}
	}
}
