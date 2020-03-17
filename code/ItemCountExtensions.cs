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

		/// <summary>
		/// Checks for equality based on equal contents, regardless of order
		/// </summary>
		/// <param name="me"></param>
		/// <param name="other"></param>
		/// <returns></returns>
		public static bool EqualContents(this List<ItemCount> me, List<ItemCount> other)
		{
			if (me.Count != other.Count)
			{
				return false;
			}
			Dictionary<ItemCount, int> meCounts = new Dictionary<ItemCount, int>();
			foreach (ItemCount ic in me)
			{
				if (meCounts.ContainsKey(ic))
				{
					meCounts[ic]++;
				} else
				{
					meCounts.Add(ic, 1);
				}
			}
			Dictionary<ItemCount, int> otherCounts = new Dictionary<ItemCount, int>();
			foreach (ItemCount ic in other)
			{
				if (otherCounts.ContainsKey(ic))
				{
					otherCounts[ic]++;
				} else
				{
					otherCounts.Add(ic, 1);
				}
			}
			foreach (ItemCount ic in meCounts.Keys)
			{
				if (meCounts[ic] != otherCounts[ic])
				{
					return false;
				}
			}
			return true;
		}
	}
}
