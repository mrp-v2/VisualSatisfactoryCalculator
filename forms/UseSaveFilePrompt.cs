using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisualSatisfactoryCalculator.forms
{
	public partial class UseSaveFilePrompt : Form
	{
		public UseSaveFilePrompt()
		{
			InitializeComponent();
		}

		private void UseFileButton_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Yes;
			Close();
		}

		private void DontUseFileButton_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.No;
			Close();
		}
	}
}
