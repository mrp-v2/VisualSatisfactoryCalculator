using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

using MrpV2.GenericLibrary.code.persistance.classes;

using VisualSatisfactoryCalculator.code.DataStorage;
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
			FileInteractor sfi = new FileInteractor();
			Encodings encoders = sfi.GetEncoders();
			Application.Run(new MainForm(encoders, sfi.GetAllRecipes(encoders)));
		}

		public Encodings Encoders { get; }

		private ProductionPlan _plan;
		private ProductionPlanTotalViewControl _PPTVC;

		private readonly DigitalStenographySaveLoad _saveLoad;

		private MainForm(Encodings encoders, Dictionary<string, IRecipe> recipes)
		{
			InitializeComponent();
			Encoders = encoders;
			_saveLoad = new DigitalStenographySaveLoad();
		}

		private void SelectFirstRecipeButton_Click(object sender, EventArgs e)
		{
			SelectRecipePrompt srp = new SelectRecipePrompt(Encoders.Recipes);
			if (srp.ShowDialog() == DialogResult.OK)
			{
				_plan = new ProductionPlan(srp.GetSelectedRecipe());
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
			_PPTVC = new ProductionPlanTotalViewControl();
			UpdateTotalView();
			ProductionPlanPanel.Controls.Add(_PPTVC);
			ProductionStepControl PSC = new ProductionStepControl(_plan, this, null);
			ProductionPlanPanel.Controls.Add(PSC);
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
				// TODO create image stuff
				// ProductionPlanPanel
				Size size = ProductionPlanPanel.GetPreferredSize(new Size());
				Bitmap map = new Bitmap(size.Width, size.Height);
				ProductionPlanPanel.DrawToBitmap(map, new Rectangle(0, 0, size.Width, size.Height));
				_saveLoad.Save(map, new CondensedProductionPlan(_plan));
				map.Save(dialog.FileName, ImageFormat.Png);
			}
		}

		public void UpdateTotalView()
		{
			_PPTVC.ProductsLabel.Text = _plan.GetProductsString(Encoders);
			_PPTVC.MachinesLabel.Text = _plan.GetTotalMachineString(Encoders);
			_PPTVC.IngredientsLabel.Text = _plan.GetIngredientsString(Encoders);
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
				if (_saveLoad.TryLoad(dialog.FileName, out CondensedProductionPlan loadedPlan))
				{
					if (loadedPlan != null)
					{
						_plan = loadedPlan.ToProductionPlan(Encoders.Recipes);
						PlanUpdated();
					}
				}
			}
		}
	}
}
