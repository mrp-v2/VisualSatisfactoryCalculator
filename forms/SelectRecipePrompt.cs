using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VisualSatisfactoryCalculator.code.Interfaces;
using VisualSatisfactoryCalculator.code.JSONClasses;

namespace VisualSatisfactoryCalculator.forms
{
	public partial class SelectRecipePrompt : Form
	{
		private readonly IReceives<IRecipe> parent;
		private readonly string purpose;

		private readonly List<IRecipe> originalList;

		public SelectRecipePrompt(List<IRecipe> options, IReceives<IRecipe> parent, string purpose)
		{
			InitializeComponent();
			this.parent = parent;
			this.purpose = purpose;
			originalList = options;
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

		private void FilterBox_TextChanged(object sender, EventArgs e)
		{
			RecipesList.BeginUpdate();
			RecipesList.Items.Clear();
			foreach (JSONRecipe recipe in originalList)
			{
				if (recipe.ToString().ToLower().Contains(FilterBox.Text.ToLower()))
				{
					RecipesList.Items.Add(recipe);
				}
			}
			RecipesList.EndUpdate();
		}
	}
}
