using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace VisualSatisfactoryCalculator.satisfactory.Numbers
{
	public sealed class RationalNumber
	{
		private readonly ImmutableList<int> PrimeFactoredNumerator, PrimeFactoredDenominator;
		public readonly bool IsPositive;
		public readonly bool IsNonZero;

		private RationalNumber(bool isNonZero, bool isPositive)
		{
			PrimeFactoredNumerator = ImmutableList.Create<int>();
			PrimeFactoredDenominator = ImmutableList.Create<int>();
			IsPositive = isPositive;
			IsNonZero = isNonZero;
		}

		private RationalNumber(IEnumerable<int> primeFactoredNumerator, IEnumerable<int> primeFactoredDenominator, bool isPositive, bool isNonZero)
		{
			PrimeFactoredNumerator = ImmutableList.CreateRange(primeFactoredNumerator);
			PrimeFactoredDenominator = ImmutableList.CreateRange(primeFactoredDenominator);
			IsPositive = isPositive;
			IsNonZero = isNonZero;
		}

		public RationalNumber(int numerator, int denominator) : this(numerator > 0 ? numerator : -numerator, denominator, numerator > 0) { }

		public RationalNumber(int numerator, int denominator, bool isPositive) : this(numerator == 1 ? new List<int>() : PrimeNumberHandler.PrimeFactors(numerator), PrimeNumberHandler.PrimeFactors(denominator), isPositive, true)
		{
			if (numerator == 0 || denominator == 0)
			{
				throw new ArgumentException("Should not have zero arguments here. Use a different method.");
			}
		}

		private static RationalNumber From(int value)
		{
			bool isPositive;
			if (value == 0)
			{
				return new RationalNumber(new List<int>(), new List<int>(), true, false);
			}
			isPositive = value > 0;
			value = isPositive ? value : -value;
			if (value == 1)
			{
				return new RationalNumber(new List<int>(), new List<int>(), isPositive, true);
			}
			else
			{
				return new RationalNumber(PrimeNumberHandler.PrimeFactors(value), new List<int>(), isPositive, true);
			}
		}

		public static RationalNumber FromDecimalString(string str)
		{
			str = str.TrimEnd('0');
			str = str.TrimEnd('.');
			if (str.Contains('.'))
			{
				int decimals = str.Length - str.IndexOf('.') - 1;
				str = str.Remove(str.IndexOf("."), 1);
				int numerator = int.Parse(str);
				if (numerator == 0)
				{
					return new RationalNumber(false, true);
				}
				bool isPositive = numerator > 0;
				int denominator = (int)Math.Pow(10, decimals);
				return new RationalNumber(PrimeNumberHandler.PrimeFactors(isPositive ? numerator : -numerator), PrimeNumberHandler.PrimeFactors(denominator), isPositive, true);
			}
			return From(int.Parse(str));
		}

		private RationalNumber Simplify()
		{
			List<int> numerator = new List<int>(PrimeFactoredNumerator);
			List<int> denominator = new List<int>(PrimeFactoredDenominator);
			Simplify(numerator, denominator);
			return new RationalNumber(numerator, denominator, IsPositive, IsNonZero);
		}

		private static void Simplify(List<int> numerator, List<int> denominator)
		{
			numerator.Sort();
			denominator.Sort();
			for (int i = 0; i < numerator.Count; i++)
			{
				if (denominator.Remove(numerator[i]))
				{
					numerator.RemoveAt(i--);
				}
			}
		}

		private RationalNumber Clone()
		{
			if (!IsNonZero)
			{
				return new RationalNumber(false, true);
			}
			return new RationalNumber(PrimeFactoredNumerator, PrimeFactoredDenominator, IsPositive, true);
		}

		public int GetNumerator()
		{
			if (!IsNonZero)
			{
				return 0;
			}
			int numerator = Product(PrimeFactoredNumerator);
			if (!IsPositive)
			{
				numerator = -numerator;
			}
			return numerator;
		}

		private static int Product(IEnumerable<int> factors)
		{
			int product = 1;
			foreach (int i in factors)
			{
				product *= i;
			}
			return product;
		}

		public int GetDenominator()
		{
			return Product(PrimeFactoredDenominator);
		}

		public double ToDouble()
		{
			return (double)GetNumerator() / GetDenominator();
		}

		public decimal ToDecimalT()
		{
			return ((decimal)GetNumerator()) / GetDenominator();
		}

		public bool AreSignsEqual(RationalNumber other)
		{
			return IsPositive == other.IsPositive;
		}

		public int Ceiling()
		{
			return (int)Math.Ceiling(ToDecimalT());
		}

		public RationalNumber Ceiling(int decimals)
		{
			int numerator = GetNumerator(), denominator = GetDenominator();
			int multiplier = (int)Math.Pow(10, decimals);
			numerator *= multiplier;
			numerator += denominator - (numerator % denominator);
			denominator *= multiplier;
			return new RationalNumber(numerator, denominator);
		}

		public RationalNumber AbsoluteValue()
		{
			return new RationalNumber(PrimeFactoredNumerator, PrimeFactoredDenominator, true, IsNonZero);
		}

		/// <summary>
		/// Returns a <c>RationalNumber</c> representing ten raised to the specified power
		/// </summary>
		/// <param name="power"></param>
		/// <returns></returns>
		public static RationalNumber Pow(int power)
		{
			if (power < 0)
			{
				throw new ArgumentException("Power should not be less than zero!");
			}
			RationalNumber result = new RationalNumber(true, true);
			List<int> tenFactors = new List<int>() { 2, 5 };
			for (int i = 0; i < power; i++)
			{
				result.PrimeFactoredNumerator.AddRange(tenFactors);
			}
			return result;
		}

		public static RationalNumber operator +(RationalNumber a)
		{
			return a;
		}

		public static RationalNumber operator -(RationalNumber a)
		{
			return new RationalNumber(a.PrimeFactoredNumerator, a.PrimeFactoredDenominator, !a.IsPositive, a.IsNonZero);
		}

		public static RationalNumber Add(RationalNumber a, RationalNumber b)
		{
			return a + b;
		}

		public static RationalNumber operator +(RationalNumber a, RationalNumber b)
		{
			if (!a.IsNonZero)
			{
				return b;
			}
			if (!b.IsNonZero)
			{
				return a;
			}
			ImmutableList<int> numeratorA = a.PrimeFactoredNumerator.AddRange(b.PrimeFactoredDenominator);
			ImmutableList<int> numeratorB = b.PrimeFactoredNumerator.AddRange(a.PrimeFactoredDenominator);
			ImmutableList<int> denominator = a.PrimeFactoredDenominator.AddRange(b.PrimeFactoredDenominator);
			int numerator = Product(numeratorA) + Product(numeratorB);
			if (numerator == 0)
			{
				return new RationalNumber(false, true);
			}
			bool isPositive = numerator > 0;
			if (!isPositive)
			{
				numerator = -numerator;
			}
			if (numerator == 1)
			{
				return new RationalNumber(true, isPositive);
			}
			RationalNumber result = new RationalNumber(PrimeNumberHandler.PrimeFactors(numerator), denominator, isPositive, true);
			return result.Simplify();
		}

		public static RationalNumber operator -(RationalNumber a, RationalNumber b)
		{
			return a + (-b);
		}

		public static RationalNumber operator *(RationalNumber a, RationalNumber b)
		{
			if (!a.IsNonZero || !b.IsNonZero)
			{
				return new RationalNumber(false, true);
			}
			bool isPositive = b.IsPositive ? a.IsPositive : !a.IsPositive;
			RationalNumber result = new RationalNumber(true, isPositive);
			result.PrimeFactoredNumerator.AddRange(a.PrimeFactoredNumerator);
			result.PrimeFactoredNumerator.AddRange(b.PrimeFactoredNumerator);
			result.PrimeFactoredDenominator.AddRange(a.PrimeFactoredDenominator);
			result.PrimeFactoredDenominator.AddRange(b.PrimeFactoredDenominator);
			return result.Simplify();
		}

		public static RationalNumber operator /(RationalNumber a, RationalNumber b)
		{
			if (!b.IsNonZero)
			{
				throw new ArgumentException("Can't divide by zero!");
			}
			if (!a.IsNonZero)
			{
				return new RationalNumber(false, true);
			}
			bool isPositive = a.IsPositive;
			if (!b.IsPositive)
			{
				isPositive = !isPositive;
			}
			RationalNumber result = new RationalNumber(true, isPositive);
			result.PrimeFactoredNumerator.AddRange(a.PrimeFactoredNumerator);
			result.PrimeFactoredNumerator.AddRange(b.PrimeFactoredDenominator);
			result.PrimeFactoredDenominator.AddRange(a.PrimeFactoredDenominator);
			result.PrimeFactoredDenominator.AddRange(b.PrimeFactoredNumerator);
			return result.Simplify();
		}

		public static bool operator ==(RationalNumber a, RationalNumber b)
		{
			return !(a != b);
		}

		public static bool operator !=(RationalNumber a, RationalNumber b)
		{
			if (a.IsPositive != b.IsPositive)
			{
				return true;
			}
			if (a.IsNonZero != b.IsNonZero)
			{
				return true;
			}
			if (a.PrimeFactoredNumerator.Count != b.PrimeFactoredNumerator.Count)
			{
				return true;
			}
			if (a.PrimeFactoredDenominator.Count != b.PrimeFactoredDenominator.Count)
			{
				return true;
			}
			for (int i = 0; i < a.PrimeFactoredNumerator.Count; i++)
			{
				if (a.PrimeFactoredNumerator[i] != b.PrimeFactoredNumerator[i])
				{
					return true;
				}
			}
			for (int i = 0; i < a.PrimeFactoredDenominator.Count; i++)
			{
				if (a.PrimeFactoredDenominator[i] != b.PrimeFactoredDenominator[i])
				{
					return true;
				}
			}
			return false;
		}

		public static bool operator ==(RationalNumber a, int b)
		{
			if (b == 0 && !a.IsNonZero)
			{
				return true;
			}
			if (a.PrimeFactoredNumerator.Count == 0 && a.PrimeFactoredDenominator.Count == 0)
			{
				if (b == 1 && a.IsPositive)
				{
					return true;
				}
				if (b == -1 && !a.IsPositive)
				{
					return true;
				}
			}
			if (a.PrimeFactoredDenominator.Count > 0)
			{
				return false;
			}
			return a.GetNumerator() == b;
		}

		public static bool operator !=(RationalNumber a, int b)
		{
			return !(a == b);
		}

		public static bool operator >(RationalNumber left, RationalNumber right)
		{
			if (!left.IsNonZero && !right.IsNonZero)
			{
				return false;
			}
			if (!left.IsNonZero)
			{
				return !right.IsPositive;
			}
			if (!right.IsNonZero)
			{
				return left.IsPositive;
			}
			if (left.IsPositive && !right.IsPositive)
			{
				return true;
			}
			if (right.IsPositive && !left.IsPositive)
			{
				return false;
			}
			return left.ToDecimalT() > right.ToDecimalT();
		}

		public static bool operator <(RationalNumber left, RationalNumber right)
		{
			return right > left;
		}

		public static RationalNumber operator *(RationalNumber a, int b)
		{
			if (b == 0 || !a.IsNonZero)
			{
				return new RationalNumber(false, true);
			}
			RationalNumber result = new RationalNumber(true, b > 0 ? a.IsPositive : !a.IsPositive);
			result.PrimeFactoredNumerator.AddRange(a.PrimeFactoredNumerator);
			result.PrimeFactoredDenominator.AddRange(a.PrimeFactoredDenominator);
			if (b < 0)
			{
				b = -b;
			}
			result.PrimeFactoredNumerator.AddRange(PrimeNumberHandler.PrimeFactors(b));
			return result.Simplify();
		}

		public static implicit operator RationalNumber(int a)
		{
			return From(a);
		}

		public static implicit operator RationalNumber(decimal d)
		{
			return FromDecimalString(d.ToString());
		}

		public override int GetHashCode()
		{
			return PrimeFactoredNumerator.GetHashCode() + PrimeFactoredDenominator.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj is RationalNumber other)
			{
				return this == other;
			}
			else
			{
				return false;
			}
		}

		public override string ToString()
		{
			if (!IsNonZero)
			{
				return 0.ToString();
			}
			if (PrimeFactoredDenominator.Count > 0)
			{
				return GetNumerator().ToString() + " / " + GetDenominator().ToString();
			}
			else
			{
				return GetNumerator().ToString();
			}
		}
	}
}
