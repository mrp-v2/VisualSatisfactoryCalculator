﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using VisualSatisfactoryCalculator.code.DataStorage;
using VisualSatisfactoryCalculator.code.Extensions;
using VisualSatisfactoryCalculator.code.Interfaces;
using VisualSatisfactoryCalculator.code.JSONClasses;
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
				Console.WriteLine(e.ToString());
				parentStep.SetMultiplierAndRelated(MultiplierNumeric.Value);
				mainForm.UpdateTotalView();
			}
		}

		public void SendObject(IRecipe recipe, string purpose)
		{
			ProductionStep ps = new ProductionStep(recipe, parentStep);
			parentStep.AddRelatedStep(ps);
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
	}
}
