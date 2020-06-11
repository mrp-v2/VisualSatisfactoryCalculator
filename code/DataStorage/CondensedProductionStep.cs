using System;
using System.Collections.Generic;

using VisualSatisfactoryCalculator.code.Interfaces;

namespace VisualSatisfactoryCalculator.code.DataStorage
{
	[Serializable]
	internal class CondensedProductionStep
	{
		protected readonly Dictionary<CondensedProductionStep, string> _childProducts;
		protected readonly Dictionary<CondensedProductionStep, string> _childIngredients;
		private readonly string _recipeUID;
		private readonly bool _isProduct;

		public IRecipe GetRecipe(Dictionary<string, IRecipe> recipes)
		{
			return recipes[_recipeUID];
		}

		public ProductionStep ToProductionStep(Dictionary<string, IRecipe> recipes, ProductionStep parent, string itemUID)
		{
			IRecipe myRecipe = GetRecipe(recipes);
			ProductionStep step = new ProductionStep(myRecipe, parent, itemUID, _isProduct);
			if (_childProducts != null && _childProducts.Count > 0)
			{
				foreach (CondensedProductionStep child in _childProducts.Keys)
				{
					_ = child.ToProductionStep(recipes, step, _childProducts[child]);
				}
			}
			if (_childIngredients != null && _childIngredients.Count > 0)
			{
				foreach (CondensedProductionStep child in _childIngredients.Keys)
				{
					_ = child.ToProductionStep(recipes, step, _childIngredients[child]);
				}
			}
			return step;
		}

		public CondensedProductionStep(ProductionStep step)
		{
			_recipeUID = step.GetRecipe().UID;
			_isProduct = step.IsProductOfParent;
			if (step.ChildIngredientSteps.Count > 0)
			{
				_childIngredients = new Dictionary<CondensedProductionStep, string>();
				foreach (ProductionStep childStep in step.ChildIngredientSteps.Keys)
				{
					_childIngredients.Add(new CondensedProductionStep(childStep), step.ChildIngredientSteps[childStep]);
				}
			}
			if (step.ChildProductSteps.Count > 0)
			{
				_childProducts = new Dictionary<CondensedProductionStep, string>();
				foreach (ProductionStep childStep in step.ChildProductSteps.Keys)
				{
					_childProducts.Add(new CondensedProductionStep(childStep), step.ChildProductSteps[childStep]);
				}
			}
		}
	}
}
