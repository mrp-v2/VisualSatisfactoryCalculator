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

		public ProductionStep ToProductionStep(List<IRecipe> recipes)
		{
			IRecipe myRecipe = GetRecipe(recipes);
			ProductionStep step = new ProductionStep(myRecipe, 1);
			if (children != null && children.Count > 0)
			{
				foreach (CondensedProductionStep child in children)
				{
					IItem shared = myRecipe.GetItemCounts().ToItems().FindMatch(child.GetRecipe(recipes).GetItemCounts().ToItems());
					ProductionStep childStep = child.ToProductionStep(recipes);
					childStep.AddRelatedStep(step);
					step.AddRelatedStep(childStep);
				}
			}
			return step;
		}

		public CondensedProductionStep(ProductionStep step, ProductionStep parent)
		{
			recipeUID = step.GetRecipe().GetUID();
			if (step.GetRelatedSteps().Count > 1)
			{
				children = new List<CondensedProductionStep>();
				foreach (ProductionStep relatedStep in step.GetRelatedSteps())
				{
					if (!relatedStep.Equals(parent))
					{
						children.Add(new CondensedProductionStep(relatedStep, step));
					}
				}
			}
		}
	}
}
