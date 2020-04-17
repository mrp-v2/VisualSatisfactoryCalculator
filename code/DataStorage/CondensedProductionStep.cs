using System;
using System.Collections.Generic;
using VisualSatisfactoryCalculator.code.Extensions;
using VisualSatisfactoryCalculator.code.Interfaces;

namespace VisualSatisfactoryCalculator.code.DataStorage
{
	[Serializable]
	class CondensedProductionStep
	{
		protected readonly Dictionary<CondensedProductionStep, string> children;
		private readonly string recipeUID;

		public IRecipe GetRecipe(List<IRecipe> recipes)
		{
			return recipes.FindByID(recipeUID);
		}

		public ProductionStep ToProductionStep(List<IRecipe> recipes, ProductionStep parent, string itemUID)
		{
			IRecipe myRecipe = GetRecipe(recipes);
			ProductionStep step = new ProductionStep(myRecipe, parent, itemUID);
			if (children != null && children.Count > 0)
			{
				foreach (CondensedProductionStep child in children.Keys)
				{
					ProductionStep childStep = child.ToProductionStep(recipes, step, children[child]);
					step.AddChildStep(childStep, children[child]);
				}
			}
			return step;
		}

		public CondensedProductionStep(ProductionStep step)
		{
			recipeUID = step.GetRecipe().GetUID();
			if (step.GetChildSteps().Count > 0)
			{
				children = new Dictionary<CondensedProductionStep, string>();
				foreach (ProductionStep childStep in step.GetChildSteps().Keys)
				{
					children.Add(new CondensedProductionStep(childStep), step.GetChildSteps()[childStep]);
				}
			}
		}
	}
}
