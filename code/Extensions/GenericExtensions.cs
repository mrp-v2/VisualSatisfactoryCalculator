using System.Collections.Generic;
using System.Linq;

namespace VisualSatisfactoryCalculator.code.Extensions
{
	static class GenericExtensions
	{
		public static bool ContainsAny<T>(this IEnumerable<T> me, IEnumerable<T> other, IEqualityComparer<T> comparer)
		{
			foreach (T item in other)
			{
				if (me.Contains(item, comparer))
				{
					return true;
				}
			}
			return false;
		}

		public static T FindMatch<T>(this IEnumerable<T> me, IEnumerable<T> other, IEqualityComparer<T> comparer)
		{
			foreach (T item in me)
			{
				if (other.Contains(item, comparer))
				{
					return item;
				}
			}
			return default;
		}

		public static List<T> FindMatches<T>(this IEnumerable<T> me, IEnumerable<T> other, IEqualityComparer<T> comparer)
		{
			List<T> matches = new List<T>();
			foreach (T item in me)
			{
				if (other.Contains(item, comparer))
				{
					matches.Add(item);
				}
			}
			return matches;
		}

		public static void AddIfNew<T>(this List<T> me, T other)
		{
			if (!me.Contains(other))
			{
				me.Add(other);
			}
		}

		public static void AddRangeIfNew<T>(this List<T> me, IEnumerable<T> other)
		{
			foreach (T item in other)
			{
				me.AddIfNew(item);
			}
		}

		public static List<T> Clone<T>(this List<T> me) where T : IMyCloneable<T>
		{
			List<T> cloned = new List<T>();
			foreach (T item in me)
			{
				cloned.Add(item.Clone());
			}
			return cloned;
		}

		public static List<T> ShallowClone<T>(this List<T> me)
		{
			List<T> list = new List<T>();
			foreach (T rec in me)
			{
				list.Add(rec);
			}
			return list;
		}

		/// <summary>
		/// Checks for equality based on equal contents, regardless of order
		/// </summary>
		/// <param name="me"></param>
		/// <param name="other"></param>
		/// <returns></returns>
		public static bool EqualContents<T>(this List<T> me, List<T> other)
		{
			if (me.Count != other.Count)
			{
				return false;
			}
			Dictionary<T, int> meCounts = new Dictionary<T, int>();
			foreach (T item in me)
			{
				if (meCounts.ContainsKey(item))
				{
					meCounts[item]++;
				}
				else
				{
					meCounts.Add(item, 1);
				}
			}
			Dictionary<T, int> otherCounts = new Dictionary<T, int>();
			foreach (T item in other)
			{
				if (otherCounts.ContainsKey(item))
				{
					otherCounts[item]++;
				}
				else
				{
					otherCounts.Add(item, 1);
				}
			}
			foreach (T ic in meCounts.Keys)
			{
				try
				{
					if (meCounts[ic] != otherCounts[ic])
					{
						return false;
					}
				}
#pragma warning disable CS0168 // Variable is declared but never used
				catch (KeyNotFoundException e)
#pragma warning restore CS0168 // Variable is declared but never used
				{
					return false;
				}

			}
			return true;
		}

		/// <summary>
		/// Returns a new list that is a combination of this list and another list
		/// </summary>
		/// <param name="me"></param>
		/// <param name="other"></param>
		/// <returns></returns>
		public static List<T> Merge<T>(this List<T> me, List<T> other)
		{
			List<T> merged = new List<T>();
			merged.AddRange(me);
			merged.AddRange(other);
			return merged;
		}

		public static void AddRange<T, Y>(this Dictionary<T, List<Y>> me, Dictionary<T, List<Y>> other)
		{
			foreach (T key in other.Keys)
			{
				if (!me.ContainsKey(key))
				{
					me.Add(key, new List<Y>());
				}
				me[key].AddRange(other[key]);
			}
		}
	}
}
