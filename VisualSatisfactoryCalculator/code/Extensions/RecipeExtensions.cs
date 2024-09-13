using System.Collections.Generic;
using System.Linq;

using VisualSatisfactoryCalculator.satisfactory.Interfaces;
using VisualSatisfactoryCalculator.satisfactory.JSONClasses;

namespace VisualSatisfactoryCalculator.satisfactory.Extensions
{
	public static class RecipeExtensions
	{
		public static Dictionary<JSONItem, IRecipe> GetRecipesThatProduce(this Dictionary<JSONItem, IRecipe> me, JSONItem item)
		{
			Dictionary<string, IRecipe> recs = new Dictionary<string, IRecipe>();
			foreach (IRecipe rec in me.Values)
			
				if (rec.Products.Keys.Contains(item))
				{
					recs.Add(rec.ID, rec);
				}
			}
			return recs;
		}

		public static Dictionary<JSONItem, IRecipe> GetRecipesThatConsume(this Dictionary<JSONItem, IRecipe> me, JSONItem item)
		{
			Dictionary<string, IRecipe> recs = new Dictionary<string, IRecipe>();
			foreach (IRecipe rec in me.Values)
			{
				if (rec.Ingredients.Keys.Contains(item))
				{
					recs.Add(rec.ID, rec);
				}
			}
			return recs;
		}
	}
}
