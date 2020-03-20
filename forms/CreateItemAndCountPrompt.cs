using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisualSatisfactoryCalculator.code;

namespace VisualSatisfactoryCalculator.forms
{
	public partial class CreateItemAndCountPrompt : Form
	{
		private readonly IReceives<ItemCount> parentForm;
		private readonly string purpose;

		public CreateItemAndCountPrompt(IReceives<ItemCount> parentForm, string purpose = null)
		{
			InitializeComponent();
			this.parentForm = parentForm;
			this.purpose = purpose;
			ItemNameCombo.BeginUpdate();
			foreach (string str in SuggestionsController.SC.GetItems())
			{
				ItemNameCombo.Items.Add(str);
			}
			ItemNameCombo.EndUpdate();
		}

		private void OkButton_Click(object sender, EventArgs e)
		{
			parentForm.SendObject(GetItemCount(), purpose);
			Close();
		}

		private void CancelButton_Click(object sender, EventArgs e)
		{
			Close();
		}

		private ItemCount GetItemCount()
		{
			return new ItemCount(ItemNameCombo.Text, (int)ItemCountNumeric.Value);
		}
	}
}
