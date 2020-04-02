using System;
using System.Collections.Generic;
using System.Linq;

namespace VisualSatisfactoryCalculator.code
{
	static class GenericExtensions
	{
		public static bool ContainsAny<T>(this IEnumerable<T> me, IEnumerable<T> other)
		{
			foreach (T item in other)
			{
				if (me.Contains(item))
				{
					return true;
				}
			}
			return false;
		}

		public static T FindMatch<T>(this IEnumerable<T> me, IEnumerable<T> other)
		{
			foreach (T item in me)
			{
				if (other.Contains(item))
				{
					return item;
				}
			}
			return default;
		}

		public static List<T> FindMatches<T>(this List<T> me, List<T> other)
		{
			List<T> matches = new List<T>();
			foreach (T item in me)
			{
				if (other.Contains(item))
				{
					matches.AddIfNew(item);
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

		public static List<object> Clone<T>(this List<T> me) where T : ICloneable
		{
			List<object> cloned = new List<object>();
			foreach (T item in me)
			{
				cloned.Add(item.Clone());
			}
			return cloned;
		}
	}
}
