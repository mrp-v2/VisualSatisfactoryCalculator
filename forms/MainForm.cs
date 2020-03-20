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
	public partial class MainForm : Form, IReceives<List<Recipe>>, IReceives<Recipe>
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

		public void SendObject(List<Recipe> recipes, string purpose = null)
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

		public void SendObject(Recipe recipe, string purpose)
		{
			switch (purpose)
			{
				case firstRecipePurpose:
					plan = new ProductionPlan(this);
					ProductionStep ps = new ProductionStep(recipe, 1);
					plan.AddStep(ps);
					break;
			}
		}

		public void PlanUpdated()
		{
			Dictionary<int, List<ProductionStep>> tiers = plan.GetAllSteps().ToTierList();
			foreach (Control c in ProductionPlanPanel.Controls)
			{
				c.Dispose();
			}
			ProductionPlanPanel.Controls.Clear();
			for (int i = tiers.Keys.Count - 1; i >= 0; i--)
			{
				FlowLayoutPanel flp = new FlowLayoutPanel
				{
					AutoSizeMode = AutoSizeMode.GrowAndShrink,
					AutoSize = true
				};
				ProductionPlanPanel.Controls.Add(flp);
				foreach (ProductionStep step in tiers[i])
				{
					flp.Controls.Add(new ProductionStepControl(step, this));
				}
			}
		}

		public List<Recipe> GetAllRecipes()
		{
			return AllRecipes.ShallowClone();
		}

		public void AddProductionStep(ProductionStep step)
		{
			plan.AddStep(step);
		}
	}
}
