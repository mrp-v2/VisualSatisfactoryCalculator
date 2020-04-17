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
	public partial class MainForm : Form
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

		private readonly List<IRecipe> AllRecipes;
		public readonly List<IEncoder> encoders;
		private ProductionPlan plan;
		private ProductionPlanTotalViewControl PPTVC;

		private MainForm()
		{
			InitializeComponent();
			AllRecipes = new List<IRecipe>();
			encoders = new List<IEncoder>();
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
				form.encoders.AddRange(SaveFileInteractor.GetEncoders());
				form.AllRecipes.AddRange(SaveFileInteractor.GetUnlockedRecipesFromSave(dialog.FileName, form.encoders));
				SaveFileInteractor.FinishedInteracting();
				SuggestionsController.SC = new SuggestionsController(form.AllRecipes);
				return true;
			}
			return false;
		}

		private void SelectFirstRecipeButton_Click(object sender, EventArgs e)
		{
			SelectRecipePrompt srp = new SelectRecipePrompt(AllRecipes);
			if (srp.ShowDialog() == DialogResult.OK)
			{
				plan = new ProductionPlan(srp.GetSelectedRecipe());
				PlanUpdated();
			}
		}

		public void PlanUpdated()
		{
			foreach (Control c in ProductionPlanPanel.Controls)
			{
				c.Dispose();
			}
			ProductionPlanPanel.Controls.Clear();
			PPTVC = new ProductionPlanTotalViewControl();
			UpdateTotalView();
			ProductionPlanPanel.Controls.Add(PPTVC);
			ProductionStepControl PSC = new ProductionStepControl(plan, this, null);
			ProductionPlanPanel.Controls.Add(PSC);
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
			PPTVC.ProductsLabel.Text = plan.GetProductsString(encoders);
			PPTVC.MachinesLabel.Text = plan.GetTotalMachineString(encoders);
			PPTVC.IngredientsLabel.Text = plan.GetIngredientsString(encoders);
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
