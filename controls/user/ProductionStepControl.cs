using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using VisualSatisfactoryCalculator.code.DataStorage;
using VisualSatisfactoryCalculator.code.Extensions;
using VisualSatisfactoryCalculator.forms;

namespace VisualSatisfactoryCalculator.controls.user
{
	public partial class ProductionStepControl : UserControl
	{
		private readonly Step backingStep;
		public readonly MainForm mainForm;
		private bool initialized;

		public ProductionStepControl(Step parentStep, MainForm mainForm, ProductionStepControl parentControl)
		{
			initialized = false;
			InitializeComponent();
			this.backingStep = parentStep;
			this.mainForm = mainForm;
			parentStep.SetControl(this);
			foreach (ItemCount ic in parentStep.GetRecipe().Products.Values)
			{
				AddItemRateControl(ic.ItemUID, true);
			}
			foreach (ItemCount ic in parentStep.GetRecipe().Ingredients.Values)
			{
				AddItemRateControl(ic.ItemUID, false);
			}
			RecipeLabel.Text = parentStep.GetRecipe().ToString(mainForm.Encoders, "{name} | {conversion} | {time} seconds");
			MultiplierChanged();
			FinishInitialization();
		}

		public void MultiplierChanged()
		{
			foreach (ItemRateControl irc in GetItemRateControls())
			{
				irc.UpdateRateValue(backingStep.GetItemRate(irc.ItemUID, irc.IsProduct));
			}
			if (MultiplierNumeric.Value != backingStep.GetMultiplier())
			{
				MultiplierNumeric.Value = backingStep.GetMultiplier();
			}
			MachineCountLabel.Text = mainForm.Encoders[backingStep.GetRecipe().MachineUID].DisplayName + ": " + backingStep.CalculateMachineCount() + " x " + backingStep.CalculateMachineClockPercentage() + "%";
			PowerConsumptionLabel.Text = "Power Consumption: " + backingStep.GetPowerDraw(mainForm.Encoders).ToPrettyString() + "MW";
		}

		public void RateChanged(string itemUID, decimal newRate, bool isProduct)
		{
			if (Math.Abs(backingStep.GetItemRate(itemUID, isProduct)) != newRate)
			{
				// TODO
			}
		}

		public void ItemClicked(string itemUID, bool isProduct)
		{
			if (!backingStep.GetItemUIDsWithRelatedStep().Contains(itemUID))
			{
				SelectRecipePrompt srp;
				if (isProduct)
				{
					srp = new SelectRecipePrompt(mainForm.Encoders.Recipes.GetRecipesThatConsume(itemUID));
				}
				else
				{
					srp = new SelectRecipePrompt(mainForm.Encoders.Recipes.GetRecipesThatProduce(itemUID));
				}
				if (srp.ShowDialog() == DialogResult.OK)
				{
					Step ps = new Step(srp.GetSelectedRecipe(), backingStep, itemUID, isProduct);
					// TODO
				}
			}
		}

		private void AddItemRateControl(string itemUID, bool isProduct)
		{
			ItemRateControl irc = new ItemRateControl(this, itemUID, backingStep.GetItemRate(itemUID, isProduct), isProduct);
			if (isProduct)
			{
				irc.Anchor = AnchorStyles.Bottom;
				ProductsPanel.Controls.Add(irc);
			}
			else
			{
				IngredientsPanel.Controls.Add(irc);
			}
			irc.FinishInitialization();
		}

		private List<ItemRateControl> GetItemRateControls()
		{
			List<ItemRateControl> list = GetItemRateControls(true);
			list.AddRange(GetItemRateControls(false));
			return list;
		}

		private List<ItemRateControl> GetItemRateControls(bool products)
		{
			List<ItemRateControl> irc = new List<ItemRateControl>();
			foreach (Control c in products ? ProductsPanel.Controls : IngredientsPanel.Controls)
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
			if (MultiplierNumeric.Value == 0)
			{
				return;
			}
			if (Enabled && initialized)
			{
				// TODO
			}
		}

		public bool ItemHasRelatedRecipe(string itemUID)
		{
			return backingStep.GetItemUIDsWithRelatedStep().Contains(itemUID);
		}

		public void ToggleInput(bool on)
		{
			Enabled = on;
		}

		public void FinishInitialization()
		{
			initialized = true;
		}

		private void DeleteStepButton_Click(object sender, EventArgs e)
		{
			backingStep.Delete();
			mainForm.UpdateTotalView();
			Parent.Controls.Remove(this);
			// TODO
		}
	}
}
