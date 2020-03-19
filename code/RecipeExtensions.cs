using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.code
{
	public static class RecipeExtensions
	{
		public static string ToStringC(this List<Recipe> me)
		{
			string str = "";
			foreach (Recipe recipe in me)
			{
				str += recipe.ToString() + "\n";
			}
			return str;
		}

		public static List<Item> GetAllProductItems(this List<Recipe> me)
		{
			List<Item> products = new List<Item>();
			foreach (Recipe rec in me)
			{
				products.AddRange(rec.GetProductItems());
			}
			return products;
		}

		public static List<Recipe> CastToRecipeList(this List<object> me)
		{
			List<Recipe> list = new List<Recipe>();
			foreach (object obj in me)
			{
				if (obj is Recipe)
				{
					list.Add(obj as Recipe);
				}
			}
			return list;
		}
	}
}
