using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VisualSatisfactoryCalculator.code.Extensions;
using VisualSatisfactoryCalculator.code.Numbers;

namespace VisualSatisfactoryCalculator.code.Utility
{
	class Util
	{
		// example single item lists:
		// (BlueprintGeneratedClass'\"/Game/FactoryGame/Resource/RawResources/CrudeOil/Desc_LiquidOil.Desc_LiquidOil_C\"')
		// example 2 item list:
		// (/Game/FactoryGame/Resource/Parts/NuclearFuelRod/Desc_NuclearFuelRod.Desc_NuclearFuelRod_C,/Game/FactoryGame/Resource/Parts/PlutoniumFuelRods/Desc_PlutoniumFuelRod.Desc_PlutoniumFuelRod_C)
		// example 3 item list:
		// (/Game/FactoryGame/Resource/RawResources/Coal/Desc_Coal.Desc_Coal_C,/Game/FactoryGame/Resource/Parts/CompactedCoal/Desc_CompactedCoal.Desc_CompactedCoal_C,/Game/FactoryGame/Resource/Parts/PetroleumCoke/Desc_PetroleumCoke.Desc_PetroleumCoke_C)
		public static string[] ParseUIDList(string itemList)
		{
			string[] items = itemList.TrimStart('(').TrimEnd(')').Replace("'", "").Replace("\"", "").Split(',');
			for (int i = 0; i < items.Length; i++)
			{
				string item = items[i];
				items[i] = item.Substring(item.IndexOf('.') + 1);
			}
			return items;
		}

		public static bool TryBalanceRates<T>(Dictionary<T, (RationalNumber, RationalNumber)> rates, RationalNumber inputTotal, RationalNumber outputTotal, out (RationalNumber, RationalNumber, RationalNumber) multipliers)
		{
			try
			{
				multipliers = BalanceRates(rates, inputTotal, outputTotal);
				return true;
			}
			catch (BalancingException)
			{
				multipliers = (1, 1, 1);
				return false;
			}
		}

		/// <summary>
		/// Calculates multipliers that can be used to balance rates
		/// </summary>
		/// <typeparam name="T">The type of the keys in <paramref name="rates"/></typeparam>
		/// <param name="rates">A map indicating the input and output rates of each <typeparamref name="T"/></param>
		/// <param name="inputTotal">The desired amount of output</param>
		/// <param name="outputTotal">The desired amount of input</param>
		/// <returns>A Tuple that contains the (input multipluer, output multiplier, paired multiplier)</returns>
		public static (RationalNumber, RationalNumber, RationalNumber) BalanceRates<T>(Dictionary<T, (RationalNumber, RationalNumber)> rates, RationalNumber inputTotal, RationalNumber outputTotal)
		{
			RationalNumber isolatedInputRate = 0, isolatedOutputRate = 0, pairedInputRate = 0, pairedOutputRate = 0;
			bool anyPairedRates = false;
			foreach ((RationalNumber, RationalNumber) tuple in rates.Values)
			{
				if (tuple.Item1 == 0 || tuple.Item2 == 0)
				{
					isolatedInputRate += tuple.Item1;
					isolatedOutputRate += tuple.Item2;
				}
				else
				{
					pairedInputRate += tuple.Item1;
					pairedOutputRate += tuple.Item2;
					if (!anyPairedRates)
					{
						anyPairedRates = true;
					}
				}
			}
			if (anyPairedRates)
			{
				if (isolatedInputRate == 0 && isolatedOutputRate == 0)
				{
					if (pairedInputRate / pairedOutputRate != inputTotal / outputTotal)
					{
						throw new BalancingException("Cannot balance only paired rates that don't have the same ratio as the desired input and output");
					}
					return (1, 1, inputTotal / pairedInputRate);
				}
				else if (isolatedInputRate == 0)
				{
					RationalNumber pairedMultiplier = inputTotal / pairedInputRate;
					RationalNumber outputMultiplier = (outputTotal - (pairedOutputRate * pairedMultiplier)) / isolatedOutputRate;
					return (1, outputMultiplier, pairedMultiplier);
				}
				else if (isolatedOutputRate == 0)
				{
					RationalNumber pairedMultiplier = outputTotal / pairedOutputRate;
					RationalNumber inputMultiplier = (inputTotal - (pairedInputRate * pairedMultiplier)) / isolatedInputRate;
					return (inputMultiplier, 1, pairedMultiplier);
				}
				else
				{
					RationalNumber a = 3 * pairedInputRate * pairedOutputRate, b = -2 * ((pairedInputRate * outputTotal) + (inputTotal * pairedOutputRate)), c = inputTotal * outputTotal;
					RationalNumber discriminate = (b * b) - (4 * a * c);
					RationalNumber discriminateSqrt = discriminate.Sqrt();
					RationalNumber pairedMultiplierA = (-b + discriminateSqrt) / (2 * a), pairedMultiplierB = discriminate > 0 ? (-b - discriminateSqrt) / (2 * a) : pairedMultiplierA;
					if (!pairedMultiplierA.IsPositive && !pairedMultiplierB.IsPositive)
					{
						throw new BalancingException("Unable to find a valid multiplier");
					}
					else if (!pairedMultiplierB.IsPositive)
					{
						pairedMultiplierB = pairedMultiplierA;
					}
					else if (!pairedMultiplierA.IsPositive)
					{
						pairedMultiplierA = pairedMultiplierB;
					}
					else if (pairedMultiplierA > pairedMultiplierB)
					{
						RationalNumber temp = pairedMultiplierA;
						pairedMultiplierA = pairedMultiplierB;
						pairedMultiplierB = temp;
					}
					bool isMultiplierProductIncreasingAt(RationalNumber point)
					{
						return (3 * point * point * pairedInputRate * pairedOutputRate) - (2 * point * ((pairedInputRate * outputTotal) + (inputTotal * pairedOutputRate))) + (inputTotal * outputTotal) > 0;
					}
					RationalNumber pairedMultiplier;
					if (pairedMultiplierA == pairedMultiplierB)
					{
						pairedMultiplier = pairedMultiplierA;
					}
					else
					{
						bool increasingBeforeA = isMultiplierProductIncreasingAt(pairedMultiplierA - 1);
						if (!increasingBeforeA)
						{
							throw new BalancingException("Cannot maximize products");
						}
						bool increasingBetweenAAndB = isMultiplierProductIncreasingAt(pairedMultiplierA + ((pairedMultiplierB - pairedMultiplierA) / 2));
						bool increasingAfterB = isMultiplierProductIncreasingAt(pairedMultiplierB + 1);
						if (increasingAfterB)
						{
							throw new BalancingException("Cannot maximize products");
						}
						else if (!increasingAfterB && increasingBetweenAAndB)
						{
							pairedMultiplier = pairedMultiplierB;
						}
						else if (!increasingBetweenAAndB && !increasingAfterB)
						{
							pairedMultiplier = pairedMultiplierA;
						}
						else
						{
							throw new BalancingException("Cannot maximize products");
						}
					}
					RationalNumber inputMultiplier = isolatedInputRate != 0 ? (inputTotal - (pairedMultiplier * pairedInputRate)) / isolatedInputRate : 1;
					RationalNumber outputMultiplier = isolatedOutputRate != 0 ? (outputTotal - (pairedMultiplier * pairedOutputRate)) / isolatedOutputRate : 1;
					return (inputMultiplier, outputMultiplier, pairedMultiplier);
				}
			}
			else
			{
				RationalNumber inputMultiplier = isolatedInputRate == 0 ? 1 : inputTotal / isolatedInputRate;
				RationalNumber outputMultiplier = isolatedOutputRate == 0 ? 1 : outputTotal / isolatedOutputRate;
				return (inputMultiplier, outputMultiplier, 1);
			}
		}

		public class BalancingException : ArgumentException
		{
			public BalancingException(string message) : base(message)
			{
			}
		}
	}
}
