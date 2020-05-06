using System;
using System.Collections.Generic;
using VisualSatisfactoryCalculator.code.Extensions;
using VisualSatisfactoryCalculator.code.Interfaces;

namespace VisualSatisfactoryCalculator.code.DataStorage
{
	[Serializable]
	class CondensedProductionStep
	{
		protected readonly Dictionary<CondensedProductionStep, string> childProducts;
		protected readonly Dictionary<CondensedProductionStep, string> childIngredients;
		private readonly string recipeUID;
		private readonly bool isProduct;

		public IRecipe GetRecipe(Dictionary<string, IRecipe> recipes)
		{
			return recipes[recipeUID];
		}

		public ProductionStep ToProductionStep(Dictionary<string, IRecipe> recipes, ProductionStep parent, string itemUID)
		{
			IRecipe myRecipe = GetRecipe(recipes);
			ProductionStep step = new ProductionStep(myRecipe, parent, itemUID, isProduct);
			if (childProducts != null && childProducts.Count > 0)
			{
				foreach (CondensedProductionStep child in childProducts.Keys)
				{
					child.ToProductionStep(recipes, step, childProducts[child]);
				}
			}
			if (childIngredients != null && childIngredients.Count > 0)
			{
				foreach (CondensedProductionStep child in childIngredients.Keys)
				{
					child.ToProductionStep(recipes, step, childIngredients[child]);
				}
			}
			return step;
		}

		public CondensedProductionStep(ProductionStep step)
		{
			recipeUID = step.GetRecipe().UID;
			isProduct = step.IsProductOfParent;
			if (step.ChildIngredientSteps.Count > 0)
			{
				childIngredients = new Dictionary<CondensedProductionStep, string>();
				foreach (ProductionStep childStep in step.ChildIngredientSteps.Keys)
				{
					childIngredients.Add(new CondensedProductionStep(childStep), step.ChildIngredientSteps[childStep]);
				}
			}
			if (step.ChildProductSteps.Count > 0)
			{
				childProducts = new Dictionary<CondensedProductionStep, string>();
				foreach (ProductionStep childStep in step.ChildProductSteps.Keys)
				{
					childProducts.Add(new CondensedProductionStep(childStep), step.ChildProductSteps[childStep]);
				}
			}
		}
	}
}
