using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace VisualSatisfactoryCalculator.satisfactory.Numbers
{
	/// <summary>
	/// Represents any rational number as a prime-factored numerator and denominator.
	/// </summary>
	public sealed class RationalNumber
	{
		private readonly ImmutableList<int> _primeFactoredNumerator, _primeFactoredDenominator;
		public readonly bool isPositive;
		public readonly bool isNonZero;

		private RationalNumber(bool isNonZero, bool isPositive)
		{
			_primeFactoredNumerator = ImmutableList.Create<int>();
			_primeFactoredDenominator = ImmutableList.Create<int>();
			this.isPositive = isPositive;
			this.isNonZero = isNonZero;
		}

		private RationalNumber(IEnumerable<int> primeFactoredNumerator, IEnumerable<int> primeFactoredDenominator, bool isPositive, bool isNonZero)
		{
			_primeFactoredNumerator = ImmutableList.CreateRange(primeFactoredNumerator);
			_primeFactoredDenominator = ImmutableList.CreateRange(primeFactoredDenominator);
			this.isPositive = isPositive;
			this.isNonZero = isNonZero;
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

		public int GetNumerator()
		{
			if (!isNonZero)
			{
				return 0;
			}
			int numerator = Product(_primeFactoredNumerator);
			if (!isPositive)
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
			return Product(_primeFactoredDenominator);
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
			return isPositive == other.isPositive;
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
			return new RationalNumber(_primeFactoredNumerator, _primeFactoredDenominator, true, isNonZero);
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
				result._primeFactoredNumerator.AddRange(tenFactors);
			}
			return result;
		}

		public static RationalNumber operator +(RationalNumber a)
		{
			return a;
		}

		public static RationalNumber operator -(RationalNumber a)
		{
			return new RationalNumber(a._primeFactoredNumerator, a._primeFactoredDenominator, !a.isPositive, a.isNonZero);
		}

		public static RationalNumber Add(RationalNumber a, RationalNumber b)
		{
			return a + b;
		}

		public static RationalNumber operator +(RationalNumber a, RationalNumber b)
		{
			if (!a.isNonZero)
			{
				return b;
			}
			if (!b.isNonZero)
			{
				return a;
			}
			ImmutableList<int> numeratorA = a._primeFactoredNumerator.AddRange(b._primeFactoredDenominator);
			ImmutableList<int> numeratorB = b._primeFactoredNumerator.AddRange(a._primeFactoredDenominator);
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
			List<int> finalNumerator = PrimeNumberHandler.PrimeFactors(numerator);
			List<int> denominator = new List<int>(a._primeFactoredDenominator.Count + b._primeFactoredDenominator.Count);
			denominator.AddRange(a._primeFactoredDenominator);
			denominator.AddRange(b._primeFactoredDenominator);
			Simplify(finalNumerator, denominator);
			return new RationalNumber(finalNumerator, denominator, isPositive, true);
		}

		public static RationalNumber operator -(RationalNumber a, RationalNumber b)
		{
			return a + (-b);
		}

		public static RationalNumber operator *(RationalNumber a, RationalNumber b)
		{
			if (!a.isNonZero || !b.isNonZero)
			{
				return new RationalNumber(false, true);
			}
			bool isPositive = b.isPositive ? a.isPositive : !a.isPositive;
			List<int> numerator = new List<int>(a._primeFactoredNumerator.Count + b._primeFactoredNumerator.Count);
			List<int> denominator = new List<int>(a._primeFactoredDenominator.Count + b._primeFactoredDenominator.Count);
			numerator.AddRange(a._primeFactoredNumerator);
			numerator.AddRange(b._primeFactoredNumerator);
			denominator.AddRange(a._primeFactoredDenominator);
			denominator.AddRange(b._primeFactoredDenominator);
			Simplify(numerator, denominator);
			return new RationalNumber(numerator, denominator, isPositive, true);
		}

		public static RationalNumber operator /(RationalNumber a, RationalNumber b)
		{
			if (!b.isNonZero)
			{
				throw new ArgumentException("Can't divide by zero!");
			}
			if (!a.isNonZero)
			{
				return new RationalNumber(false, true);
			}
			bool isPositive = a.isPositive;
			if (!b.isPositive)
			{
				isPositive = !isPositive;
			}
			List<int> numerator = new List<int>(a._primeFactoredNumerator.Count + b._primeFactoredDenominator.Count);
			List<int> denominator = new List<int>(a._primeFactoredDenominator.Count + b._primeFactoredNumerator.Count);
			numerator.AddRange(a._primeFactoredNumerator);
			numerator.AddRange(b._primeFactoredDenominator);
			denominator.AddRange(a._primeFactoredDenominator);
			denominator.AddRange(b._primeFactoredNumerator);
			Simplify(numerator, denominator);
			return new RationalNumber(numerator, denominator, isPositive, true);
		}

		public static bool operator ==(RationalNumber a, RationalNumber b)
		{
			return !(a != b);
		}

		public static bool operator !=(RationalNumber a, RationalNumber b)
		{
			if (a.isPositive != b.isPositive)
			{
				return true;
			}
			if (a.isNonZero != b.isNonZero)
			{
				return true;
			}
			if (a._primeFactoredNumerator.Count != b._primeFactoredNumerator.Count)
			{
				return true;
			}
			if (a._primeFactoredDenominator.Count != b._primeFactoredDenominator.Count)
			{
				return true;
			}
			for (int i = 0; i < a._primeFactoredNumerator.Count; i++)
			{
				if (a._primeFactoredNumerator[i] != b._primeFactoredNumerator[i])
				{
					return true;
				}
			}
			for (int i = 0; i < a._primeFactoredDenominator.Count; i++)
			{
				if (a._primeFactoredDenominator[i] != b._primeFactoredDenominator[i])
				{
					return true;
				}
			}
			return false;
		}

		public static bool operator ==(RationalNumber a, int b)
		{
			if (b == 0 && !a.isNonZero)
			{
				return true;
			}
			if (a._primeFactoredNumerator.Count == 0 && a._primeFactoredDenominator.Count == 0)
			{
				if (b == 1 && a.isPositive)
				{
					return true;
				}
				if (b == -1 && !a.isPositive)
				{
					return true;
				}
			}
			if (a._primeFactoredDenominator.Count > 0)
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
			if (!left.isNonZero && !right.isNonZero)
			{
				return false;
			}
			if (!left.isNonZero)
			{
				return !right.isPositive;
			}
			if (!right.isNonZero)
			{
				return left.isPositive;
			}
			if (left.isPositive && !right.isPositive)
			{
				return true;
			}
			if (right.isPositive && !left.isPositive)
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
			if (b == 0 || !a.isNonZero)
			{
				return new RationalNumber(false, true);
			}
			List<int> numerator = new List<int>(a._primeFactoredNumerator);
			List<int> denominator = new List<int>(a._primeFactoredDenominator);
			if (b < 0)
			{
				b = -b;
			}
			numerator.AddRange(PrimeNumberHandler.PrimeFactors(b));
			Simplify(numerator, denominator);
			return new RationalNumber(numerator, denominator, b > 0 ? a.isPositive : !a.isPositive, true);
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
			return _primeFactoredNumerator.GetHashCode() + _primeFactoredDenominator.GetHashCode();
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
			if (!isNonZero)
			{
				return 0.ToString();
			}
			if (_primeFactoredDenominator.Count > 0)
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
