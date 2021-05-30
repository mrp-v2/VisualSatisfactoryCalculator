using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.code.Numbers
{
	readonly struct RationalNumber
	{
		private readonly long numerator, denominator;

		public RationalNumber(long numerator, long denominator)
		{
			if (denominator == 0)
			{
				throw new ArgumentException("Denominator cannot be zero.", nameof(denominator));
			}
			(long, long) fraction = Simplify(numerator, denominator);
			this.numerator = fraction.Item1;
			this.denominator = fraction.Item2;
		}

		private RationalNumber(long numerator, long denominator, bool simplify)
		{
			if (denominator == 0)
			{
				throw new ArgumentException("Denominator cannot be zero.", nameof(denominator));
			}
			if (simplify)
			{
				(long, long) fraction = Simplify(numerator, denominator);
				this.numerator = fraction.Item1;
				this.denominator = fraction.Item2;
			}
			else
			{
				this.numerator = numerator;
				this.denominator = denominator;
			}
		}


		private static (long, long) Simplify(long numerator, long denominator)
		{
			if (denominator > 1)
			{
				if (numerator % denominator == 0)
				{
					return (numerator / denominator, 1);
				}
				long gcf = RationalNumberUtil.GreatestCommonFactor(numerator, denominator);
				if (gcf != 1)
				{
					return (numerator / gcf, denominator / gcf);
				}
			}
			return (numerator, denominator);
		}

		public static RationalNumber operator +(RationalNumber a) => a;
		public static RationalNumber operator -(RationalNumber a) => new RationalNumber(-a.numerator, a.denominator, false);

		public static RationalNumber operator +(RationalNumber a, RationalNumber b) => new RationalNumber(a.numerator * b.denominator + b.numerator * a.denominator, a.denominator * b.denominator);
		public static RationalNumber operator -(RationalNumber a, RationalNumber b) => a + (-b);

		public static RationalNumber operator *(RationalNumber a, RationalNumber b) => new RationalNumber(a.numerator * b.numerator, a.denominator * b.denominator);
		public static RationalNumber operator /(RationalNumber a, RationalNumber b) => new RationalNumber(a.numerator * b.denominator, b.numerator * a.denominator);

		public override string ToString() => $"{numerator} / {denominator}";

		public float ToFloat()
		{
			return numerator / (float)denominator;
		}

		public static RationalNumber From(decimal d)
		{
			long denominator = 1;
			while (d - Math.Round(d) != 0)
			{
				d *= 10;
				denominator *= 10;
			}
			return new RationalNumber((long)d, denominator);
		}
	}
}
