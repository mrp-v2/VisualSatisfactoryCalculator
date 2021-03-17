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
		private readonly ProductionStep parentStep;
		public readonly MainForm mainForm;
		private bool initialized;
		private readonly ProductionStepControl parentControl;

		public ProductionStepControl(ProductionStep parentStep, MainForm mainForm, ProductionStepControl parentControl)
		{
			initialized = false;
			InitializeComponent();
			this.parentStep = parentStep;
			this.mainForm = mainForm;
			this.parentControl = parentControl;
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
			if (parentStep is ProductionPlan)
			{
				DeleteStepButton.Enabled = false;
			}
			foreach (ProductionStep childStep in parentStep.ChildIngredientSteps.Keys)
			{
				AddProductionStep(childStep, false);
			}
			foreach (ProductionStep childStep in parentStep.ChildProductSteps.Keys)
			{
				AddProductionStep(childStep, true);
			}
			UpdateButtons();
			FinishInitialization();
		}

		public void MultiplierChanged()
		{
			foreach (ItemRateControl irc in GetItemRateControls())
			{
				irc.UpdateRateValue(parentStep.GetItemRate(irc.ItemUID, irc.IsProduct));
			}
			if (MultiplierNumeric.Value != parentStep.GetMultiplier())
			{
				MultiplierNumeric.Value = parentStep.GetMultiplier();
			}
			MachineCountLabel.Text = mainForm.Encoders[parentStep.GetRecipe().MachineUID].DisplayName + ": " + parentStep.CalculateMachineCount() + " x " + parentStep.CalculateMachineClockPercentage() + "%";
			PowerConsumptionLabel.Text = "Power Consumption: " + Math.Round(parentStep.GetPowerDraw(mainForm.Encoders), 3) + "MW";
		}

		public void RateChanged(string itemUID, decimal newRate, bool isProduct)
		{
			if (Math.Abs(parentStep.GetItemRate(itemUID, isProduct)) != newRate)
			{
				parentStep.SetMultiplierAndRelatedRelative(itemUID, newRate, isProduct);
			}
		}

		public void ItemClicked(string itemUID, bool isProduct)
		{
			if (!parentStep.GetItemUIDsWithRelatedStep().Contains(itemUID))
			{
				SelectRecipePrompt srp;
				if (isProduct)
				{
					srp = new SelectRecipePrompt(mainForm.Recipes.GetRecipesThatConsume(itemUID));
				}
				else
				{
					srp = new SelectRecipePrompt(mainForm.Recipes.GetRecipesThatProduce(itemUID));
				}
				if (srp.ShowDialog() == DialogResult.OK)
				{
					ProductionStep ps = new ProductionStep(srp.GetSelectedRecipe(), parentStep, itemUID, isProduct);
					AddProductionStep(ps, isProduct);
				}
			}
		}

		private void AddItemRateControl(string itemUID, bool isProduct)
		{
			ItemRateControl irc = new ItemRateControl(this, itemUID, parentStep.GetItemRate(itemUID, isProduct), isProduct);
			if (isProduct)
			{
				ProductsPanel.Controls.Add(irc);
			}
			else
			{
				IngredientsPanel.Controls.Add(irc);
			}
			irc.FinishInitialization();
		}

		private void AddProductionStep(ProductionStep ps, bool isItemProduct)
		{
			if (isItemProduct)
			{
				ReplaceItemRateControl(parentStep.ChildProductSteps[ps], new ProductionStepControl(ps, mainForm, this), isItemProduct);
			}
			else
			{
				ReplaceItemRateControl(parentStep.ChildIngredientSteps[ps], new ProductionStepControl(ps, mainForm, this), isItemProduct);
			}

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

		private void ReplaceItemRateControl(string itemUID, ProductionStepControl psc, bool isProduct)
		{
			foreach (ItemRateControl irc in GetItemRateControls()) // TODO fix this to allow for having an item that is both an ingredient and a product
			{
				if (irc.ItemUID.Equals(itemUID))
				{
					if (isProduct)
					{
						int index = ProductsPanel.Controls.GetChildIndex(irc);
						ProductsPanel.Controls.Remove(irc);
						ProductsPanel.Controls.Add(psc);
						ProductsPanel.Controls.SetChildIndex(psc, index);
					}
					else
					{
						int index = IngredientsPanel.Controls.GetChildIndex(irc);
						IngredientsPanel.Controls.Remove(irc);
						IngredientsPanel.Controls.Add(psc);
						IngredientsPanel.Controls.SetChildIndex(psc, index);
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
			if (parentControl != null)
			{
				parentControl.UpdateButtons();
			}
		}

		private void UpdateButtons()
		{
			foreach (ItemRateControl irc in GetItemRateControls())
			{
				irc.UpdateButton();
			}
			foreach (string itemUID in GetIngredientUIDsWithNoMatch())
			{
				AddItemRateControl(itemUID, false);
			}
			foreach (string itemUID in GetProductUIDsWithNoMatch())
			{
				AddItemRateControl(itemUID, true);
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

		private List<string> GetProductUIDsWithNoMatch()
		{
			List<string> productUIDs = parentStep.GetRecipe().Products.Keys.ToList();
			foreach (ItemRateControl itemRateControl in GetItemRateControls())
			{
				if (itemRateControl.IsProduct)
				{
					_ = productUIDs.Remove(itemRateControl.ItemUID);
				}
			}
			foreach (ProductionStepControl productionStepControl in GetProductionStepControls())
			{
				if (productionStepControl.parentStep.IsProductOfParent)
				{
					_ = productUIDs.Remove(parentStep.ChildProductSteps[productionStepControl.parentStep]);
				}
			}
			return productUIDs;
		}

		private List<string> GetIngredientUIDsWithNoMatch()
		{
			List<string> ingredientUIDs = parentStep.GetRecipe().Ingredients.Keys.ToList();
			foreach (ItemRateControl itemRateControl in GetItemRateControls())
			{
				if (!itemRateControl.IsProduct)
				{
					_ = ingredientUIDs.Remove(itemRateControl.ItemUID);
				}
			}
			foreach (ProductionStepControl productionStepControl in GetProductionStepControls())
			{
				if (!productionStepControl.parentStep.IsProductOfParent)
				{
					_ = ingredientUIDs.Remove(parentStep.ChildIngredientSteps[productionStepControl.parentStep]);
				}
			}
			return ingredientUIDs;
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
