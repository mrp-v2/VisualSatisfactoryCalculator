using System.Collections.Generic;

namespace VisualSatisfactoryCalculator.code
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

		public static bool ContainsItem(this List<ItemCount> me, JSONItem item)
		{
			foreach (ItemCount count in me)
			{
				if (JSONItem.blank.Equals(count, item))
				{
					return true;
				}
			}
			return false;
		}

		public static ItemCount GetCountFor(this List<ItemCount> me, JSONItem item)
		{
			foreach (ItemCount count in me)
			{
				if (JSONItem.blank.Equals(count, item))
				{
					return count;
				}
			}
			return default;
		}
	}
}
