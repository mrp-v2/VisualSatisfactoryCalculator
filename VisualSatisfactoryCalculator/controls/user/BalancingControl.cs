using System;
using System.Windows.Forms;

using VisualSatisfactoryCalculator.code.Numbers;
using VisualSatisfactoryCalculator.code.Production;
using VisualSatisfactoryCalculator.forms;
using VisualSatisfactoryCalculator.model.production;

namespace VisualSatisfactoryCalculator.controls.user
{
	public partial class BalancingControl<ItemType> : UserControl where ItemType : AbstractItem
	{
		private readonly BalancingPrompt balancingPrompt;
		public readonly AbstractStep<ItemType> Step;
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

		public BalancingControl(BalancingPrompt balancingPrompt, AbstractStep<ItemType> step, bool isOutput, bool locked)
		{
			InitializeComponent();
			this.balancingPrompt = balancingPrompt;
			Step = step;
			IsOutput = isOutput;
			OriginalRate = step.GetRate(balancingPrompt.Connection.ItemID, !isOutput);
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
	}
}
