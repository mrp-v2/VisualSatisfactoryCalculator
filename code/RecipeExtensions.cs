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

		public static List<string> GetAllProductItems(this List<Recipe> me)
		{
			List<string> products = new List<string>();
			foreach (Recipe rec in me)
			{
				products.AddRange(rec.GetProductItems());
			}
			return products;
		}
	}
}
