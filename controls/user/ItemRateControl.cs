using System;
using System.Windows.Forms;
using VisualSatisfactoryCalculator.code.Extensions;
using VisualSatisfactoryCalculator.code.JSONClasses;

namespace VisualSatisfactoryCalculator.controls.user
{
	public partial class ItemRateControl : UserControl
	{
		private readonly ProductionStepControl parentControl;
		private readonly JSONItem item;
		private bool initialized;

		public ItemRateControl() : this(default, default, default)
		{

		}

		public ItemRateControl(ProductionStepControl parentControl, JSONItem item, decimal rate)
		{
			initialized = false;
			InitializeComponent();
			this.parentControl = parentControl;
			this.item = item;
			ItemButton.Text = item.ToString();
			RateNumeric.Value = rate.Abs();
			if (parentControl.ItemHasRelatedRecipe(item))
			{
				ItemButton.Enabled = false;
			}
		}

		private void RateNumeric_ValueChanged(object sender, EventArgs e)
		{
			if (Enabled && initialized)
			{
				Console.WriteLine(e.ToString());
				parentControl.RateChanged(item, RateNumeric.Value);
				parentControl.mainForm.UpdateTotalView();
			}
		}

		private void ItemButton_Click(object sender, EventArgs e)
		{
			parentControl.ItemClicked(item);
		}

		public void UpdateRateValue(decimal newRate)
		{
			if (newRate != RateNumeric.Value)
			{
				RateNumeric.Value = newRate.Abs();
			}
		}

		public JSONItem GetItem()
		{
			return item;
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
