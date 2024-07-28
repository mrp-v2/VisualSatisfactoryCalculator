using System;

using VisualSatisfactoryCalculator.code.Numbers;

namespace VisualSatisfactoryCalculator.code.Extensions
{
	public static class NumberExtensions
	{
		public static decimal Abs(this decimal m)
		{
			return Math.Abs(m);
		}

		public static RationalNumber Sqrt(this RationalNumber x)
		{
			return x.Sqrt(0);
		}

		public static RationalNumber Sqrt(this RationalNumber x, RationalNumber epsilon)
		{
			if (x < 0)
			{
				throw new OverflowException("Cannot calculate square root from a negative number");
			}
			RationalNumber current = (decimal)Math.Sqrt((double)x.ToDecimalT()), previous;
			do
			{
				previous = current;
				if (previous == 0)
				{
					return 0;
				}
				current = (previous + (x / previous)) / 2;
			}
			while ((previous - current).AbsoluteValue() > epsilon);
			return current;
		}
	}
}
