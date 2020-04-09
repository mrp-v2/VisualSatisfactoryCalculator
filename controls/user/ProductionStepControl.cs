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

		public ProductionStepControl()
		{

		}

		public ProductionStepControl(ProductionStep parentStep, MainForm mainForm)
		{
			initialized = false;
			InitializeComponent();
			this.parentStep = parentStep;
			this.mainForm = mainForm;
			parentStep.SetControl(this);
			foreach (ItemCount ic in parentStep.GetRecipe().GetItemCounts().GetProducts())
			{
				ProductsPanel.Controls.Add(new ItemRateControl(this, ic.GetItem(), parentStep.GetItemRate(ic.GetItem())));
			}
			foreach (ItemCount ic in parentStep.GetRecipe().GetItemCounts().GetIngredients())
			{
				IngredientsPanel.Controls.Add(new ItemRateControl(this, ic.GetItem(), parentStep.GetItemRate(ic.GetItem())));
			}
			RecipeLabel.Text = parentStep.ToString();
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
				if (childStep.GetRecipe().GetItemCounts().GetIngredients().ToItems().ContainsAny(parentStep.GetRecipe().GetItemCounts().GetProducts().ToItems()))
				{
					ChildProductsPanel.Controls.Add(new ProductionStepControl(childStep, mainForm));
				}
				else if (childStep.GetRecipe().GetItemCounts().GetProducts().ToItems().ContainsAny(parentStep.GetRecipe().GetItemCounts().GetIngredients().ToItems()))
				{
					ChildIngredientsPanel.Controls.Add(new ProductionStepControl(childStep, mainForm));
				}
				else
				{
					throw new ArgumentException("The child does not have a similarity to its parent!");
				}
			}
			FinishInitialization();
		}

		public void MultiplierChanged()
		{
			foreach (ItemRateControl irc in GetItemRateControls())
			{
				irc.UpdateRateValue(parentStep.GetItemRate(irc.GetItem()));
			}
			if (MultiplierNumeric.Value != parentStep.GetMultiplier())
			{
				MultiplierNumeric.Value = parentStep.GetMultiplier();
			}
			MachineCountLabel.Text = parentStep.GetRecipe().GetMachine() + ": " + parentStep.CalculateMachineCount();
		}

		public void RateChanged(IItem item, decimal newRate)
		{
			if (Math.Abs(parentStep.GetItemRate(item)) != newRate)
			{
				parentStep.SetMultiplierAndRelatedRelative(item, newRate);
			}
		}

		public void ItemClicked(IItem item)
		{
			if (parentStep.GetItemsWithRelatedStep().Contains(item))
			{

			}
			else
			{
				if (parentStep.GetRecipe().GetItemCounts().GetProducts().ContainsItem(item))
				{
					new SelectRecipePrompt(mainForm.GetAllRecipes().GetRecipesThatConsume(item), this, null).ShowDialog();
				}
				else if (parentStep.GetRecipe().GetItemCounts().GetIngredients().ContainsItem(item))
				{
					new SelectRecipePrompt(mainForm.GetAllRecipes().GetRecipesThatProduce(item), this, null).ShowDialog();
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
			mainForm.PlanUpdated();
		}

		public bool ItemHasRelatedRecipe(IItem item)
		{
			return parentStep.GetItemsWithRelatedStep().Contains(item);
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
			mainForm.PlanUpdated();
		}
	}
}
