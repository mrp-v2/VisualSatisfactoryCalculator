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
using VisualSatisfactoryCalculator.controls.user;

namespace VisualSatisfactoryCalculator.forms
{
	public partial class MainForm : Form, IReceivesRecipeList, IReceivesRecipe
	{
		[STAThread]
		public static void Main()
		{
			Application.EnableVisualStyles();
			Application.Run(new MainForm());
		}

		private const string recipeListFileName = "recipes.list";
		private const string firstRecipePurpose = "first recipe";
		private List<Recipe> AllRecipes;
		private ProductionPlan plan;

		public MainForm()
		{
			InitializeComponent();
			AllRecipes = SaveLoad.Load<List<Recipe>>(recipeListFileName);
			SuggestionsController.SC = new SuggestionsController(AllRecipes);
			if (AllRecipes == null)
			{
				AllRecipes = new List<Recipe>();
			}
		}

		private void ViewEditGlobalRecipesButton_Click(object sender, EventArgs e)
		{
			new EditRecipeListPrompt(this, AllRecipes.Clone().CastToRecipeList()).ShowDialog();
		}

		public void SendRecipeList(List<Recipe> recipes, string purpose = null)
		{
			AllRecipes = recipes;
			SuggestionsController.SC = new SuggestionsController(AllRecipes);
			SaveAllRecipesList();
		}

		private void SaveAllRecipesList()
		{
			SaveLoad.Save(recipeListFileName, AllRecipes);
		}

		private void SelectFirstRecipeButton_Click(object sender, EventArgs e)
		{
			new SelectRecipePrompt(AllRecipes, this, firstRecipePurpose).ShowDialog();
		}

		public void AddRecipe(Recipe recipe, string purpose)
		{
			switch (purpose)
			{
				case firstRecipePurpose:
					plan = new ProductionPlan();
					ProductionStep ps = new ProductionStep(recipe, 1);
					plan.AddStep(ps);
					MainPanel.Controls.Add(new ProductionStepControl(ps));
					break;
			}
		}
	}
}
