using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.code.Numbers
{
	[Serializable]
	public readonly struct RationalNumber
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

		public bool AreSignsEqual(RationalNumber other)
		{
			return (denominator > 0) == (other.denominator > 0) == ((numerator > 0) == (other.numerator > 0));
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

		public static RationalNumber operator +(RationalNumber a)
		{
			return a;
		}

		public static RationalNumber operator -(RationalNumber a)
		{
			return new RationalNumber(-a.numerator, a.denominator, false);
		}

		public static RationalNumber operator +(RationalNumber a, RationalNumber b)
		{
			return new RationalNumber((a.numerator * b.denominator) + (b.numerator * a.denominator), a.denominator * b.denominator);
		}

		public static RationalNumber operator -(RationalNumber a, RationalNumber b)
		{
			return a + (-b);
		}

		public static RationalNumber operator *(RationalNumber a, RationalNumber b)
		{
			return new RationalNumber(a.numerator * b.numerator, a.denominator * b.denominator);
		}

		public static RationalNumber operator /(RationalNumber a, RationalNumber b)
		{
			return new RationalNumber(a.numerator * b.denominator, b.numerator * a.denominator);
		}

		public static bool operator ==(RationalNumber a, RationalNumber b)
		{
			return !(a != b);
		}

		public override bool Equals(object obj)
		{
			if (obj is RationalNumber other)
			{
				return other == this;
			}
			else
			{
				return false;
			}
		}

		public override int GetHashCode()
		{
			return ToDecimal().GetHashCode();
		}

		public RationalNumber Abs()
		{
			return new RationalNumber(Math.Abs(numerator), Math.Abs(denominator));
		}

		public static bool operator !=(RationalNumber a, RationalNumber b)
		{
			if (a.numerator != b.numerator)
			{
				return true;
			}
			if (a.denominator != b.denominator)
			{
				return true;
			}
			return false;
		}

		public static implicit operator RationalNumber(int i)
		{
			return new RationalNumber(i, 1);
		}

		public static bool operator <(RationalNumber a, RationalNumber b)
		{
			return a.numerator * b.denominator < b.numerator * a.denominator;
		}

		public static bool operator >(RationalNumber a, RationalNumber b)
		{
			return b < a;
		}

		public override string ToString()
		{
			return $"{numerator} / {denominator}";
		}

		public decimal ToDecimal()
		{
			return numerator / (decimal)denominator;
		}

		public static implicit operator RationalNumber(decimal d)
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
