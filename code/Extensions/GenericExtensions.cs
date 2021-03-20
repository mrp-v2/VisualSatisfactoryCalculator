using System;
using System.Collections.Generic;

namespace VisualSatisfactoryCalculator.code.Extensions
{
	internal static class GenericExtensions
	{
		public static void AddRange<T>(this ICollection<T> me, ICollection<T> collection)
		{
			foreach (T item in collection)
			{
				me.Add(item);
			}
		}

		public static List<T> ToList<T>(this ICollection<T> me)
		{
			List<T> list = new List<T>();
			list.AddRange(me);
			return list;
		}

		public static T[] SubArray<T>(this T[] me, int startIndex, int length)
		{
			if (startIndex + length > me.Length)
			{
				throw new ArgumentException("The given length exceeds the size of the array!");
			}

			T[] subArray = new T[length];
			for (int i = 0; i < length; i++)
			{
				subArray[i] = me[startIndex + i];
			}

			return subArray;
		}

		public static T[] AddElement<T>(this T[] me, T element)
		{
			T[] newMe = new T[me.Length + 1];
			me.CopyTo(newMe, 0);
			newMe[me.Length] = element;
			return newMe;
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
	}
}
