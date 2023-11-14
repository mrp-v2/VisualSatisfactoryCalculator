using System;
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
