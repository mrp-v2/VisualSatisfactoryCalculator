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
		private readonly bool isItemFluid;

		private decimal oldDValue;
		private RationalNumber oldRNValue;

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
			isItemFluid = (mainForm.Encoders[itemUID] as IItem).IsFluid;
			UpdateRateValue(rate);
		}

		private void RateNumeric_ValueChanged(object sender, EventArgs e)
		{
			if (RateNumeric.Value == 0)
			{
				return;
			}
			if (Enabled && initialized)
			{
				mainForm.SuspendDrawing();
				decimal difference = (RateNumeric.Value - oldDValue) * 1000;
				if (isItemFluid)
				{
					difference *= 1000;
				}
				RationalNumber newRN = oldRNValue + difference;
				rateChanged(ItemUID, newRN, IsProduct);
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
			if (newRate.ToDecimal().Round() != RateNumeric.Value)
			{
				if (isItemFluid)
				{
					RateNumeric.Value = (newRate.Abs() / 1000).ToDecimal().Round();
				}
				else
				{
					RateNumeric.Value = newRate.Abs().ToDecimal().Round();
				}
				oldDValue = RateNumeric.Value;
				oldRNValue = newRate;
			}
		}

		public void ToggleInput(bool on)
		{
			if (RateNumeric.Value == 0 && Enabled)
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
