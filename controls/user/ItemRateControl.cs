using System;
using System.Windows.Forms;

using VisualSatisfactoryCalculator.code.Extensions;
using VisualSatisfactoryCalculator.code.Interfaces;

namespace VisualSatisfactoryCalculator.controls.user
{
	public partial class ItemRateControl : UserControl
	{
		private readonly ProductionStepControl parentControl;
		public string ItemUID { get; }
		private bool initialized;
		public bool IsProduct { get; }

		public ItemRateControl(ProductionStepControl parentControl, string itemUID, decimal rate, bool isProduct)
		{
			initialized = false;
			InitializeComponent();
			this.parentControl = parentControl;
			ItemUID = itemUID;
			IsProduct = isProduct;
			ItemButton.Text = parentControl.mainForm.Encoders[itemUID].DisplayName;
			if ((parentControl.mainForm.Encoders[itemUID] as IItem).IsLiquid)
			{
				RateNumeric.Value = rate.Abs() / 1000;
			}
			else
			{
				RateNumeric.Value = rate.Abs();
			}

			UpdateButton();
		}

		public void UpdateButton()
		{
			ItemButton.Enabled = !parentControl.ItemHasRelatedRecipe(ItemUID);
		}

		private void RateNumeric_ValueChanged(object sender, EventArgs e)
		{
			if (Enabled && initialized)
			{
				if ((parentControl.mainForm.Encoders[ItemUID] as IItem).IsLiquid)
				{
					parentControl.RateChanged(ItemUID, RateNumeric.Value * 1000, IsProduct);
				}
				else
				{
					parentControl.RateChanged(ItemUID, RateNumeric.Value, IsProduct);
				}

				parentControl.mainForm.UpdateTotalView();
			}
		}

		private void ItemButton_Click(object sender, EventArgs e)
		{
			parentControl.ItemClicked(ItemUID, IsProduct);
		}

		public void UpdateRateValue(decimal newRate)
		{
			if (newRate != RateNumeric.Value)
			{
				if ((parentControl.mainForm.Encoders[ItemUID] as IItem).IsLiquid)
				{
					RateNumeric.Value = newRate.Abs() / 1000;
				}
				else
				{
					RateNumeric.Value = newRate.Abs();
				}
			}
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
