using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.code.Numbers
{
	internal static class PrimeNumberHandler
	{
		private static readonly List<long> primeNumbers = new List<long>() { 2, 3, 5, 7 };

		public static void FindNextPrimeNumber()
		{
			long current = primeNumbers[primeNumbers.Count - 1] + 2;
			while (!IsPrime(current))
			{
				current += 2;
			}
			primeNumbers.Add(current);
		}

		private static bool IsPrime(long number)
		{
			long sqrt = (long)Math.Sqrt(number) + 1;
			foreach (long test in primeNumbers)
			{
				if (test > sqrt)
				{
					break;
				}
				if (number % test == 0)
				{
					return false;
				}
			}
			return true;
		}

		internal class PrimeNumberAccesor
		{
			private int index = 0;

			public long GetNextPrimeNumber()
			{
				EnsureIndexExists();
				long num = primeNumbers[index];
				index++;
				return num;
			}

			public long PeekNextPrimeNumber()
			{
				EnsureIndexExists();
				return primeNumbers[index];
			}

			public void Reset()
			{
				index = 0;
			}

			private void EnsureIndexExists()
			{
				if (primeNumbers.Count == index)
				{
					FindNextPrimeNumber();
				}
			}
		}
	}
}
