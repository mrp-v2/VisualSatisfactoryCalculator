using System;
using System.Collections.Generic;

using VisualSatisfactoryCalculator.code.Interfaces;

namespace VisualSatisfactoryCalculator.code.DataStorage
{
	[Serializable]
	internal class CondensedProductionPlan : CondensedProductionStep
	{
		private readonly decimal _multiplier;

		public ProductionPlan ToProductionPlan(Dictionary<string, IRecipe> recipes)
		{
			IRecipe myRecipe = GetRecipe(recipes);
			ProductionPlan plan = new ProductionPlan(myRecipe, _multiplier);
			if (_childIngredients != null && _childIngredients.Count > 0)
			{
				foreach (CondensedProductionStep child in _childIngredients.Keys)
				{
					_ = child.ToProductionStep(recipes, plan, _childIngredients[child]);
				}
			}
			if (_childProducts != null && _childProducts.Count > 0)
			{
				foreach (CondensedProductionStep child in _childProducts.Keys)
				{
					_ = child.ToProductionStep(recipes, plan, _childProducts[child]);
				}
			}
			return plan;
		}

		public CondensedProductionPlan(ProductionPlan plan) : base(plan)
		{
			_multiplier = plan.GetMultiplier();
		}
	}
}
