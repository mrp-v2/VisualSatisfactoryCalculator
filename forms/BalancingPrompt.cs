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
using VisualSatisfactoryCalculator.controls.user;

namespace VisualSatisfactoryCalculator.forms
{
	public partial class BalancingPrompt : Form
	{
		private readonly Connection connection;

		public BalancingPrompt(Connection connection)
		{
			InitializeComponent();
		}

		private void DoneButton_Click(object sender, EventArgs e)
		{

		}

		private void CancelButton_Click(object sender, EventArgs e)
		{

		}

		public void LockChanged(BalancingControl piece)
		{

		}
	}
}
