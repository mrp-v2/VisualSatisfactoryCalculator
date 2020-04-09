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

		public static List<IItem> GetAllIngredientItems(this List<IRecipe> me)
		{
			List<IItem> ingredients = new List<IItem>();
			foreach (IRecipe rec in me)
			{
				ingredients.AddRange(rec.GetItemCounts().GetIngredients().ToItems());
			}
			return ingredients;
		}

		public static List<IItem> GetAllProductItems(this List<IRecipe> me)
		{
			List<IItem> products = new List<IItem>();
			foreach (IRecipe rec in me)
			{
				products.AddRange(rec.GetItemCounts().GetProducts().ToItems());
			}
			return products;
		}

		public static List<IRecipe> GetRecipesThatProduce(this List<IRecipe> me, IItem product)
		{
			List<IRecipe> recs = new List<IRecipe>();
			foreach (IRecipe rec in me)
			{
				if (rec.GetItemCounts().GetProducts().ContainsItem(product))
				{
					recs.Add(rec);
				}
			}
			return recs;
		}

		public static List<IRecipe> GetRecipesThatConsume(this List<IRecipe> me, IItem ingredient)
		{
			List<IRecipe> recs = new List<IRecipe>();
			foreach (IRecipe rec in me)
			{
				if (rec.GetItemCounts().GetIngredients().ContainsItem(ingredient))
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
				if (count.GetItem().Equals(item))
				{
					return count.GetCount() * (60m / me.GetCraftTime());
				}
			}
			return default;
		}
	}
}
