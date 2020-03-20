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
	public partial class SelectRecipePrompt : Form
	{
		private readonly IReceives<Recipe> parent;
		private readonly string purpose;

		public SelectRecipePrompt(List<Recipe> options, IReceives<Recipe> parent, string purpose)
		{
			InitializeComponent();
			this.parent = parent;
			this.purpose = purpose;
			foreach (Recipe rec in options)
			{
				RecipesList.Items.Add(rec);
			}
		}

		private void YesButton_Click(object sender, EventArgs e)
		{
			if (RecipesList.SelectedItem is Recipe)
			{
				parent.SendObject(RecipesList.SelectedItem as Recipe, purpose);
				Close();
			}
		}

		private void NoButton_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
