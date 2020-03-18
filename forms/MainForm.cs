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
	public partial class MainForm : Form, IReceivesRecipeList
	{
		[STAThread]
		public static void Main()
		{
			SuggestionsController.SC = new SuggestionsController();
			Application.EnableVisualStyles();
			Application.Run(new MainForm());
		}

		private const string recipeListFileName = "recipes.list";
		private List<Recipe> AllRecipes;

		public MainForm()
		{
			InitializeComponent();
			AllRecipes = SaveLoad.Load<List<Recipe>>(recipeListFileName);
			if (AllRecipes == null)
			{
				AllRecipes = new List<Recipe>();
			}
		}

		private void ViewEditGlobalRecipesButton_Click(object sender, EventArgs e)
		{
			new RecipeListForm(this, AllRecipes).ShowDialog();
		}

		public void SendRecipeList(List<Recipe> recipes, string purpose = null)
		{
			AllRecipes = recipes;
			SaveAllRecipesList();
		}

		private void SaveAllRecipesList()
		{
			SaveLoad.Save(recipeListFileName, AllRecipes);
		}
	}
}
