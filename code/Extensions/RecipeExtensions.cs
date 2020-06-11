using System.Collections.Generic;
using System.Linq;

using VisualSatisfactoryCalculator.code.Interfaces;

namespace VisualSatisfactoryCalculator.code.Extensions
{
	public static class RecipeExtensions
	{
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
