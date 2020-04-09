using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using VisualSatisfactoryCalculator.code.DataStorage;
using VisualSatisfactoryCalculator.code.Extensions;
using VisualSatisfactoryCalculator.code.Interfaces;
using VisualSatisfactoryCalculator.code.Utility;
using VisualSatisfactoryCalculator.controls.user;

namespace VisualSatisfactoryCalculator.forms
{
	public partial class MainForm : Form, IReceives<IRecipe>
	{
		[STAThread]
		public static void Main()
		{
			Application.EnableVisualStyles();
			if (SafeNewMainForm(out MainForm safe))
			{
				Application.Run(safe);
			}
			else
			{
				Application.Exit();
			}
		}

		private const string firstRecipePurpose = "first recipe";
		private readonly List<IRecipe> AllRecipes;
		private ProductionPlan plan;
		private ProductionPlanTotalViewControl PPTVC;

		private MainForm()
		{
			InitializeComponent();
			AllRecipes = new List<IRecipe>();
		}

		private static bool SafeNewMainForm(out MainForm form)
		{
			form = new MainForm();
			OpenFileDialog dialog = new OpenFileDialog()
			{
				Title = "Select a save file",
				InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\FactoryGame\\Saved\\SaveGames\\",
				DefaultExt = ".sav",
				Filter = "save files (*.sav)|*.sav"
			};
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				form.AllRecipes.AddRange(SaveFileInteractor.GetRecipesFromSave(dialog.FileName));
				SuggestionsController.SC = new SuggestionsController(form.AllRecipes);
				return true;
			}
			return false;
		}

		private void SelectFirstRecipeButton_Click(object sender, EventArgs e)
		{
			new SelectRecipePrompt(AllRecipes, this, firstRecipePurpose).ShowDialog();
		}

		public void SendObject(IRecipe recipe, string purpose)
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

		public List<IRecipe> GetAllRecipes()
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
				Stream stream;
				if ((stream = dialog.OpenFile()) != null)
				{
					using (Bitmap map = new Bitmap(ProductionPlanPanel.PreferredSize.Width, ProductionPlanPanel.PreferredSize.Height))
					{
						ProductionPlanPanel.DrawToBitmap(map, new Rectangle(0, 0, map.Width, map.Height));
						byte[] bytes = new CondensedProductionPlan(plan).ToBytes();
						new BitmapSerializer(map).WriteBytes(bytes);
						map.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
					}
					stream.Close();
				}
			}
		}

		public void UpdateTotalView()
		{
			PPTVC.NetProductsLabel.Text = plan.GetNetProductsString();
			PPTVC.MachinesLabel.Text = plan.GetTotalMachineString();
			PPTVC.NetIngredientsLabel.Text = plan.GetNetIngredientsString();
		}

		private void LoadChartButton_Click(object sender, EventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog()
			{
				Title = "Select a previously made chart",
				InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
				DefaultExt = ".png",
				Filter = "png images (*.png)|*.png"
			};
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				Stream stream;
				if ((stream = dialog.OpenFile()) != null)
				{
					CondensedProductionPlan loadedPlan;
					using (Bitmap map = new Bitmap(stream))
					{
						int length = CondensedProductionPlan.BytesToInt(new BitmapSerializer(map).ReadBytes(CondensedProductionPlan.BYTES_OF_LENGTH));
						loadedPlan = CondensedProductionPlan.FromBytes(new BitmapSerializer(map).ReadBytes(CondensedProductionPlan.BYTES_OF_LENGTH + length));
					}
					if (loadedPlan != null)
					{
						plan = loadedPlan.ToProductionPlan(AllRecipes);
						PlanUpdated();
					}
					stream.Close();
				}
			}
		}
	}
}
