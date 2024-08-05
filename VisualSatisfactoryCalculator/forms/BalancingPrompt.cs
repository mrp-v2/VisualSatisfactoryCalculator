﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;

using VisualSatisfactoryCalculator.code.Numbers;
using VisualSatisfactoryCalculator.code.Production;
using VisualSatisfactoryCalculator.code.Utility;
using VisualSatisfactoryCalculator.controls.user;
using VisualSatisfactoryCalculator.model.production;

namespace VisualSatisfactoryCalculator.forms
{
	public partial class BalancingPrompt<ItemType> : Form where ItemType : AbstractItem
	{
		public readonly Connection Connection;
		private readonly Dictionary<BalancingControl<ItemType>, HashSet<BalancingControl<ItemType>>> relatedControlsMap = new Dictionary<BalancingControl<ItemType>, HashSet<BalancingControl<ItemType>>>();
		public readonly Dictionary<Step, BalancingControl<ItemType>> ConsumingStepMap = new Dictionary<Step, BalancingControl<ItemType>>();
		public readonly Dictionary<Step, BalancingControl<ItemType>> ProducingStepMap = new Dictionary<Step, BalancingControl<ItemType>>();
		public readonly CachedValue<RationalNumber> outputRate;
		public readonly CachedValue<RationalNumber> inputRate;
		private readonly RationalNumber totalRate;
		public readonly Dictionary<Step, decimal> finalRates = new Dictionary<Step, decimal>();

		public BalancingPrompt(Connection connection)
		{
			InitializeComponent();
			Connection = connection;
			foreach (Step step in connection.GetProducerSteps())
			{
				BalancingControl<ItemType> bc = new BalancingControl<ItemType>(this, step, false);
				InFlowPanel.Controls.Add(bc);
				relatedControlsMap.Add(bc, new HashSet<BalancingControl<ItemType>>());
				ProducingStepMap.Add(step, bc);
			}
			foreach (Step step in connection.GetConsumerSteps())
			{
				BalancingControl<ItemType> bc = new BalancingControl<ItemType>(this, step, true);
				OutFlowPanel.Controls.Add(bc);
				relatedControlsMap.Add(bc, new HashSet<BalancingControl<ItemType>>());
				ConsumingStepMap.Add(step, bc);
			}
			foreach (BalancingControl<ItemType> bc in relatedControlsMap.Keys)
			{
				foreach (HashSet<Step> stepGroup in connection.RelevantConnectedStepGroups.Get())
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
			outputRate = new CachedValue<RationalNumber>(() =>
			{
				RationalNumber total = 0;
				foreach (BalancingControl<ItemType> bc in ConsumingStepMap.Values)
				{
					total += bc.Rate;
				}
				return total;
			});
			inputRate = new CachedValue<RationalNumber>(() =>
			{
				RationalNumber total = 0;
				foreach (BalancingControl<ItemType> bc in ProducingStepMap.Values)
				{
					total += bc.Rate;
				}
				return total;
			});
			totalRate = outputRate.Get();
		}

		private void DoneButton_Click(object sender, EventArgs e)
		{
			Connection.UpdateRates(this);
			DialogResult = DialogResult.OK;
			Close();
		}

		private void CancelButton_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

		public void NumericValueChanged(BalancingControl<ItemType> piece)
		{
			RationalNumber multiplier = piece.Rate / piece.OriginalRate;
			foreach (BalancingControl<ItemType> bc in relatedControlsMap[piece])
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

		private void BalanceRates(BalancingControl<ItemType> changed)
		{
			RationalNumber outputTotal = totalRate, inputTotal = totalRate;
			List<HashSet<Step>> changableGroups = new List<HashSet<Step>>();
			List<HashSet<Step>> unchangableGroups = new List<HashSet<Step>>();
			Dictionary<HashSet<Step>, (RationalNumber, RationalNumber)> groupRates = new Dictionary<HashSet<Step>, (RationalNumber, RationalNumber)>();
			foreach (HashSet<Step> group in Connection.RelevantConnectedStepGroups.Get())
			{
				foreach (Step step in group)
				{
					if (ConsumingStepMap.ContainsKey(step))
					{
						BalancingControl<ItemType> bc = ConsumingStepMap[step];
						if (bc.Locked || bc == changed)
						{
							goto Unchangeable;
						}
					}
					if (ProducingStepMap.ContainsKey(step))
					{
						BalancingControl<ItemType> bc = ProducingStepMap[step];
						if (bc.Locked || bc == changed)
						{
							goto Unchangeable;
						}
					}
				}
				changableGroups.Add(group);
				groupRates.Add(group, (0, 0));
				goto Continue;
			Unchangeable:
				unchangableGroups.Add(group);
			Continue:
				continue;
			}
			foreach (HashSet<Step> group in unchangableGroups)
			{
				foreach (Step step in group)
				{
					if (ConsumingStepMap.ContainsKey(step))
					{
						BalancingControl<ItemType> bc = ConsumingStepMap[step];
						outputTotal -= bc.Rate;
					}
					if (ProducingStepMap.ContainsKey(step))
					{
						BalancingControl<ItemType> bc = ProducingStepMap[step];
						inputTotal -= bc.Rate;
					}
				}
			}
			foreach (HashSet<Step> group in changableGroups)
			{
				foreach (Step step in group)
				{
					if (ConsumingStepMap.ContainsKey(step))
					{
						BalancingControl<ItemType> bc = ConsumingStepMap[step];
						(RationalNumber, RationalNumber) tuple = groupRates[group];
						groupRates[group] = (tuple.Item1, tuple.Item2 + bc.Rate);
					}
					if (ProducingStepMap.ContainsKey(step))
					{
						BalancingControl<ItemType> bc = ProducingStepMap[step];
						(RationalNumber, RationalNumber) tuple = groupRates[group];
						groupRates[group] = (tuple.Item1 + bc.Rate, tuple.Item2);
					}
				}
			}
			(RationalNumber, RationalNumber, RationalNumber) multipliers = Util.BalanceRates(groupRates, inputTotal, outputTotal);
			foreach (HashSet<Step> group in changableGroups)
			{
				foreach (Step step in group)
				{
					(RationalNumber, RationalNumber) tuple = groupRates[group];
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
	}
}
