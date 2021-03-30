using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using VisualSatisfactoryCalculator.code.Extensions;
using VisualSatisfactoryCalculator.code.Production;
using VisualSatisfactoryCalculator.code.Utility;
using VisualSatisfactoryCalculator.controls.user;

namespace VisualSatisfactoryCalculator.forms
{
	public partial class BalancingPrompt : Form
	{
		public readonly Connection Connection;
		private readonly Dictionary<BalancingControl, HashSet<BalancingControl>> relatedControlsMap = new Dictionary<BalancingControl, HashSet<BalancingControl>>();
		private readonly Dictionary<Step, BalancingControl> ConsumingStepMap = new Dictionary<Step, BalancingControl>();
		private readonly Dictionary<Step, BalancingControl> ProducingStepMap = new Dictionary<Step, BalancingControl>();
		public readonly CachedValue<decimal> outputRate;
		public readonly CachedValue<decimal> inputRate;
		private readonly decimal totalRate;
		public readonly Dictionary<Step, decimal> finalRates = new Dictionary<Step, decimal>();

		public BalancingPrompt(Connection connection)
		{
			InitializeComponent();
			Connection = connection;
			List<HashSet<Step>> stepGroups = connection.RelevantConnectedStepGroups.Get();
			foreach (Step step in connection.GetProducerSteps())
			{
				BalancingControl bc = new BalancingControl(this, step, false);
				InFlowPanel.Controls.Add(bc);
				relatedControlsMap.Add(bc, new HashSet<BalancingControl>());
				ProducingStepMap.Add(step, bc);
			}
			foreach (Step step in connection.GetConsumerSteps())
			{
				BalancingControl bc = new BalancingControl(this, step, true);
				OutFlowPanel.Controls.Add(bc);
				relatedControlsMap.Add(bc, new HashSet<BalancingControl>());
				ConsumingStepMap.Add(step, bc);
			}
			foreach (BalancingControl bc in relatedControlsMap.Keys)
			{
				foreach (HashSet<Step> stepGroup in stepGroups)
				{
					if (stepGroup.Contains(bc.Step))
					{
						foreach (Step step in stepGroup)
						{
							if (step != bc.Step)
							{
								relatedControlsMap[bc].Add(ConsumingStepMap.ContainsKey(step) ? ConsumingStepMap[step] : ProducingStepMap[step]);
							}
						}
					}
				}
			}
			outputRate = new CachedValue<decimal>(() =>
			{
				decimal total = 0;
				foreach (BalancingControl bc in ConsumingStepMap.Values)
				{
					total += bc.Rate;
				}
				return total;
			});
			inputRate = new CachedValue<decimal>(() =>
			{
				decimal total = 0;
				foreach (BalancingControl bc in ProducingStepMap.Values)
				{
					total += bc.Rate;
				}
				return total;
			});
			totalRate = outputRate.Get();
		}

		private void DoneButton_Click(object sender, EventArgs e)
		{
			// TODO
			DialogResult = DialogResult.OK;
			Close();
		}

		private void CancelButton_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

		public void LockChanged(BalancingControl piece)
		{
			foreach (BalancingControl bc in relatedControlsMap[piece])
			{
				bc.Locked = piece.Locked;
			}
		}

		public void NumericValueChanged(BalancingControl piece)
		{
			decimal multiplier = piece.Rate / piece.OriginalRate;
			foreach (BalancingControl bc in relatedControlsMap[piece])
			{
				bc.Rate = bc.OriginalRate * multiplier;
			}
			inputRate.Invalidate();
			outputRate.Invalidate();
			if (inputRate.Get() != outputRate.Get() || outputRate.Get() != totalRate)
			{
				BalanceRates(piece);
			}
		}

		private void BalanceRates(BalancingControl changed)
		{
			decimal outputTotal = 0, inputTotal = 0;
			List<HashSet<Step>> changableGroups = new List<HashSet<Step>>();
			Dictionary<HashSet<Step>, (decimal, decimal)> groupRates = new Dictionary<HashSet<Step>, (decimal, decimal)>();
			foreach (HashSet<Step> group in Connection.RelevantConnectedStepGroups.Get())
			{
				foreach (Step step in group)
				{
					if (ConsumingStepMap.ContainsKey(step))
					{
						BalancingControl bc = ConsumingStepMap[step];
						if (bc.Locked || bc == changed)
						{
							goto Continue;
						}
					}
					if (ProducingStepMap.ContainsKey(step))
					{
						BalancingControl bc = ProducingStepMap[step];
						if (bc.Locked || bc == changed)
						{
							goto Continue;
						}
					}
				}
				changableGroups.Add(group);
				groupRates.Add(group, (0, 0));
			Continue:
				continue;
			}
			foreach (HashSet<Step> group in changableGroups)
			{
				foreach (Step step in group)
				{
					if (ConsumingStepMap.ContainsKey(step))
					{
						BalancingControl bc = ConsumingStepMap[step];
						outputTotal += bc.Rate;
						(decimal, decimal) tuple = groupRates[group];
						groupRates[group] = (tuple.Item1, tuple.Item2 + bc.Rate);
					}
					if (ProducingStepMap.ContainsKey(step))
					{
						BalancingControl bc = ProducingStepMap[step];
						inputTotal += bc.Rate;
						(decimal, decimal) tuple = groupRates[group];
						groupRates[group] = (tuple.Item1 + bc.Rate, tuple.Item2);
					}
				}
			}
			(decimal, decimal, decimal) multipliers = BalanceRates(groupRates, inputTotal, outputTotal);
			foreach (HashSet<Step> group in changableGroups)
			{
				foreach (Step step in group)
				{
					(decimal, decimal) tuple = groupRates[group];
					if (tuple.Item1 == 0 || tuple.Item2 == 0)
					{
						if (ConsumingStepMap.ContainsKey(step))
						{
							ConsumingStepMap[step].Rate *= multipliers.Item2;
						}
						else
						{
							ProducingStepMap[step].Rate *= multipliers.Item1;
						}
					}
					else
					{
						if (ConsumingStepMap.ContainsKey(step))
						{
							ConsumingStepMap[step].Rate *= multipliers.Item3;
						}
						if (ProducingStepMap.ContainsKey(step))
						{
							ProducingStepMap[step].Rate *= multipliers.Item3;
						}
					}
				}
			}
		}

		public static (decimal, decimal, decimal) BalanceRates<T>(Dictionary<T, (decimal, decimal)> rates, decimal inputTotal, decimal outputTotal)
		{
			decimal isolatedInputRate = 0, isolatedOutputRate = 0, pairedInputRate = 0, pairedOutputRate = 0;
			foreach ((decimal, decimal) tuple in rates.Values)
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
				}
			}
			decimal a = 3 * pairedInputRate * pairedOutputRate, b = -2 * ((pairedInputRate * outputTotal) + (inputTotal * pairedOutputRate)), c = inputTotal * outputTotal;
			decimal discriminate = (b * b) - (4 * a * c);
			decimal discriminateSqrt = discriminate.Sqrt();
			decimal pairedMultiplierA = (-b + discriminateSqrt) / (2 * a), pairedMultiplierB = discriminate > 0 ? (-b - discriminateSqrt) / (2 * a) : pairedMultiplierA;
			if (pairedMultiplierA < 0 && pairedMultiplierB < 0)
			{
				throw new ArgumentException("Unable to find a valid multiplier");
			}
			else if (pairedMultiplierB < 0)
			{
				pairedMultiplierB = pairedMultiplierA;
			}
			else if (pairedMultiplierA < 0)
			{
				pairedMultiplierA = pairedMultiplierB;
			}
			else if (pairedMultiplierA > pairedMultiplierB)
			{
				decimal temp = pairedMultiplierA;
				pairedMultiplierA = pairedMultiplierB;
				pairedMultiplierB = temp;
			}
			bool isMultiplierProductIncreasingAt(decimal point)
			{
				return (3 * point * point * pairedInputRate * pairedOutputRate) - (2 * point * ((pairedInputRate * outputTotal) + (inputTotal * pairedOutputRate))) + (inputTotal * outputTotal) > 0;
			}
			decimal pairedMultiplier;
			if (pairedMultiplierA == pairedMultiplierB)
			{
				pairedMultiplier = pairedMultiplierA;
			}
			else
			{
				bool increasingBeforeA = isMultiplierProductIncreasingAt(pairedMultiplierA - 1);
				if (!increasingBeforeA)
				{
					throw new ArgumentException("Cannot maximize product");
				}
				bool increasingBetweenAAndB = isMultiplierProductIncreasingAt(pairedMultiplierA + ((pairedMultiplierB - pairedMultiplierA) / 2));
				bool increasingAfterB = isMultiplierProductIncreasingAt(pairedMultiplierB + 1);
				if (increasingAfterB)
				{
					throw new ArgumentException("Cannot maximize products");
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
					throw new ArgumentException("Cannot maximize products");
				}
			}
			decimal inputMultiplier = (inputTotal - (pairedMultiplier * pairedInputRate)) / isolatedInputRate;
			decimal outputMultiplier = (outputTotal - (pairedMultiplier * pairedOutputRate)) / isolatedOutputRate;
			return (inputMultiplier, outputMultiplier, pairedMultiplier);
		}
	}
}
