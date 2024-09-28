using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using VisualSatisfactoryCalculator.satisfactory.model.production;

namespace VisualSatisfactoryCalculator.satisfactory.Extensions
{
	public static class RecipeExtensions
	{
		public static HashSet<Recipe> GetRecipesThatProduce(this ImmutableDictionary<string, Recipe> me, Item item)
		{
			HashSet<Recipe> recs = new HashSet<Recipe>();
			foreach (Recipe rec in me.Values)
			{
				if (rec.products.Keys.Contains(item))
				{
					recs.Add(rec);
				}
			}
			return recs;
		}

		public static HashSet<Recipe> GetRecipesThatConsume(this ImmutableDictionary<string, Recipe> me, Item item)
		{
			HashSet<Recipe> recs = new HashSet<Recipe>();
			foreach (Recipe rec in me.Values)
			{
				if (rec.ingredients.Keys.Contains(item))
				{
					recs.Add(rec);
				}
			}
			return recs;
		}
	}
}
