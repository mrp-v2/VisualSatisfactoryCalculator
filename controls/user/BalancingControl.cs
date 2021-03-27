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

		public bool Locked
		{
			get
			{
				return LockBox.Checked;
			}
			private set
			{
				LockBox.Checked = value;
			}
		}

		public BalancingControl(BalancingPrompt balancingPrompt, Step step, bool isOutput)
		{
			InitializeComponent();
			this.balancingPrompt = balancingPrompt;
			Step = step;
			IsOutput = isOutput;
		}

		private void Numeric_ValueChanged(object sender, EventArgs e)
		{

		}

		private void LockBox_CheckedChanged(object sender, EventArgs e)
		{

		}
	}
}
