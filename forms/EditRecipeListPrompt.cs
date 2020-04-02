using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VisualSatisfactoryCalculator.code;

namespace VisualSatisfactoryCalculator.forms
{
	public partial class EditRecipeListPrompt : Form, IReceives<Recipe>
	{
		private readonly List<Recipe> recipes;
		private readonly IReceives<List<Recipe>> parentForm;
		private readonly string purpose;
		private readonly SuggestionsController TEMPSC;

		public EditRecipeListPrompt(IReceives<List<Recipe>> parentForm, List<Recipe> recipes, string purpose = null)
		{
			InitializeComponent();
			this.parentForm = parentForm;
			this.recipes = recipes;
			this.purpose = purpose;
			UpdateListVisual();
			TEMPSC = SuggestionsController.SC;
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

		public void SendObject(Recipe recipe, string purpose)
		{
			recipes.Add(recipe);
			SuggestionsController.SC = new SuggestionsController(recipes);
			UpdateListVisual();
		}

		private void AddRecipeButton_Click(object sender, EventArgs e)
		{
			new CreateRecipePrompt(this, null).ShowDialog();
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
			parentForm.SendObject(recipes, purpose);
			Close();
		}

		private void CancelButton_Click(object sender, EventArgs e)
		{
			SuggestionsController.SC = TEMPSC;
			Close();
		}

		private void SyncWithSaveButton_Click(object sender, EventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog()
			{
				Title = "Select a save file",
				InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\FactoryGame\\Saved\\SaveGames\\",
				DefaultExt = ".sav",
				Filter = "save files (*.sav)|*.sav"
			};
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				recipes.Clear();
				recipes.AddRange(SaveFileInteractor.GetRecipesFromSave(dialog.FileName));
				UpdateListVisual();
			}
		}
	}
}
