using System;
using System.Windows.Forms;

using VisualSatisfactoryCalculator.code.Numbers;
using VisualSatisfactoryCalculator.code.Production;
using VisualSatisfactoryCalculator.forms;

namespace VisualSatisfactoryCalculator.controls.user
{
	public partial class BalancingControl : UserControl
	{
		private readonly BalancingPrompt balancingPrompt;
		public readonly Step Step;
		public readonly bool IsOutput;
		public readonly RationalNumber OriginalRate;

		public RationalNumber Rate
		{
			get
			{
				return NumberControl.GetNumber();
			}
			set
			{
				Enabled = false;
				NumberControl.SetNumber(value);
				Enabled = true;
			}
		}

		public bool Locked
		{
			get
			{
				return LockBox.Checked;
			}
			set
			{
				Enabled = false;
				LockBox.Checked = value;
				Enabled = true;
			}
		}

		public BalancingControl(BalancingPrompt balancingPrompt, Step step, bool isOutput)
		{
			InitializeComponent();
			this.balancingPrompt = balancingPrompt;
			Step = step;
			IsOutput = isOutput;
			OriginalRate = step.GetItemRate(balancingPrompt.Connection.ItemID, !isOutput);
			NumberControl.AddNumberChangedListener(ValueChanged);
			Rate = OriginalRate;
		}

		private void ValueChanged()
		{
			if (Enabled)
			{
				balancingPrompt.NumericValueChanged(this);
			}
		}

		private void LockBox_CheckedChanged(object sender, EventArgs e)
		{
			if (Enabled)
			{
				balancingPrompt.LockChanged(this);
			}
		}
	}
}
