using System;
using System.Collections.Generic;
using System.Linq;
using VisualSatisfactoryCalculator.code.Interfaces;

namespace VisualSatisfactoryCalculator.code.Extensions
{
	static class GenericExtensions
	{
		public static bool ContainsAny<T>(this IEnumerable<T> me, IEnumerable<T> other)
		{
			foreach (T item in other) if (me.Contains(item)) return true;
			return false;
		}

		public static T FindMatch<T>(this IEnumerable<T> me, IEnumerable<T> other)
		{
			foreach (T item in me) if (other.Contains(item)) return item;
			return default;
		}

		public static List<T> FindMatches<T>(this IEnumerable<T> me, IEnumerable<T> other)
		{
			List<T> matches = new List<T>();
			foreach (T item in me) if (other.Contains(item)) matches.Add(item);
			return matches;
		}

		public static void AddIfNew<T>(this List<T> me, T other)
		{
			if (!me.Contains(other)) me.Add(other);
		}

		public static void AddRangeIfNew<T>(this List<T> me, IEnumerable<T> other)
		{
			foreach (T item in other) me.AddIfNew(item);
		}

		public static List<T> ShallowClone<T>(this List<T> me)
		{
			List<T> list = new List<T>();
			foreach (T rec in me) list.Add(rec);
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
			if (me.Count != other.Count) return false;
			Dictionary<T, int> meCounts = new Dictionary<T, int>();
			foreach (T item in me)
			{
				if (!other.Contains(item)) return false;
				if (meCounts.ContainsKey(item)) meCounts[item]++;
				else meCounts.Add(item, 1);
			}
			Dictionary<T, int> otherCounts = new Dictionary<T, int>();
			foreach (T item in other)
			{
				if (!me.Contains(item)) return false;
				if (otherCounts.ContainsKey(item)) otherCounts[item]++;
				else otherCounts.Add(item, 1);
			}
			foreach (T key in meCounts.Keys) if (meCounts[key] != otherCounts[key]) return false;
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
				if (!me.ContainsKey(key)) me.Add(key, new List<Y>());
				me[key].AddRange(other[key]);
			}
		}

		public static T FindByID<T>(this IEnumerable<T> me, string id) where T : IHasUID
		{
			foreach (T item in me) if (item.EqualID(id)) return item;
			throw new ArgumentException("The given ID did not match any items in the list.");
		}

		public static T[] SubArray<T>(this T[] me, int startIndex, int length)
		{
			if (startIndex + length > me.Length) throw new ArgumentException("The given length exceeds the size of the array!");
			T[] subArray = new T[length];
			for (int i = 0; i < length; i++) subArray[i] = me[startIndex + i];
			return subArray;
		}

		public static T[] AddElement<T>(this T[] me, T element)
		{
			T[] newMe = new T[me.Length + 1];
			me.CopyTo(newMe, 0);
			newMe[me.Length] = element;
			return newMe;
		}

		public static string GetDisplayNameFor(this IEnumerable<IEncoder> me, string UID)
		{
			foreach (IEncoder UIDEncoder in me) if (UIDEncoder.EqualID(UID)) return UIDEncoder.DisplayName;
			return UID;
		}

		public static Dictionary<T, decimal> Subtract<T>(this Dictionary<T, decimal> me, Dictionary<T, decimal> other)
		{
			Dictionary<T, decimal> merged = new Dictionary<T, decimal>();
			foreach (T key in me.Keys)
			{
				if (merged.ContainsKey(key))
				{
					merged[key] += me[key];
				}
				else
				{
					merged.Add(key, me[key]);
				}
			}
			foreach (T key in other.Keys)
			{
				if (merged.ContainsKey(key))
				{
					merged[key] -= other[key];
				}
				else
				{
					merged.Add(key, -other[key]);
				}
			}
			return merged;
		}

		public static void AddRange<TKey, TValueA, TValueB>(this Dictionary<TKey, TValueA> me, Dictionary<TKey, TValueB> other) where TValueB : TValueA
		{
			foreach (TKey key in other.Keys)
			{
				me.Add(key, other[key]);
			}
		}
	}
}
