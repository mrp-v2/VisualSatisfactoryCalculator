using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.code
{
	public static class JSONItemListExtensions
	{
		public static string GetDisplayNameFor(this List<JSONItem> me, string uniqueID)
		{
			foreach (JSONItem item in me)
			{
				if (item.ClassName == uniqueID)
				{
					return item.mDisplayName;
				}
			}
			return "Unknown Item";
		}

		public static bool IsLiquid(this List<JSONItem> me , string uniqueID)
		{
			foreach (JSONItem item in me)
			{
				if (item.ClassName == uniqueID)
				{
					return item.mForm == "RF_LIQUID";
				}
			}
			return false;
		}

		public static Recipe GetRecipeFor(this List<JSONRecipe> me, List<JSONItem> descriptors, string id)
		{
			foreach (JSONRecipe recipe in me)
			{
				if (recipe.ClassName == id)
				{
					return recipe.ToRecipe(descriptors);
				}
			}
			return null;
		}
	}
}
