using System;
using System.Drawing;
using System.Windows.Forms;

using VisualSatisfactoryCalculator.code.Extensions;
using VisualSatisfactoryCalculator.code.Interfaces;
using VisualSatisfactoryCalculator.code.Numbers;
using VisualSatisfactoryCalculator.code.Production;
using VisualSatisfactoryCalculator.code.Utility;
using VisualSatisfactoryCalculator.forms;

namespace VisualSatisfactoryCalculator.controls.user
{
	public partial class ItemRateControl : UserControl
	{
		public delegate void RateChanged(string itemUID, RationalNumber newRate, bool isProduct);
		public delegate void ItemClicked(string itemUID, bool isProduct);

		public string ItemUID { get; }
		private bool initialized;
		public bool IsProduct { get; }
		private readonly MainForm mainForm;
		private readonly RateChanged rateChanged;
		private readonly ItemClicked itemClicked;
		private readonly int panelDepth;

		public Point GetTotalLocation()
		{
			return PlanLayoutMaker.AddParentPoints(this, panelDepth);
		}

		public ItemRateControl(MainForm mainForm, string itemUID, RationalNumber rate, bool isProduct, int panelDepth, RateChanged rateChanged, ItemClicked itemClicked)
		{
			initialized = false;
			InitializeComponent();
			this.mainForm = mainForm;
			this.rateChanged = rateChanged;
			this.itemClicked = itemClicked;
			this.panelDepth = panelDepth;
			ItemUID = itemUID;
			IsProduct = isProduct;
			ItemButton.Text = mainForm.Encoders[itemUID].DisplayName;
			UpdateRateValue(rate);
			NumberControl.AddNumberChangedListener(NumberChanged);
		}

		private void NumberChanged()
		{
			if (Enabled && initialized)
			{
				mainForm.SuspendDrawing();
				rateChanged(ItemUID, NumberControl.GetNumber(), IsProduct);
				mainForm.UpdateTotalView();
				mainForm.ResumeDrawing();
			}
		}

		private void ItemButton_Click(object sender, EventArgs e)
		{
			itemClicked(ItemUID, IsProduct);
		}

		public void UpdateRateValue(RationalNumber newRate)
		{
			NumberControl.SetNumber(newRate);
		}

		public void ToggleInput(bool on)
		{
			if (!NumberControl.GetNumber().IsNonZero && Enabled)
			{
				Enabled = false;
				return;
			}
			Enabled = on;
		}

		public void FinishInitialization()
		{
			initialized = true;
		}
	}
}
