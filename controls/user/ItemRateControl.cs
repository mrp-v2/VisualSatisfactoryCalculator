using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisualSatisfactoryCalculator.code;

namespace VisualSatisfactoryCalculator.controls.user
{
	public partial class ItemRateControl : UserControl
	{
		private readonly ProductionStepControl parentControl;
		private readonly Item item;

		public ItemRateControl() : this(default, default, default)
		{

		}

		public ItemRateControl(ProductionStepControl parentControl, Item item, decimal rate)
		{
			InitializeComponent();
			this.parentControl = parentControl;
			this.item = item;
			ItemButton.Text = item.ToItemString();
			RateNumeric.Value = rate.Abs();
			if (parentControl.ItemHasRelatedRecipe(item))
			{
				ItemButton.Enabled = false;
			}
		}

		private void RateNumeric_ValueChanged(object sender, EventArgs e)
		{
			parentControl.RateChanged(item, RateNumeric.Value);
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

		public Item GetItem()
		{
			return item;
		}
	}
}
