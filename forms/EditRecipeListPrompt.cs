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
	public partial class EditRecipeListPrompt : Form, IReceivesRecipe
	{
		private readonly List<Recipe> recipes;
		private readonly IReceivesRecipeList parentForm;
		private readonly string purpose;

		public EditRecipeListPrompt(IReceivesRecipeList parentForm, List<Recipe> recipes, string purpose = null)
		{
			InitializeComponent();
			this.parentForm = parentForm;
			this.recipes = recipes;
			this.purpose = purpose;
			UpdateListVisual();
		}

		private void UpdateListVisual()
		{
			RecipeList.BeginUpdate();
			RecipeList.Items.Clear();
			foreach (Recipe rec in recipes)
			{
				RecipeList.Items.Add(rec);
			}
			RecipeList.EndUpdate();
		}

		public void AddRecipe(Recipe recipe, string purpose = null)
		{
			recipes.Add(recipe);
			UpdateListVisual();
		}

		private void AddRecipeButton_Click(object sender, EventArgs e)
		{
			new CreateRecipePrompt(this).ShowDialog();
		}

		private void RemoveRecipeButton_Click(object sender, EventArgs e)
		{
			try
			{
				if (RecipeList.SelectedItem is Recipe)
				{
					recipes.Remove(RecipeList.SelectedItem as Recipe);
					UpdateListVisual();
				}
			}
			catch
			{

			}
		}

		private void FinishButton_Click(object sender, EventArgs e)
		{
			parentForm.SendRecipeList(recipes, purpose);
			Close();
		}

		private void CancelButton_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
