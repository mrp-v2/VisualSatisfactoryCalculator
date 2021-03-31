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
			decimal outputTotal = totalRate, inputTotal = totalRate;
			List<HashSet<Step>> changableGroups = new List<HashSet<Step>>();
			List<HashSet<Step>> unchangableGroups = new List<HashSet<Step>>();
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
							goto Unchangeable;
						}
					}
					if (ProducingStepMap.ContainsKey(step))
					{
						BalancingControl bc = ProducingStepMap[step];
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
						BalancingControl bc = ConsumingStepMap[step];
						outputTotal -= bc.Rate;
					}
					if (ProducingStepMap.ContainsKey(step))
					{
						BalancingControl bc = ProducingStepMap[step];
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
						BalancingControl bc = ConsumingStepMap[step];
						(decimal, decimal) tuple = groupRates[group];
						groupRates[group] = (tuple.Item1, tuple.Item2 + bc.Rate);
					}
					if (ProducingStepMap.ContainsKey(step))
					{
						BalancingControl bc = ProducingStepMap[step];
						(decimal, decimal) tuple = groupRates[group];
						groupRates[group] = (tuple.Item1 + bc.Rate, tuple.Item2);
					}
				}
			}
			(decimal, decimal, decimal) multipliers = Util.BalanceRates(groupRates, inputTotal, outputTotal);
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
	}
}
