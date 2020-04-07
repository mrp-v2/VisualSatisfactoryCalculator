using System.Collections.Generic;
using VisualSatisfactoryCalculator.code.JSONClasses;

namespace VisualSatisfactoryCalculator.code.Extensions
{
	public static class IItemListExtensions
	{
		public static JSONItem GetJSONItemFor(this List<JSONItem> me, string uniqueID)
		{
			foreach (JSONItem item in me)
			{
				if (item.EqualID(uniqueID))
				{
					return item;
				}
			}
			return default;
		}

		public static JSONRecipe GetRecipeFor(this List<JSONRecipe> me, string id)
		{
			foreach (JSONRecipe recipe in me)
			{
				if (recipe.EqualID(id))
				{
					return recipe;
				}
			}
			return null;
		}
	}
}
