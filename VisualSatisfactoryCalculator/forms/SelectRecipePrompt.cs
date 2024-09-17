using System;
using System.Collections.Generic;
using System.Windows.Forms;

using VisualSatisfactoryCalculator.satisfactory.model.production;

namespace VisualSatisfactoryCalculator.forms
{
	public partial class SelectRecipePrompt : Form
	{
		private readonly IEnumerable<Recipe> _originalList;

		public SelectRecipePrompt(IEnumerable<Recipe> options)
		{
			InitializeComponent();
			_originalList = options;
			foreach (Recipe rec in options)
			{
				RecipesList.Items.Add(rec);
			}
		}

		private void YesButton_Click(object sender, EventArgs e)
		{
			if (RecipesList.SelectedItem is Recipe)
			{
				DialogResult = DialogResult.OK;
				Close();
			}
		}

		public Recipe GetSelectedRecipe()
		{
			if (RecipesList.SelectedItem is Recipe)
			{
				return RecipesList.SelectedItem as Recipe;
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
			foreach (Recipe recipe in _originalList)
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
