using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.code.Numbers
{
	internal static class RationalNumberUtil
	{
		public static long GreatestCommonFactor(long a, long b)
		{
			PrimeNumberHandler.PrimeNumberAccesor primeNumberAccesor = new PrimeNumberHandler.PrimeNumberAccesor();
			long currentPrime = primeNumberAccesor.GetNextPrimeNumber();
			long gcf = 1;
			long smallerSQRT = (long)Math.Min(Math.Sqrt(a), Math.Sqrt(b)) + 1;
			while (currentPrime <= smallerSQRT)
			{
				if (a % currentPrime == 0 && b % currentPrime == 0)
				{
					gcf *= currentPrime;
					a /= currentPrime;
					b /= currentPrime;
					primeNumberAccesor.Reset();
				}
				currentPrime = primeNumberAccesor.GetNextPrimeNumber();
			}
			return gcf;
		}
	}
}
