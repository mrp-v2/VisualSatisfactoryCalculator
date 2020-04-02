using System;
using System.Collections.Generic;

namespace VisualSatisfactoryCalculator.code
{
	public static class ProductionStepExtensions
	{
		public static List<ProductionStep> ShallowClone(this List<ProductionStep> me)
		{
			List<ProductionStep> list = new List<ProductionStep>();
			foreach (ProductionStep ps in me)
			{
				list.Add(ps);
			}
			return list;
		}

		public static List<Recipe> CastToRecipeList(this List<ProductionStep> me)
		{
			List<Recipe> list = new List<Recipe>();
			foreach (ProductionStep ps in me)
			{
				list.Add(ps);
			}
			return list;
		}

		public static Dictionary<int, List<ProductionStep>> ToTierList(this List<ProductionStep> me)
		{
			List<ProductionStep> cloned = me.ShallowClone();
			Dictionary<int, List<ProductionStep>> tiers = new Dictionary<int, List<ProductionStep>>();
			for (int i = 0; i < me.Count; i++)
			{
				List<ProductionStep> currentTier = new List<ProductionStep>();
				List<Item> AllRemainingIngredients = cloned.CastToRecipeList().GetAllIngredientItems();
				foreach (ProductionStep recipe in cloned)
				{
					if (!recipe.GetProductItems().ContainsAny(AllRemainingIngredients))
					{
						currentTier.Add(recipe);
					}
				}
				foreach (ProductionStep recipe in currentTier)
				{
					cloned.Remove(recipe);
				}
				tiers.Add(i, currentTier);
				if (cloned.Count == 0)
				{
					return tiers;
				}
			}
			Console.WriteLine("There are more tiers than recipes? This should not happen!");
			return tiers;
		}
	}
}
