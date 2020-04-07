using System.Collections.Generic;
using VisualSatisfactoryCalculator.code.JSONClasses;

namespace VisualSatisfactoryCalculator.code.Extensions
{
	public static class JSONRecipeExtensions
	{
		public static string ToStringC(this List<JSONRecipe> me)
		{
			string str = "";
			foreach (JSONRecipe recipe in me)
			{
				str += recipe.ToString() + "\n";
			}
			return str;
		}

		public static List<JSONItem> GetAllIngredientItems(this List<JSONRecipe> me)
		{
			List<JSONItem> ingredients = new List<JSONItem>();
			foreach (JSONRecipe rec in me)
			{
				ingredients.AddRange(rec.GetItemCounts().GetIngredients());
			}
			return ingredients;
		}

		public static List<JSONItem> GetAllProductItems(this List<JSONRecipe> me)
		{
			List<JSONItem> products = new List<JSONItem>();
			foreach (JSONRecipe rec in me)
			{
				products.AddRange(rec.GetItemCounts().GetProducts());
			}
			return products;
		}

		public static List<JSONRecipe> GetRecipesThatProduce(this List<JSONRecipe> me, JSONItem product)
		{
			List<JSONRecipe> recs = new List<JSONRecipe>();
			foreach (JSONRecipe rec in me)
			{
				if (rec.GetItemCounts().GetProducts().ContainsItem(product))
				{
					recs.Add(rec);
				}
			}
			return recs;
		}

		public static List<JSONRecipe> GetRecipesThatConsume(this List<JSONRecipe> me, JSONItem ingredient)
		{
			List<JSONRecipe> recs = new List<JSONRecipe>();
			foreach (JSONRecipe rec in me)
			{
				if (rec.GetItemCounts().GetIngredients().ContainsItem(ingredient))
				{
					recs.Add(rec);
				}
			}
			return recs;
		}
	}
}
