using System;

namespace VisualSatisfactoryCalculator.satisfactory.Extensions
{
	public static class NumberExtensions
	{
		public static decimal Abs(this decimal m)
		{
			return Math.Abs(m);
		}

		public static decimal Sqrt(this decimal x)
		{
			return x.Sqrt(0);
		}

		public static decimal Sqrt(this decimal x, decimal epsilon)
		{
			if (x < 0)
			{
				throw new OverflowException("Cannot calculate square root from a negative number");
			}
			decimal current = (decimal)Math.Sqrt((double)x), previous;
			do
			{
				previous = current;
				if (previous == 0)
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
