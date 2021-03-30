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
			string str1 = m.ToString();
			if (str1.Contains("."))
			{
				str1 = str1.TrimEnd('0').TrimEnd('.');
			}
			string str2 = Math.Round(m, Constants.DECIMALS).ToString();
			return str1.Length < str2.Length ? str1 : str2;
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
	}
}
