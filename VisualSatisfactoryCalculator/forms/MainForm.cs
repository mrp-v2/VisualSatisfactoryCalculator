﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
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
		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int wMsg, bool wParam, int lParam);
		private const int WM_SETREDRAW = 11;

		public void SuspendDrawing()
		{
			SendMessage(Handle, WM_SETREDRAW, false, 0);
		}

		public void ResumeDrawing()
		{
			SendMessage(Handle, WM_SETREDRAW, true, 0);
			Refresh();
		}

		[STAThread]
		public static void Main()
		{
			Application.EnableVisualStyles();
			FileInteractor sfi = new FileInteractor();
			Encodings encoders = sfi.GetEncoders();
			Application.Run(new MainForm(encoders));
			Console.WriteLine("Hello");
		}

		public Encodings Encoders { get; }

		public Plan Plan;
		private PlanTotalViewControl PPTVC;
		public bool ControlKeyPressed { get; private set; }
		public ItemRateControl CurrentConnectionIRC;
		public Func<Connection> CurrentConnectionFunc;

		private readonly DigitalStenographySaveLoad saveLoad;

		private MainForm(Encodings encoders)
		{
			InitializeComponent();
			KeyDown += MainForm_KeyDown;
			KeyUp += MainForm_KeyUp;
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
			SuspendDrawing();
			foreach (Control c in PlanPanel.Controls)
			{
				c.Dispose();
			}
			PlanPanel.Controls.Clear();
			PPTVC = new PlanTotalViewControl();
			UpdateTotalView();
			PlanPanel.Controls.Add(PPTVC);
			PlanLayoutMaker.LayoutSteps(this, PlanPanel, Plan);
			ResumeDrawing();
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
				saveLoad.Save(map, new CondensedPlan(Plan));
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
			double powerDraw = Plan.GetPowerDraw(Encoders);
			PPTVC.PowerDrawLabel.Text = powerDraw > 0 ? $"Power Draw: {powerDraw} MW" : $"Power Production: {-powerDraw} MW";
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
				if (saveLoad.TryLoad(dialog.FileName, out CondensedPlan loadedPlan))
				{
					if (loadedPlan != null)
					{
						Plan = loadedPlan.ToPlan(Encoders);
						PlanUpdated();
					}
				}
			}
		}

		void MainForm_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.ControlKey)
			{
				ControlKeyPressed = true;
			}
			if (e.KeyCode == Keys.Escape)
			{
				if (CurrentConnectionIRC != null)
				{
					CurrentConnectionIRC.ItemButton.Enabled = true;
					CurrentConnectionIRC = null;
					CurrentConnectionFunc = null;
				}
			}
		}

		void MainForm_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.ControlKey)
			{
				ControlKeyPressed = false;
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
