using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using VisualSatisfactoryCalculator.code.Production;
using VisualSatisfactoryCalculator.forms;

namespace VisualSatisfactoryCalculator.controls.user
{
	public partial class BalancingControl : UserControl
	{
		private readonly BalancingPrompt balancingPrompt;
		public readonly Step Step;
		public readonly bool IsOutput;
		public readonly decimal OriginalRate;

		public decimal Rate
		{
			get
			{
				return Numeric.Value;
			}
			set
			{
				Enabled = false;
				Numeric.Value = value;
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
			Rate = OriginalRate;
		}

		private void Numeric_ValueChanged(object sender, EventArgs e)
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
