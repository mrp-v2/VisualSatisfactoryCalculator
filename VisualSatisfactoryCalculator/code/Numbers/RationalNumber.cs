using System;
using System.Collections.Generic;
using System.Linq;

namespace VisualSatisfactoryCalculator.code.Numbers
{
	public class RationalNumber
	{
		private readonly List<int> PrimeFactoredNumerator, PrimeFactoredDenominator;
		public readonly bool IsPositive;
		public readonly bool IsNonZero;

		private RationalNumber(bool isNonZero, bool isPositive)
		{
			if (!isNonZero && !isPositive)
			{
				throw new ArgumentException("Can't have a negative zero value.");
			}
			PrimeFactoredNumerator = new List<int>();
			PrimeFactoredDenominator = new List<int>();
			IsNonZero = isNonZero;
			IsPositive = isPositive;
		}

		private RationalNumber(IEnumerable<int> primeFactoredNumerator, IEnumerable<int> primeFactoredDenominator, bool isPositive)
		{
			PrimeFactoredNumerator = new List<int>();
			PrimeFactoredDenominator = new List<int>();
			PrimeFactoredNumerator.AddRange(primeFactoredNumerator);
			PrimeFactoredDenominator.AddRange(primeFactoredDenominator);
			IsPositive = isPositive;
			IsNonZero = true;
		}

		public RationalNumber(int numerator, int denominator) : this(numerator > 0 ? numerator : -numerator, denominator, numerator > 0)
		{

		}

		public RationalNumber(int numerator, int denominator, bool isPositive)
		{
			if (numerator == 0 || denominator == 0)
			{
				throw new ArgumentException("Should not have zero arguments here. Use a different method.");
			}
			IsNonZero = true;
			IsPositive = isPositive;
			if (numerator == 1)
			{
				PrimeFactoredNumerator = new List<int>();
				PrimeFactoredDenominator = PrimeNumberHandler.PrimeFactors(denominator);
			}
			else
			{
				PrimeFactoredNumerator = PrimeNumberHandler.PrimeFactors(numerator);
				PrimeFactoredDenominator = PrimeNumberHandler.PrimeFactors(denominator);
			}
		}

		private RationalNumber(int value)
		{
			if (value == 0)
			{
				IsNonZero = false;
				IsPositive = true;
				PrimeFactoredNumerator = new List<int>();
				PrimeFactoredDenominator = new List<int>();
				return;
			}
			IsNonZero = true;
			IsPositive = value > 0;
			value = IsPositive ? value : -value;
			if (value == 1)
			{
				PrimeFactoredNumerator = new List<int>();
			}
			else
			{
				PrimeFactoredNumerator = PrimeNumberHandler.PrimeFactors(value);
			}
			PrimeFactoredDenominator = new List<int>();
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
				return new RationalNumber(PrimeNumberHandler.PrimeFactors(isPositive ? numerator : -numerator), PrimeNumberHandler.PrimeFactors(denominator), isPositive);
			}
			return new RationalNumber(int.Parse(str));
		}

		private void Simplify()
		{
			PrimeFactoredNumerator.Sort();
			PrimeFactoredDenominator.Sort();
			for (int i = 0; i < PrimeFactoredNumerator.Count; i++)
			{
				if (PrimeFactoredDenominator.Remove(PrimeFactoredNumerator[i]))
				{
					PrimeFactoredNumerator.RemoveAt(i--);
				}
			}
		}

		private RationalNumber Clone()
		{
			if (!IsNonZero)
			{
				return new RationalNumber(false, true);
			}
			return new RationalNumber(PrimeFactoredNumerator, PrimeFactoredDenominator, IsPositive);
		}

		public int GetNumerator()
		{
			if (!IsNonZero)
			{
				return 0;
			}
			int numerator = 1;
			foreach (int i in PrimeFactoredNumerator)
			{
				numerator *= i;
			}
			if (!IsPositive)
			{
				numerator = -numerator;
			}
			return numerator;
		}

		public int GetDenominator()
		{
			int denominator = 1;
			foreach (int i in PrimeFactoredDenominator)
			{
				denominator *= i;
			}
			return denominator;
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
			return new RationalNumber(PrimeFactoredNumerator, PrimeFactoredDenominator, true);
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
			return new RationalNumber(a.PrimeFactoredNumerator, a.PrimeFactoredDenominator, !a.IsPositive);
		}

		public static RationalNumber Add(RationalNumber a, RationalNumber b)
		{
			return a + b;
		}

		public static RationalNumber operator +(RationalNumber a, RationalNumber b)
		{
			a = a.Clone();
			b = b.Clone();
			if (!a.IsNonZero)
			{
				return b;
			}
			if (!b.IsNonZero)
			{
				return a;
			}
			RationalNumber c = a.Clone();
			a.PrimeFactoredNumerator.AddRange(b.PrimeFactoredDenominator);
			a.PrimeFactoredDenominator.AddRange(b.PrimeFactoredDenominator);
			b.PrimeFactoredNumerator.AddRange(c.PrimeFactoredDenominator);
			b.PrimeFactoredDenominator.AddRange(c.PrimeFactoredDenominator);
			int numerator = a.GetNumerator() + b.GetNumerator();
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
			RationalNumber result = new RationalNumber(PrimeNumberHandler.PrimeFactors(numerator), a.PrimeFactoredDenominator, isPositive);
			result.Simplify();
			return result;
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
			result.Simplify();
			return result;
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
			result.Simplify();
			return result;
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
			result.Simplify();
			return result;
		}

		public static implicit operator RationalNumber(int a)
		{
			return new RationalNumber(a);
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
