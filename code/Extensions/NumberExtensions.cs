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
			str1 = str1.TrimEnd('0').TrimEnd('.');
			string str2 = Math.Round(m, Constants.DECIMALS).ToString();
			return str1.Length < str2.Length ? str1 : str2;
		}
	}
}
