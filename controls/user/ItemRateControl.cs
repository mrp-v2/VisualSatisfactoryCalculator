using System;
using System.Windows.Forms;
using VisualSatisfactoryCalculator.code.Extensions;
using VisualSatisfactoryCalculator.code.Interfaces;

namespace VisualSatisfactoryCalculator.controls.user
{
	public partial class ItemRateControl : UserControl
	{
		private readonly ProductionStepControl parentControl;
		private readonly string itemUID;
		private bool initialized;

		public ItemRateControl() : this(default, default, default)
		{

		}

		public ItemRateControl(ProductionStepControl parentControl, string itemUID, decimal rate)
		{
			initialized = false;
			InitializeComponent();
			this.parentControl = parentControl;
			this.itemUID = itemUID;
			ItemButton.Text = parentControl.mainForm.encoders.GetDisplayNameFor(itemUID);
			if ((parentControl.mainForm.encoders.FindByID(itemUID) as IItem).IsLiquid()) RateNumeric.Value = rate.Abs() / 1000;
			else RateNumeric.Value = rate.Abs();
			if (parentControl.ItemHasRelatedRecipe(itemUID))
			{
				ItemButton.Enabled = false;
			}
		}

		private void RateNumeric_ValueChanged(object sender, EventArgs e)
		{
			if (Enabled && initialized)
			{
				if ((parentControl.mainForm.encoders.FindByID(itemUID) as IItem).IsLiquid()) parentControl.RateChanged(itemUID, RateNumeric.Value * 1000);
				parentControl.RateChanged(itemUID, RateNumeric.Value);
				parentControl.mainForm.UpdateTotalView();
			}
		}

		private void ItemButton_Click(object sender, EventArgs e)
		{
			parentControl.ItemClicked(itemUID);
		}

		public void UpdateRateValue(decimal newRate)
		{
			if (newRate != RateNumeric.Value)
			{
				if ((parentControl.mainForm.encoders.FindByID(itemUID) as IItem).IsLiquid()) RateNumeric.Value = newRate.Abs() / 1000;
				else RateNumeric.Value = newRate.Abs();
			}
		}

		public string GetItemUID()
		{
			return itemUID;
		}

		public void ToggleInput(bool on)
		{
			Enabled = on;
		}

		public void FinishInitialization()
		{
			initialized = true;
		}
	}
}
