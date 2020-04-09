using System.Collections.Generic;
using VisualSatisfactoryCalculator.code.DataStorage;
using VisualSatisfactoryCalculator.code.Interfaces;

namespace VisualSatisfactoryCalculator.code.Extensions
{
	public static class RecipeExtensions
	{
		public static string ToStringC(this List<IRecipe> me)
		{
			string str = "";
			foreach (IRecipe recipe in me)
			{
				str += recipe.ToString() + "\n";
			}
			return str;
		}

		public static List<string> GetAllIngredientItemUIDs(this List<IRecipe> me)
		{
			List<string> ingredients = new List<string>();
			foreach (IRecipe rec in me)
			{
				ingredients.AddRange(rec.GetItemCounts().GetIngredients().ToItemUIDs());
			}
			return ingredients;
		}

		public static List<string> GetAllProductItemUIDs(this List<IRecipe> me)
		{
			List<string> products = new List<string>();
			foreach (IRecipe rec in me)
			{
				products.AddRange(rec.GetItemCounts().GetProducts().ToItemUIDs());
			}
			return products;
		}

		public static List<IRecipe> GetRecipesThatProduce(this List<IRecipe> me, string productUID)
		{
			List<IRecipe> recs = new List<IRecipe>();
			foreach (IRecipe rec in me)
			{
				if (rec.GetItemCounts().GetProducts().ToItemUIDs().Contains(productUID))
				{
					recs.Add(rec);
				}
			}
			return recs;
		}

		public static List<IRecipe> GetRecipesThatConsume(this List<IRecipe> me, string ingredientUID)
		{
			List<IRecipe> recs = new List<IRecipe>();
			foreach (IRecipe rec in me)
			{
				if (rec.GetItemCounts().GetIngredients().ToItemUIDs().Contains(ingredientUID))
				{
					recs.Add(rec);
				}
			}
			return recs;
		}

		public static decimal GetRateOf(this IRecipe me, IItem item)
		{
			foreach (ItemCount count in me.GetItemCounts())
			{
				if (count.GetItemUID().Equals(item))
				{
					return count.GetCount() * (60m / me.GetCraftTime());
				}
			}
			return default;
		}
	}
}
