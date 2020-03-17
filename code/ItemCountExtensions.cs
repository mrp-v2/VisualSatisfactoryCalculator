using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.code
{
	public static class ItemCountExtensions
	{
		/// <summary>
		/// Returns a new list that is a combination of this list and another list
		/// </summary>
		/// <param name="me"></param>
		/// <param name="other"></param>
		/// <returns></returns>
		public static List<ItemCount> Merge(this List<ItemCount> me, List<ItemCount> other)
		{
			List<ItemCount> merged = new List<ItemCount>();
			merged.AddRange(me);
			merged.AddRange(other);
			return merged;
		}

		/// <summary>
		/// Returns a clone of this list in which the count for each ItemCount has been multiplied by -1
		/// </summary>
		/// <param name="me"></param>
		/// <returns></returns>
		public static List<ItemCount> Inverse(this List<ItemCount> me)
		{
			List<ItemCount> flipped = new List<ItemCount>();
			foreach (ItemCount ic in me)
			{
				flipped.Add(ic.Inverse());
			}
			return flipped;
		}

		/// <summary>
		/// Returns a copy of this ItemCount whose count has been multiplied by -1
		/// </summary>
		/// <param name="me"></param>
		/// <returns></returns>
		public static ItemCount Inverse(this ItemCount me)
		{
			return new ItemCount(me.GetItem(), -me.GetCount());
		}

		public static List<ItemCount> GetProducts(this List<ItemCount> me)
		{
			List<ItemCount> products = new List<ItemCount>();
			foreach (ItemCount ic in me)
			{
				if (ic.GetCount() > 0)
				{
					products.Add(ic);
				}
			}
			return products;
		}

		public static List<ItemCount> GetIngredients(this List<ItemCount> me)
		{
			List<ItemCount> ingredients = new List<ItemCount>();
			foreach (ItemCount ic in me)
			{
				if (ic.GetCount() < 0)
				{
					ingredients.Add(ic);
				}
			}
			return ingredients;
		}
	}
}
