using System;
using System.Collections.Generic;

namespace VisualSatisfactoryCalculator.code
{
	public static class ProductionStepExtensions
	{
		public static List<ProductionStep> ShallowClone(this List<ProductionStep> me)
		{
			List<ProductionStep> list = new List<ProductionStep>();
			foreach (ProductionStep ps in me)
			{
				list.Add(ps);
			}
			return list;
		}

		public static List<JSONRecipe> CastToRecipeList(this List<ProductionStep> me)
		{
			List<JSONRecipe> list = new List<JSONRecipe>();
			foreach (ProductionStep ps in me)
			{
				list.Add(ps);
			}
			return list;
		}
	}
}
