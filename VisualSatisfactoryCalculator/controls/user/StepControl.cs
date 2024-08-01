﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using VisualSatisfactoryCalculator.code.Extensions;
using VisualSatisfactoryCalculator.code.Numbers;
using VisualSatisfactoryCalculator.code.Production;
using VisualSatisfactoryCalculator.code.Utility;
using VisualSatisfactoryCalculator.forms;
using VisualSatisfactoryCalculator.model.production;

namespace VisualSatisfactoryCalculator.controls.user
{
	public partial class StepControl : UserControl, PlanLayoutMaker.ILayoutControl
	{
		public readonly Step BackingStep;
		public readonly MainForm MainForm;
		private bool initialized = false;
		public readonly Dictionary<string, ItemRateControl> ProductRateControls = new Dictionary<string, ItemRateControl>();
		public readonly Dictionary<string, ItemRateControl> IngredientRateControls = new Dictionary<string, ItemRateControl>();
		public StepControl TopControl { get { return this; } }

		public StepControl(Step backingStep, MainForm mainForm)
		{
			InitializeComponent();
			BackingStep = backingStep;
			MainForm = mainForm;
			backingStep.SetControl(this);
			foreach (ItemRate ic in backingStep.Recipe.Products.Values)
			{
				AddItemRateControl(ic.Item, true);
			}
			foreach (ItemRate ic in backingStep.Recipe.Ingredients.Values)
			{
				AddItemRateControl(ic.Item, false);
			}
			RecipeLabel.Text = backingStep.Recipe.ToString(mainForm.Encoders, "{name} | {conversion} | {time} seconds");
			MultiplierNumberControl.SetNumber(BackingStep.Multiplier);
			UpdateNumerics();
			FinishInitialization();
			Disposed += OnDisposed;
			MultiplierNumberControl.AddNumberChangedListener(MultiplierValueChanged);
		}

		private void OnDisposed(object sender, EventArgs e)
		{
			BackingStep.SetControl(null);
		}

		public void UpdateNumerics()
		{
			ToggleInput(false);
			foreach (ItemRateControl irc in GetItemRateControls())
			{
				irc.UpdateRateValue(BackingStep.GetItemRate(irc.ItemUID, irc.IsProduct));
			}
			if (MultiplierNumberControl.GetNumber() != BackingStep.Multiplier)
			{
				MultiplierNumberControl.SetNumber(BackingStep.Multiplier);
			}
			MachineCountLabel.Text = MainForm.Encoders[BackingStep.Recipe.MachineUID].DisplayName + ": " + BackingStep.CalculateMachineCount() + " x " + BackingStep.CalculateMachineClockPercentage() + "%";
			double powerDraw = BackingStep.GetPowerDraw(MainForm.Encoders);
			PowerConsumptionLabel.Text = powerDraw > 0 ? $"Power Consumption: {powerDraw} MW" : $"Power Production: {-powerDraw} MW";
			ToggleInput(true);
		}

		private void RateChanged(string itemUID, RationalNumber newRate, bool isProduct)
		{
			if (BackingStep.GetItemRate(itemUID, isProduct).AbsoluteValue() != newRate)
			{
				BackingStep.SetMultiplier(BackingStep.CalculateMultiplierForRate(itemUID, newRate, isProduct));
			}
		}

		private void ItemClicked(string itemUID, bool isProduct)
		{
			if (MainForm.CurrentConnectionIRC != null)
			{
				if (itemUID == MainForm.CurrentConnectionIRC.ItemUID)
				{
					if (isProduct != MainForm.CurrentConnectionIRC.IsProduct)
					{
						Connection connection = isProduct ? BackingStep.HasProductConnectionFor(itemUID) ? BackingStep.GetProductConnection(itemUID) : new Connection(itemUID).AddProducer(BackingStep) : BackingStep.HasIngredientConnectionFor(itemUID) ? BackingStep.GetIngredientConnection(itemUID) : new Connection(itemUID).AddConsumer(BackingStep);
						connection.MergeWith(MainForm.CurrentConnectionFunc());
						MainForm.CurrentConnectionIRC = null;
						MainForm.CurrentConnectionFunc = null;
						MainForm.Plan.ProcessedPlan.Invalidate();
						MainForm.PlanUpdated();
					}
					else
					{
						goto Else;
					}
				}
				return;
			Else:
				MainForm.CurrentConnectionIRC.ItemButton.Enabled = true;
				MainForm.CurrentConnectionIRC = null;
				MainForm.CurrentConnectionFunc = null;
			}
			else if (MainForm.ControlKeyPressed)
			{
				MainForm.CurrentConnectionIRC = isProduct ? ProductRateControls[itemUID] : IngredientRateControls[itemUID];
				MainForm.CurrentConnectionFunc = () => isProduct ? BackingStep.HasProductConnectionFor(itemUID) ? BackingStep.GetProductConnection(itemUID) : new Connection(itemUID).AddProducer(BackingStep) : BackingStep.HasIngredientConnectionFor(itemUID) ? BackingStep.GetIngredientConnection(itemUID) : new Connection(itemUID).AddConsumer(BackingStep);
				MainForm.CurrentConnectionIRC.ItemButton.Enabled = false;
			}
			else
			{
				SelectRecipePrompt srp;
				if (isProduct)
				{
					srp = new SelectRecipePrompt(MainForm.Encoders.Recipes.GetRecipesThatConsume(itemUID));
				}
				else
				{
					srp = new SelectRecipePrompt(MainForm.Encoders.Recipes.GetRecipesThatProduce(itemUID));
				}
				if (srp.ShowDialog() == DialogResult.OK)
				{
					Step ps = new Step(srp.GetSelectedRecipe(), BackingStep, itemUID, isProduct);
					MainForm.Plan.Steps.Add(ps);
					MainForm.Plan.ProcessedPlan.Invalidate();
					MainForm.PlanUpdated();
				}
			}
		}

		private void AddItemRateControl(string itemUID, bool isProduct)
		{
			ItemRateControl irc = new ItemRateControl(MainForm, itemUID, BackingStep.GetItemRate(itemUID, isProduct), isProduct, 4, RateChanged, ItemClicked);
			if (isProduct)
			{
				irc.Anchor = AnchorStyles.Bottom;
				ProductsPanel.Controls.Add(irc);
				ProductRateControls.Add(itemUID, irc);
			}
			else
			{
				IngredientsPanel.Controls.Add(irc);
				IngredientRateControls.Add(itemUID, irc);
			}
			irc.FinishInitialization();
		}

		private List<ItemRateControl> GetItemRateControls()
		{
			List<ItemRateControl> list = new List<ItemRateControl>();
			list.AddRange(ProductRateControls.Values);
			list.AddRange(IngredientRateControls.Values);
			return list;
		}

		private void MultiplierValueChanged()
		{
			if (!MultiplierNumberControl.GetNumber().IsNonZero)
			{
				return;
			}
			if (Enabled && initialized)
			{
				BackingStep.SetMultiplier(MultiplierNumberControl.GetNumber());
				MainForm.UpdateTotalView();
			}
		}

		private void ToggleInput(bool on)
		{
			Enabled = on;
		}

		public void FinishInitialization()
		{
			initialized = true;
		}

		private void DeleteStepButton_Click(object sender, EventArgs e)
		{
			BackingStep.Delete(MainForm.Plan);
			MainForm.PlanUpdated();
		}

		private bool placed = false;

		public void Place(int xStart, int yStart)
		{
			if (placed)
			{
				throw new InvalidOperationException("Can't place a step control that has already been placed");
			}
			Location = new Point(xStart, yStart);
			placed = true;
		}
	}
}
