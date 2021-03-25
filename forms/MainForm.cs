using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

using MrpV2.GenericLibrary.code.persistance.classes;

using VisualSatisfactoryCalculator.code.DataStorage;
using VisualSatisfactoryCalculator.code.Interfaces;
using VisualSatisfactoryCalculator.code.Production;
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
			Application.Run(new MainForm(encoders));
		}

		public Encodings Encoders { get; }

		public readonly Plan Plan;
		private PlanTotalViewControl PPTVC;

		private readonly DigitalStenographySaveLoad saveLoad;

		private MainForm(Encodings encoders)
		{
			InitializeComponent();
			Encoders = encoders;
			saveLoad = new DigitalStenographySaveLoad();
			Plan = new Plan();
		}

		private void AddStepButton_Click(object sender, EventArgs e)
		{
			SelectRecipePrompt srp = new SelectRecipePrompt(Encoders.Recipes);
			if (srp.ShowDialog() == DialogResult.OK)
			{
				Step step = new Step(srp.GetSelectedRecipe());
				Plan.Steps.Add(step);
				Plan.ProcessedPlan.Invalidate();
				PlanUpdated();
			}
		}

		public void PlanUpdated()
		{
			foreach (Control c in PlanPanel.Controls)
			{
				c.Dispose();
			}
			PlanPanel.Controls.Clear();
			PPTVC = new PlanTotalViewControl();
			UpdateTotalView();
			PlanPanel.Controls.Add(PPTVC);
			PlanLayoutMaker.LayoutSteps(this, PlanPanel, Plan);
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
				Size size = PlanPanel.GetPreferredSize(new Size());
				Bitmap map = new Bitmap(size.Width, size.Height);
				ReverseControlOrder();
				PlanPanel.DrawToBitmap(map, new Rectangle(0, 0, size.Width, size.Height));
				ReverseControlOrder();
				//saveLoad.Save(map, Plan);
				map.Save(dialog.FileName, ImageFormat.Png);
			}
		}

		private void ReverseControlOrder()
		{
			Control[] controls = new Control[PlanPanel.Controls.Count];
			PlanPanel.Controls.CopyTo(controls, 0);
			PlanPanel.Controls.Clear();
			Control[] reversedControls = new Control[controls.Length];
			for (int i = 0; i < controls.Length; i++)
			{
				reversedControls[controls.Length - 1 - i] = controls[i];
			}
			PlanPanel.Controls.AddRange(reversedControls);
		}

		public void UpdateTotalView()
		{
			PPTVC.ProductsLabel.Text = Plan.GetProductsString(Encoders);
			PPTVC.MachinesLabel.Text = Plan.GetMachinesString(Encoders);
			PPTVC.IngredientsLabel.Text = Plan.GetIngredientsString(Encoders);
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
				if (saveLoad.TryLoad(dialog.FileName, out ProcessedPlan loadedPlan))
				{
					if (loadedPlan != null)
					{
						//_plan = loadedPlan.ToProductionPlan(Encoders.Recipes);
						//PlanUpdated();
						// TODO
					}
				}
			}
		}

		private void ClearStepsButton_Click(object sender, EventArgs e)
		{
			Plan.Steps.Clear();
			Plan.ProcessedPlan.Invalidate();
			PlanUpdated();
		}
	}
}
