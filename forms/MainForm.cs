using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using VisualSatisfactoryCalculator.code;
using VisualSatisfactoryCalculator.controls.user;

namespace VisualSatisfactoryCalculator.forms
{
	public partial class MainForm : Form, IReceives<JSONRecipe>
	{
		[STAThread]
		public static void Main()
		{
			Application.EnableVisualStyles();
			Application.Run(new MainForm());
		}

		private const string firstRecipePurpose = "first recipe";
		private readonly List<JSONRecipe> AllRecipes;
		private ProductionPlan plan;
		private ProductionPlanTotalViewControl PPTVC;

		public MainForm()
		{
			InitializeComponent();
			OpenFileDialog dialog = new OpenFileDialog()
			{
				Title = "Select a save file",
				InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\FactoryGame\\Saved\\SaveGames\\",
				DefaultExt = ".sav",
				Filter = "save files (*.sav)|*.sav"
			};
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				AllRecipes = new List<JSONRecipe>();
				AllRecipes.AddRange(SaveFileInteractor.GetRecipesFromSave(dialog.FileName));
			} else
			{
				Close();
			}
			SuggestionsController.SC = new SuggestionsController(AllRecipes);
		}

		private void SelectFirstRecipeButton_Click(object sender, EventArgs e)
		{
			new SelectRecipePrompt(AllRecipes, this, firstRecipePurpose).ShowDialog();
		}

		public void SendObject(JSONRecipe recipe, string purpose)
		{
			switch (purpose)
			{
				case firstRecipePurpose:
					plan = new ProductionPlan(recipe);
					PlanUpdated();
					break;
			}
		}

		public void PlanUpdated()
		{
			Dictionary<sbyte, List<ProductionStep>> tiers = plan.GetTierList();
			foreach (Control c in ProductionPlanPanel.Controls)
			{
				c.Dispose();
			}
			ProductionPlanPanel.Controls.Clear();
			PPTVC = new ProductionPlanTotalViewControl();
			UpdateTotalView();
			ProductionPlanPanel.Controls.Add(PPTVC);
			for (sbyte i = 0; i < tiers.Count; i++)
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

		public List<JSONRecipe> GetAllRecipes()
		{
			return AllRecipes.ShallowClone();
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
