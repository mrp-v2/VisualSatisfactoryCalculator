using System.Collections.Generic;
using VisualSatisfactoryCalculator.code.DataStorage;
using VisualSatisfactoryCalculator.code.Interfaces;

namespace VisualSatisfactoryCalculator.code.Extensions
{
	public static class ItemCountExtensions
	{
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

		public static List<ItemCount> GetProducts(this List<ItemCount> me)
		{
			List<ItemCount> products = new List<ItemCount>();
			foreach (ItemCount ic in me)
			{
				if (ic.Count > 0)
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
				if (ic.Count < 0)
				{
					ingredients.Add(ic);
				}
			}
			return ingredients;
		}

		public static bool ContainsItem(this List<ItemCount> me, IItem item)
		{
			foreach (ItemCount count in me)
			{
				if (count.ItemUID.Equals(item))
				{
					return true;
				}
			}
			return false;
		}

		public static ItemCount GetCountFor(this List<ItemCount> me, string itemUID)
		{
			foreach (ItemCount count in me)
			{
				if (count.ItemUID.Equals(itemUID))
				{
					return count;
				}
			}
			return default;
		}
	}
}
