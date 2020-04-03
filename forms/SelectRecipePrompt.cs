using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VisualSatisfactoryCalculator.code;

namespace VisualSatisfactoryCalculator.forms
{
	public partial class SelectRecipePrompt : Form
	{
		private readonly IReceives<JSONRecipe> parent;
		private readonly string purpose;

		public SelectRecipePrompt(List<JSONRecipe> options, IReceives<JSONRecipe> parent, string purpose)
		{
			InitializeComponent();
			this.parent = parent;
			this.purpose = purpose;
			foreach (JSONRecipe rec in options)
			{
				RecipesList.Items.Add(rec);
			}
		}

		private void YesButton_Click(object sender, EventArgs e)
		{
			if (RecipesList.SelectedItem is JSONRecipe)
			{
				parent.SendObject(RecipesList.SelectedItem as JSONRecipe, purpose);
				Close();
			}
		}

		private void NoButton_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
