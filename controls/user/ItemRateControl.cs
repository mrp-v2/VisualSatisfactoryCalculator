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

		public ItemRateControl() : this(default, default)
		{

		}

		public ItemRateControl(ProductionStepControl parentControl, Item item)
		{
			InitializeComponent();
			this.parentControl = parentControl;
			this.item = item;
			ItemButton.Text = item.ToItemString();
		}

		private void RateNumeric_ValueChanged(object sender, EventArgs e)
		{
			UpdateNumericSettings();
			parentControl.RateChanged(item, (double)RateNumeric.Value);
		}

		public void UpdateNumericSettings()
		{
			if ((int)RateNumeric.Value != RateNumeric.Value)
			{
				RateNumeric.DecimalPlaces = RateNumeric.Value.ToString().Substring(RateNumeric.Value.ToString().IndexOf('.') + 1).Length;
			}
			else
			{
				RateNumeric.DecimalPlaces = 0;
			}
		}

		private void ItemButton_Click(object sender, EventArgs e)
		{
			parentControl.ItemClicked(item);
		}

		public void UpdateRateValue(double newRate)
		{
			RateNumeric.Value = Math.Abs((decimal)newRate);
			UpdateNumericSettings();
		}

		public Item GetItem()
		{
			return item;
		}
	}
}
