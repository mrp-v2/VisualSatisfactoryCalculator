using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VisualSatisfactoryCalculator.code.DataStorage;
using VisualSatisfactoryCalculator.code.Extensions;
using VisualSatisfactoryCalculator.code.Interfaces;
using VisualSatisfactoryCalculator.forms;

namespace VisualSatisfactoryCalculator.controls.user
{
	public partial class ProductionStepControl : UserControl, IReceives<IRecipe>
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
				ProductsPanel.Controls.Add(new ItemRateControl(this, ic.GetItemUID(), parentStep.GetItemRate(ic.GetItemUID())));
			}
			foreach (ItemCount ic in parentStep.GetRecipe().GetItemCounts().GetIngredients())
			{
				IngredientsPanel.Controls.Add(new ItemRateControl(this, ic.GetItemUID(), parentStep.GetItemRate(ic.GetItemUID())));
			}
			RecipeLabel.Text = parentStep.GetRecipe().ToString(mainForm.encoders);
			MultiplierChanged();
			foreach (ItemRateControl irc in GetItemRateControls())
			{
				irc.FinishInitialization();
			}
			if (parentStep is ProductionPlan)
			{
				DeleteStepButton.Enabled = false;
			}
			foreach (ProductionStep childStep in parentStep.GetChildSteps())
			{
				if (childStep.GetRecipe().GetItemCounts().GetIngredients().ToItemUIDs().ContainsAny(parentStep.GetRecipe().GetItemCounts().GetProducts().ToItemUIDs()))
				{
					AddProductControl(childStep);
				}
				else if (childStep.GetRecipe().GetItemCounts().GetProducts().ToItemUIDs().ContainsAny(parentStep.GetRecipe().GetItemCounts().GetIngredients().ToItemUIDs()))
				{
					AddIngredientControl(childStep);
				}
				else
				{
					throw new ArgumentException("The child does not have a similarity to its parent!");
				}
			}
			FinishInitialization();
		}

		private void AddIngredientControl(ProductionStep step)
		{
			ChildIngredientsPanel.Controls.Add(new ProductionStepControl(step, mainForm, this));
		}

		private void AddProductControl(ProductionStep step)
		{
			ChildProductsPanel.Controls.Add(new ProductionStepControl(step, mainForm, this));
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
			if (parentStep.GetItemUIDsWithRelatedStep().Contains(itemUID))
			{

			}
			else
			{
				if (parentStep.GetRecipe().GetItemCounts().GetProducts().ToItemUIDs().Contains(itemUID))
				{
					new SelectRecipePrompt(mainForm.GetAllRecipes().GetRecipesThatConsume(itemUID), this, null).ShowDialog();
				}
				else if (parentStep.GetRecipe().GetItemCounts().GetIngredients().ToItemUIDs().Contains(itemUID))
				{
					new SelectRecipePrompt(mainForm.GetAllRecipes().GetRecipesThatProduce(itemUID), this, null).ShowDialog();
				}
				else
				{
					throw new ArgumentException();
				}
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
			if (Enabled && initialized)
			{
				parentStep.SetMultiplierAndRelated(MultiplierNumeric.Value);
				mainForm.UpdateTotalView();
			}
		}

		public void SendObject(IRecipe recipe, string purpose)
		{
			ProductionStep ps = new ProductionStep(recipe, parentStep);
			parentStep.AddChildStep(ps);
			if (parentStep.IsChildStepIngredient(ps)) AddIngredientControl(ps);
			else if (parentStep.IsChildStepProduct(ps)) AddProductControl(ps);
			UpdateButtons();
			mainForm.UpdateTotalView();
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
			if (parentControl != null) parentControl.UpdateButtons();
			mainForm.UpdateTotalView();
			Parent.Controls.Remove(this);
		}

		private void UpdateButtons()
		{
			foreach (ItemRateControl irc in IngredientsPanel.Controls)
			{
				irc.UpdateButton();
			} 
			foreach (ItemRateControl irc in ProductsPanel.Controls)
			{
				irc.UpdateButton();
			}
		}
	}
}
