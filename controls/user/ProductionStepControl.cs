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
	public partial class ProductionStepControl : UserControl
	{
		private readonly ProductionStep parentStep;

		public ProductionStepControl()
		{

		}

		public ProductionStepControl(ProductionStep parentStep)
		{
			InitializeComponent();
			this.parentStep = parentStep;
			parentStep.SetControl(this);
			foreach (ItemCount ic in parentStep.GetProductItems())
			{
				ProductsPanel.Controls.Add(new ItemRateControl(this, ic));
			}
			foreach (ItemCount ic in parentStep.GetIngredientItems())
			{
				IngredientsPanel.Controls.Add(new ItemRateControl(this, ic));
			}
			RecipeLabel.Text = parentStep.ToString();
			MachineCountLabel.Text = parentStep.GetMachine() + ": " + parentStep.CalculateMachineCount();
		}

		public void MultiplierChanged()
		{
			foreach (ItemRateControl irc in GetItemRateControls())
			{
				irc.UpdateRateValue(parentStep.GetItemRate(irc.GetItem()));
			}
			MultiplierNumeric.Value = (decimal)parentStep.GetMultiplier();
			UpdateMultiplierNumericSettings();
		}

		public void RateChanged(Item item, double newRate)
		{
			if (parentStep.GetItemRate(item) != newRate)
			{
				parentStep.SetMultiplierAndRelatedRelative(item, newRate);
			}
		}

		public void ItemClicked(Item item)
		{
			if (parentStep.GetItemsWithRelatedStep().Contains(item))
			{

			}
			else
			{

			}
		}

		private List<ItemRateControl> GetItemRateControls()
		{
			List<ItemRateControl> irc = new List<ItemRateControl>();
			foreach (Control c in ProductsPanel.Controls)
			{
				if (c is ItemRateControl)
				{
					irc.Add(c as ItemRateControl);
				}
			}
			foreach (Control c in IngredientsPanel.Controls)
			{
				if (c is ItemRateControl)
				{
					irc.Add(c as ItemRateControl);
				}
			}
			return irc;
		}

		private void MultiplierNumeric_ValueChanged(object sender, EventArgs e)
		{
			if (parentStep.GetMultiplier() != (double)MultiplierNumeric.Value)
			{
				parentStep.SetMultiplier((double)MultiplierNumeric.Value);
			}
		}

		public void UpdateMultiplierNumericSettings()
		{
			if ((int)MultiplierNumeric.Value != MultiplierNumeric.Value)
			{
				MultiplierNumeric.DecimalPlaces = MultiplierNumeric.Value.ToString().Substring(MultiplierNumeric.Value.ToString().IndexOf('.') + 1).Length;
			}
			else
			{
				MultiplierNumeric.DecimalPlaces = 0;
			}
		}
	}
}
