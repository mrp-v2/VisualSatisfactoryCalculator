using System.Collections.Generic;
using System.Linq;
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
				ingredients.AddRange(rec.Ingredients.Keys);
			}
			return ingredients;
		}

		public static List<string> GetAllProductItemUIDs(this List<IRecipe> me)
		{
			List<string> products = new List<string>();
			foreach (IRecipe rec in me)
			{
				products.AddRange(rec.Products.Keys);
			}
			return products;
		}

		public static Dictionary<string, IRecipe> GetRecipesThatProduce(this Dictionary<string, IRecipe> me, string productUID)
		{
			Dictionary<string, IRecipe> recs = new Dictionary<string, IRecipe>();
			foreach (IRecipe rec in me.Values)
			{
				if (rec.Products.Keys.Contains(productUID))
				{
					recs.Add(rec.UID, rec);
				}
			}
			return recs;
		}

		public static Dictionary<string, IRecipe> GetRecipesThatConsume(this Dictionary<string, IRecipe> me, string ingredientUID)
		{
			Dictionary<string, IRecipe> recs = new Dictionary<string, IRecipe>();
			foreach (IRecipe rec in me.Values)
			{
				if (rec.Ingredients.Keys.Contains(ingredientUID))
				{
					recs.Add(rec.UID, rec);
				}
			}
			return recs;
		}
	}
}
