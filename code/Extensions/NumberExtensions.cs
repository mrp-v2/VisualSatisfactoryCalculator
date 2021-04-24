using System;

using VisualSatisfactoryCalculator.code.Utility;

namespace VisualSatisfactoryCalculator.code.Extensions
{
	public static class NumberExtensions
	{
		public static decimal Abs(this decimal m)
		{
			return Math.Abs(m);
		}

		public static string ToPrettyString(this decimal m)
		{
			string str = Math.Round(m, Constants.DECIMALS).ToString();
			if (str.Contains("."))
			{
				str = str.TrimEnd('0').TrimEnd('.');
			}
			return str;
		}

		public static decimal Sqrt(this decimal x, decimal epsilon = 0.0M)
		{
			if (x < 0)
			{
				throw new OverflowException("Cannot calculate square root from a negative number");
			}
			decimal current = (decimal)Math.Sqrt((double)x), previous;
			do
			{
				previous = current;
				if (previous == 0.0M)
				{
					return 0;
				}
				current = (previous + (x / previous)) / 2;
			}
			while (Math.Abs(previous - current) > epsilon);
			return current;
		}

		/// <summary>
		/// Checks if two decimals are both greater than zero or both less than zero
		/// </summary>
		public static bool AreSignsEqual(this decimal a, decimal b)
		{
			return (a < 0 && b < 0) || (a > 0 && b > 0);
		}

		/// <summary>
		/// Whether two decimals are equals within <see cref="Constants.DECIMALS"/> decimal places.
		/// </summary>
		public static bool ApproximatelyEqual(this decimal a, decimal b)
		{
			a = Math.Round(a, Constants.DECIMALS);
			b = Math.Round(b, Constants.DECIMALS);
			return a == b;
		}
	}
}
