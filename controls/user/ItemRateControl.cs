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
			if ((mainForm.Encoders[itemUID] as IItem).IsFluid)
			{
				RateNumeric.Value = (rate.Abs() / 1000).ToDecimal();
			}
			else
			{
				RateNumeric.Value = rate.Abs().ToDecimal();
			}
		}

		private void RateNumeric_ValueChanged(object sender, EventArgs e)
		{
			if (RateNumeric.Value == 0)
			{
				return;
			}
			if (Enabled && initialized)
			{
				if ((mainForm.Encoders[ItemUID] as IItem).IsFluid)
				{
					rateChanged(ItemUID, RateNumeric.Value * 1000, IsProduct);
				}
				else
				{
					rateChanged(ItemUID, RateNumeric.Value, IsProduct);
				}
				mainForm.UpdateTotalView();
			}
		}

		private void ItemButton_Click(object sender, EventArgs e)
		{
			itemClicked(ItemUID, IsProduct);
		}

		public void UpdateRateValue(RationalNumber newRate)
		{
			if (newRate != RateNumeric.Value)
			{
				if ((mainForm.Encoders[ItemUID] as IItem).IsFluid)
				{
					RateNumeric.Value = (newRate.Abs() / 1000).ToDecimal();
				}
				else
				{
					RateNumeric.Value = newRate.Abs().ToDecimal();
				}
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
