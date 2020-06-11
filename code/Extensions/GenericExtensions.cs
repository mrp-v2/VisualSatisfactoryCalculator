﻿using System;
using System.Collections.Generic;

namespace VisualSatisfactoryCalculator.code.Extensions
{
	internal static class GenericExtensions
	{
		public static void AddIfNew<T>(this List<T> me, T other)
		{
			if (!me.Contains(other))
			{
				me.Add(other);
			}
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

		public static void AddRange<TKey, TValueA, TValueB>(this Dictionary<TKey, TValueA> me, Dictionary<TKey, TValueB> other) where TValueB : TValueA
		{
			foreach (TKey key in other.Keys)
			{
				me.Add(key, other[key]);
			}
		}
	}
}
