using System;
using System.Collections.Generic;
using System.Windows.Forms;

using VisualSatisfactoryCalculator.code.Interfaces;

namespace VisualSatisfactoryCalculator.forms
{
	public partial class SelectRecipePrompt : Form
	{
		private readonly Dictionary<string, IRecipe>.ValueCollection originalList;

		public SelectRecipePrompt(Dictionary<string, IRecipe> options)
		{
			InitializeComponent();
			originalList = options.Values;
			foreach (IRecipe rec in options.Values)
			{
				_ = RecipesList.Items.Add(rec);
			}
		}

		private void YesButton_Click(object sender, EventArgs e)
		{
			if (RecipesList.SelectedItem is IRecipe)
			{
				DialogResult = DialogResult.OK;
				Close();
			}
		}

		public IRecipe GetSelectedRecipe()
		{
			if (RecipesList.SelectedItem is IRecipe)
			{
				return RecipesList.SelectedItem as IRecipe;
			}
			return default;
		}

		private void NoButton_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

		private void FilterBox_TextChanged(object sender, EventArgs e)
		{
			RecipesList.BeginUpdate();
			RecipesList.Items.Clear();
			foreach (IRecipe recipe in originalList)
			{
				if (recipe.ToString().ToLower().Contains(FilterBox.Text.ToLower()))
				{
					_ = RecipesList.Items.Add(recipe);
				}
			}
			RecipesList.EndUpdate();
		}
	}
}
