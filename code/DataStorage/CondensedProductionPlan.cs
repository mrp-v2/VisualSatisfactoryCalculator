using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSatisfactoryCalculator.code.Extensions;
using VisualSatisfactoryCalculator.code.Interfaces;

namespace VisualSatisfactoryCalculator.code.DataStorage
{
	[Serializable]
	class CondensedProductionPlan : CondensedProductionStep
	{
		private readonly decimal multiplier;

		public ProductionPlan ToProductionPlan(List<IRecipe> recipes)
		{
			IRecipe myRecipe = GetRecipe(recipes);
			ProductionPlan plan = new ProductionPlan(myRecipe, multiplier);
			if (children != null && children.Count > 0)
			{
				foreach (CondensedProductionStep child in children)
				{
					IItem shared = myRecipe.GetItemCounts().ToItems().FindMatch(child.GetRecipe(recipes).GetItemCounts().ToItems());
					decimal rate = myRecipe.GetRateOf(shared) * -1;
					plan.AddRelatedStep(child.ToProductionStep(recipes, rate, shared));
				}
			}
			return plan;
		}

		public CondensedProductionPlan(ProductionPlan plan) : base(plan, null)
		{
			multiplier = plan.GetMultiplier();
		}
	}
}
