using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
		private ProductionPlanTotalViewControl PPTVC;

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
					ProductionStep ps = new ProductionStep(recipe, 1m);
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
			PPTVC = new ProductionPlanTotalViewControl();
			UpdateTotalView();
			ProductionPlanPanel.Controls.Add(PPTVC);
			for (int i = 0; i < tiers.Count; i++)
			{
				FlowLayoutPanel flp = new FlowLayoutPanel
				{
					AutoSizeMode = AutoSizeMode.GrowAndShrink,
					AutoSize = true,
					Margin = new Padding(3),
					WrapContents = false,
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

		private void SaveChartButton_Click(object sender, EventArgs e)
		{
			SaveFileDialog dialog = new SaveFileDialog
			{
				Title = "Save Chart",
				InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
				Filter = "png images (*.png)|*.png",
				DefaultExt = ".png"
			};
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				Stream file;
				if ((file = dialog.OpenFile()) != null)
				{
					Bitmap map = new Bitmap(ProductionPlanPanel.PreferredSize.Width, ProductionPlanPanel.PreferredSize.Height);
					ProductionPlanPanel.DrawToBitmap(map, new Rectangle(0, 0, map.Width, map.Height));
					map.Save(file, System.Drawing.Imaging.ImageFormat.Png);
					file.Close();
				}
			}
		}

		public void UpdateTotalView()
		{
			PPTVC.NetProductsLabel.Text = plan.GetNetProductsString();
			PPTVC.MachinesLabel.Text = plan.GetTotalMachineString();
			PPTVC.NetIngredientsLabel.Text = plan.GetNetIngredientsString();
		}
	}
}
