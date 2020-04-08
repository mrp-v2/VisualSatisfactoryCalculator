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
	class CondensedProductionStep
	{
		protected readonly List<CondensedProductionStep> children;
		private readonly string recipeUID;

		public IRecipe GetRecipe(List<IRecipe> recipes)
		{
			return recipes.MatchID(recipeUID);
		}

		protected decimal CalculateMultiplier(decimal sharedItemRate, IItem sharedItem, IRecipe myRecipe)
		{
			return sharedItemRate / myRecipe.GetRateOf(sharedItem);
		}

		public ProductionStep ToProductionStep(List<IRecipe> recipes, decimal sharedItemRate, IItem sharedItem)
		{
			IRecipe myRecipe = GetRecipe(recipes);
			ProductionStep step = new ProductionStep(myRecipe, CalculateMultiplier(sharedItemRate, sharedItem, myRecipe));
			if (children != null && children.Count > 0)
			{
				foreach (CondensedProductionStep child in children)
				{
					IItem shared = myRecipe.GetItemCounts().ToItems().FindMatch(child.GetRecipe(recipes).GetItemCounts().ToItems());
					decimal rate = myRecipe.GetRateOf(shared) * -1;
					step.AddRelatedStep(child.ToProductionStep(recipes, rate, shared));
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
