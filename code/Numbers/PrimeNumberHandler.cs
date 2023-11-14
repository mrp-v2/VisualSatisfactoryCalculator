using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisualSatisfactoryCalculator.code.Numbers
{
	internal static class PrimeNumberHandler
	{
		// start with all the prime numbers less than 100
		private static readonly List<int> primeNumbers = new List<int>() { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97 };
		private static readonly int PrimeFindingWarningLevel = (int)Math.Round(int.MaxValue * 0.99f);

		private static void FindNextPrime()
		{
			// start testing just above the last found prime
			int test = primeNumbers[primeNumbers.Count - 1] + 2;
			if (test > PrimeFindingWarningLevel)
			{
				Console.WriteLine("WARNING: Prime number finding is approaching the integer limit!!");
			}
		MainLoop:
			foreach (int prime in primeNumbers)
			{
				if (test % prime == 0)
				{
					// if our test fails, increment by 2 and start over
					test += 2;
					goto MainLoop;
				}
			}
			primeNumbers.Add(test);
			return;
		}

		internal class PrimeNumberAccesor
		{
			private int index;

			public PrimeNumberAccesor()
			{
				index = 0;
			}

			public int PeekNextPrimeNumber()
			{
				EnsureIndexExists();
				return primeNumbers[index];
			}

			public int GetNextPrimeNumber()
			{
				EnsureIndexExists();
				return primeNumbers[index++];
			}

			public void Reset()
			{
				index = 0;
			}

			private void EnsureIndexExists()
			{
				if (primeNumbers.Count == index)
				{
					FindNextPrime();
				}
			}
		}

		public static List<int> PrimeFactors(int number)
		{
			if (number == 1)
			{
				Console.WriteLine("WARNING: Tried to prime factor 1! Something might be amiss...");
				return new List<int>();
			}
			if (number < 1)
			{
				throw new ArgumentException("Can't find the prime factors of a number less than 1!");
			}
			List<int> primeFactors = new List<int>();
			PrimeNumberAccesor primeSupplier = new PrimeNumberAccesor();
			while (number > 1)
			{
				int test = primeSupplier.GetNextPrimeNumber();
				while (number % test != 0)
				{
					test = primeSupplier.GetNextPrimeNumber();
				}
				primeSupplier.Reset();
				primeFactors.Add(test);
				number /= test;
			}
			return primeFactors;
		}
	}
}
