using System;
using System.Collections.Generic;
using VisualSatisfactoryCalculator.code.Extensions;
using VisualSatisfactoryCalculator.code.Interfaces;

namespace VisualSatisfactoryCalculator.code.DataStorage
{
	[Serializable]
	class CondensedProductionStep
	{
		protected readonly List<CondensedProductionStep> children;
		private readonly string recipeUID;

		public IRecipe GetRecipe(List<IRecipe> recipes)
		{
			return recipes.MatchID(recipeUID);
		}

		public ProductionStep ToProductionStep(List<IRecipe> recipes, ProductionStep parent)
		{
			IRecipe myRecipe = GetRecipe(recipes);
			ProductionStep step = new ProductionStep(myRecipe, parent);
			if (children != null && children.Count > 0)
			{
				foreach (CondensedProductionStep child in children)
				{
					ProductionStep childStep = child.ToProductionStep(recipes, step);
					step.AddChildStep(childStep);
				}
			}
			return step;
		}

		public CondensedProductionStep(ProductionStep step)
		{
			recipeUID = step.GetRecipe().GetUID();
			if (step.GetChildSteps().Count > 0)
			{
				children = new List<CondensedProductionStep>();
				foreach (ProductionStep childStep in step.GetChildSteps())
				{
					children.Add(new CondensedProductionStep(childStep));
				}
			}
		}
	}
}
