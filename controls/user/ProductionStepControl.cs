using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VisualSatisfactoryCalculator.code.DataStorage;
using VisualSatisfactoryCalculator.code.Extensions;
using VisualSatisfactoryCalculator.forms;

namespace VisualSatisfactoryCalculator.controls.user
{
	public partial class ProductionStepControl : UserControl
	{
		private readonly ProductionStep parentStep;
		public readonly MainForm mainForm;
		private bool initialized;
		private readonly ProductionStepControl parentControl;

		public ProductionStepControl()
		{

		}

		public ProductionStepControl(ProductionStep parentStep, MainForm mainForm, ProductionStepControl parentControl)
		{
			initialized = false;
			InitializeComponent();
			this.parentStep = parentStep;
			this.mainForm = mainForm;
			this.parentControl = parentControl;
			parentStep.SetControl(this);
			foreach (ItemCount ic in parentStep.GetRecipe().GetItemCounts().GetProducts())
			{
				AddItemRateControl(ic.GetItemUID());
			}
			foreach (ItemCount ic in parentStep.GetRecipe().GetItemCounts().GetIngredients())
			{
				AddItemRateControl(ic.GetItemUID());
			}
			RecipeLabel.Text = parentStep.GetRecipe().ToString(mainForm.encoders);
			MultiplierChanged();
			if (parentStep is ProductionPlan)
			{
				DeleteStepButton.Enabled = false;
			}
			foreach (ProductionStep childStep in parentStep.GetChildSteps().Keys)
			{
				AddProductionStep(childStep);
			}
			UpdateButtons();
			FinishInitialization();
		}

		public void MultiplierChanged()
		{
			foreach (ItemRateControl irc in GetItemRateControls())
			{
				irc.UpdateRateValue(parentStep.GetItemRate(irc.GetItemUID()));
			}
			if (MultiplierNumeric.Value != parentStep.GetMultiplier())
			{
				MultiplierNumeric.Value = parentStep.GetMultiplier();
			}
			MachineCountLabel.Text = mainForm.encoders.GetDisplayNameFor(parentStep.GetRecipe().GetMachineUID()) + ": " + parentStep.CalculateMachineCount();
			PowerConsumptionLabel.Text = "Power Consumption: " + Math.Round(parentStep.GetPowerDraw(mainForm.encoders), 3) + "MW";
		}

		public void RateChanged(string itemUID, decimal newRate)
		{
			if (Math.Abs(parentStep.GetItemRate(itemUID)) != newRate)
			{
				parentStep.SetMultiplierAndRelatedRelative(itemUID, newRate);
			}
		}

		public void ItemClicked(string itemUID)
		{
			if (!parentStep.GetItemUIDsWithRelatedStep().Contains(itemUID))
			{
				if (parentStep.GetRecipe().GetItemCounts().GetProducts().ToItemUIDs().Contains(itemUID))
				{
					SelectRecipePrompt srp = new SelectRecipePrompt(mainForm.GetAllRecipes().GetRecipesThatConsume(itemUID));
					if (srp.ShowDialog() == DialogResult.OK)
					{
						ProductionStep ps = new ProductionStep(srp.GetSelectedRecipe(), parentStep, itemUID);
						AddProductionStep(ps);
					}
				}
				else if (parentStep.GetRecipe().GetItemCounts().GetIngredients().ToItemUIDs().Contains(itemUID))
				{
					SelectRecipePrompt srp = new SelectRecipePrompt(mainForm.GetAllRecipes().GetRecipesThatProduce(itemUID));
					if (srp.ShowDialog() == DialogResult.OK)
					{
						ProductionStep ps = new ProductionStep(srp.GetSelectedRecipe(), parentStep, itemUID);
						AddProductionStep(ps);
					}
				}
				else
				{
					throw new ArgumentException();
				}
			}
		}

		private void AddItemRateControl(string itemUID)
		{
			ItemRateControl irc = new ItemRateControl(this, itemUID, parentStep.GetItemRate(itemUID));
			if (parentStep.GetRecipe().GetItemCounts().GetIngredients().ToItemUIDs().Contains(itemUID))
			{
				IngredientsPanel.Controls.Add(irc);
			}
			else if (parentStep.GetRecipe().GetItemCounts().GetProducts().ToItemUIDs().Contains(itemUID))
			{
				ProductsPanel.Controls.Add(irc);
			}
			else
			{
				throw new ArgumentException();
			}
			irc.FinishInitialization();
		}

		private void AddProductionStep(ProductionStep ps)
		{
			ReplaceItemRateControl(parentStep.GetChildSteps()[ps], new ProductionStepControl(ps, mainForm, this));
			mainForm.UpdateTotalView();
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

		private void ReplaceItemRateControl(string itemUID, ProductionStepControl psc)
		{
			foreach (ItemRateControl irc in GetItemRateControls())
			{
				if (irc.GetItemUID().Equals(itemUID))
				{
					if (ProductsPanel.Controls.Contains(irc))
					{
						ProductsPanel.Controls.Add(psc);
						ProductsPanel.Controls.Remove(irc);
					}
					if (IngredientsPanel.Controls.Contains(irc))
					{
						IngredientsPanel.Controls.Add(psc);
						IngredientsPanel.Controls.Remove(irc);
					}
				}
			}
		}

		private void MultiplierNumeric_ValueChanged(object sender, EventArgs e)
		{
			if (Enabled && initialized)
			{
				parentStep.SetMultiplierAndRelated(MultiplierNumeric.Value);
				mainForm.UpdateTotalView();
			}
		}

		public bool ItemHasRelatedRecipe(string itemUID)
		{
			return parentStep.GetItemUIDsWithRelatedStep().Contains(itemUID);
		}

		public void ToggleInput(bool on)
		{
			foreach (ItemRateControl irc in GetItemRateControls())
			{
				irc.ToggleInput(on);
			}
			Enabled = on;
		}

		public void FinishInitialization()
		{
			initialized = true;
		}

		private void DeleteStepButton_Click(object sender, EventArgs e)
		{
			parentStep.RemoveStep();
			mainForm.UpdateTotalView();
			Parent.Controls.Remove(this);
			if (parentControl != null) parentControl.UpdateButtons();
		}

		private void UpdateButtons()
		{
			foreach (ItemRateControl irc in GetItemRateControls())
			{
				irc.UpdateButton();
			}
			foreach (string itemUID in GetItemUIDsWithNoMatch())
			{
				AddItemRateControl(itemUID);
			}
		}

		private List<ProductionStepControl> GetProductionStepControls()
		{
			List<ProductionStepControl> psc = new List<ProductionStepControl>();
			foreach (Control c in ProductsPanel.Controls)
			{
				if (c is ProductionStepControl)
				{
					psc.Add(c as ProductionStepControl);
				}
			}
			foreach (Control c in IngredientsPanel.Controls)
			{
				if (c is ProductionStepControl)
				{
					psc.Add(c as ProductionStepControl);
				}
			}
			return psc;
		}

		private List<string> GetItemUIDsWithNoMatch()
		{
			List<string> allUIDs = parentStep.GetRecipe().GetItemCounts().ToItemUIDs();
			foreach (ItemRateControl irc in GetItemRateControls())
			{
				allUIDs.Remove(irc.GetItemUID());
			}
			foreach (ProductionStepControl psc in GetProductionStepControls())
			{
				allUIDs.Remove(parentStep.GetChildSteps()[psc.parentStep]);
			}
			return allUIDs;
		}

		private void ProductsPanel_ControlAdded(object sender, EventArgs e)
		{
			foreach (Control c in ProductsPanel.Controls)
			{
				c.Anchor = AnchorStyles.Bottom;
			}
		}
	}
}
