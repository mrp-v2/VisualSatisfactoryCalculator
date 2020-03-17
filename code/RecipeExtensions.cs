using System;
using System.Collections.Generic;
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
				str += me.ToString() + "\n";
			}
			return str;
		}
	}
}
