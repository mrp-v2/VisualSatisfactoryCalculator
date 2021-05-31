using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.code.Numbers
{
	internal static class PrimeNumberHandler
	{
		// The maximum square root of a long fits into a uint, and we know the number of prime numbers
		private static readonly uint[] primeNumbers = new uint[203280221];
		private static bool primeNumbersFound = false;

		private static void FindPrimeNumbers()
		{
			uint arraySize = uint.MaxValue / 4;
			uint b0a = 2, b0b = 3, b1a = arraySize, b1b = arraySize + 1, b2a = b1a + arraySize, b2b = b1b + arraySize, b3a = b2a + arraySize, b3b = b2b + arraySize;
			// b0b | b1a
			BitArray seive1 = new BitArray((int)(b1a - b0a));
			// b1b | b2a
			BitArray seive2 = new BitArray((int)(b2a - b1a));
			// b2b | b3a
			BitArray seive3 = new BitArray((int)(b3a - b2a));
			// b3b | uint.MaxValue
			BitArray seive4 = new BitArray((int)(uint.MaxValue - b3a));
			bool isPrime(uint number)
			{
				if (number <= b1a)
				{
					return !seive1.Get((int)(number - b0b));
				}
				else if (number <= b2a)
				{
					return !seive2.Get((int)(number - b1b));
				}
				else if (number <= b3a)
				{
					return !seive3.Get((int)(number - b2b));
				}
				else
				{
					return !seive4.Get((int)(number - b3b));
				}
			}
			void setComposite(uint number)
			{
				if (number <= b1a)
				{
					seive1.Set((int)(number - b0b), true);
				}
				else if (number <= b2a)
				{
					seive2.Set((int)(number - b1b), true);
				}
				else if (number <= b3a)
				{
					seive3.Set((int)(number - b2b), true);
				}
				else
				{
					seive4.Set((int)(number - b3b), true);
				}
			}
			// 2 is the starting prime number
			int primeArrayIndex = 0;
			primeNumbers[primeArrayIndex++] = 2;
			for (uint compositeNumber = 4; compositeNumber >= 4; compositeNumber += 2)
			{
				setComposite(compositeNumber);
			}
			uint currentPrime = 1;
			while (currentPrime <= uint.MaxValue)
			{
				bool shouldStop = true;
				for (uint primeTest = currentPrime + 2; primeTest < uint.MaxValue; primeTest += 2)
				{
					if (isPrime(primeTest))
					{
						primeNumbers[primeArrayIndex++] = primeTest;
						currentPrime = primeTest;
						if (currentPrime < uint.MaxValue / 3)
						{
							uint twiceCurrentPrime = currentPrime + currentPrime;
							uint thriceCurrentPrive = twiceCurrentPrime + currentPrime;
							for (uint compositeNumber = thriceCurrentPrive; compositeNumber >= thriceCurrentPrive; compositeNumber += twiceCurrentPrime)
							{
								setComposite(compositeNumber);
							}
						}
						shouldStop = false;
						break;
					}
				}
				if (shouldStop)
				{
					break;
				}
			}
			primeNumbersFound = true;
			// Found primes in bounds: 54,400,028 | 50,697,536 | 49,472,952 | 48,709,705
		}

		internal class PrimeNumberAccesor
		{
			private int index = 0;

			public PrimeNumberAccesor()
			{
				if (!primeNumbersFound)
				{
					FindPrimeNumbers();
				}
			}

			public uint GetNextPrimeNumber()
			{
				EnsureIndexExists();
				uint num = primeNumbers[index];
				index++;
				return num;
			}

			public void Reset()
			{
				index = 0;
			}

			private void EnsureIndexExists()
			{
				if (primeNumbers.Length == index)
				{
					throw new InvalidOperationException("Ran out of prime numbers");
				}
			}
		}
	}
}
