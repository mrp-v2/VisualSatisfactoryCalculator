using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using MrpV2.GenericLibrary.code.persistance.classes;

using VisualSatisfactoryCalculator.satisfactory.Production;
using VisualSatisfactoryCalculator.satisfactory.Utility;
using VisualSatisfactoryCalculator.controls.user;
using VisualSatisfactoryCalculator.code.Utility;

using Connection = VisualSatisfactoryCalculator.model.production.Connection<VisualSatisfactoryCalculator.satisfactory.model.production.Item, VisualSatisfactoryCalculator.satisfactory.Production.Step, VisualSatisfactoryCalculator.satisfactory.model.production.Recipe>;

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
			JsonEncodings jsonEncodings = sfi.GetEncoders();
			Encodings encodings = jsonEncodings.Process();
			Application.Run(new MainForm(encodings));
		}

		public Encodings Encoders { get; }

		public Plan plan;
		private PlanTotalViewControl ptvc;
		public bool ControlKeyPressed { get; private set; }
		public ItemRateControl currentConnectionIRC;
		public Func<Connection> currentConnectionFunc;

		private readonly DigitalStenographySaveLoad _saveLoad;

		private MainForm(Encodings encoders)
		{
			InitializeComponent();
			KeyDown += MainForm_KeyDown;
			KeyUp += MainForm_KeyUp;
			Encoders = encoders;
			_saveLoad = new DigitalStenographySaveLoad();
			plan = new Plan();
		}

		private void AddStepButton_Click(object sender, EventArgs e)
		{
			SelectRecipePrompt srp = new SelectRecipePrompt(Encoders.recipes.Values);
			if (srp.ShowDialog() == DialogResult.OK)
			{
				Step step = new Step(srp.GetSelectedRecipe());
				plan.steps.Add(step);
				plan.processedPlan.Invalidate();
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
			ptvc = new PlanTotalViewControl();
			UpdateTotalView();
			PlanPanel.Controls.Add(ptvc);
			PlanLayoutMaker.LayoutSteps(this, PlanPanel, plan);
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
				_saveLoad.Save(map, new CondensedPlan(plan));
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
			ptvc.ProductsLabel.Text = plan.GetProductsString();
			double powerDraw = plan.GetPowerDraw();
			ptvc.PowerDrawLabel.Text = powerDraw > 0 ? $"Power Draw: {powerDraw} MW" : $"Power Production: {-powerDraw} MW";
			ptvc.MachinesLabel.Text = plan.GetMachinesString();
			ptvc.IngredientsLabel.Text = plan.GetIngredientsString();
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
				if (_saveLoad.TryLoad(dialog.FileName, out CondensedPlan loadedPlan))
				{
					if (loadedPlan != null)
					{
						plan = loadedPlan.ToPlan(Encoders);
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
				if (currentConnectionIRC != null)
				{
					currentConnectionIRC.ItemButton.Enabled = true;
					currentConnectionIRC = null;
					currentConnectionFunc = null;
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
			plan.steps.Clear();
			plan.processedPlan.Invalidate();
			PlanUpdated();
		}
	}
}
