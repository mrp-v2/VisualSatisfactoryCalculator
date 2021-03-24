using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using VisualSatisfactoryCalculator.code.DataStorage;
using VisualSatisfactoryCalculator.code.Extensions;
using VisualSatisfactoryCalculator.code.Production;
using VisualSatisfactoryCalculator.code.Utility;
using VisualSatisfactoryCalculator.forms;

namespace VisualSatisfactoryCalculator.controls.user
{
	public partial class StepControl : UserControl, PlanLayoutMaker.ILayoutControl
	{
		public readonly Step BackingStep;
		public readonly MainForm mainForm;
		private bool initialized = false;
		public readonly Dictionary<string, ItemRateControl> ProductRateControls = new Dictionary<string, ItemRateControl>();
		public readonly Dictionary<string, ItemRateControl> IngredientRateControls = new Dictionary<string, ItemRateControl>();
		public StepControl TopControl { get { return this; } }

		public StepControl(Step backingStep, MainForm mainForm)
		{
			InitializeComponent();
			BackingStep = backingStep;
			this.mainForm = mainForm;
			backingStep.SetControl(this);
			foreach (ItemCount ic in backingStep.Recipe.Products.Values)
			{
				AddItemRateControl(ic.ItemUID, true);
			}
			foreach (ItemCount ic in backingStep.Recipe.Ingredients.Values)
			{
				AddItemRateControl(ic.ItemUID, false);
			}
			RecipeLabel.Text = backingStep.Recipe.ToString(mainForm.Encoders, "{name} | {conversion} | {time} seconds");
			UpdateNumerics();
			FinishInitialization();
		}

		public void UpdateNumerics()
		{
			ToggleInput(false);
			foreach (ItemRateControl irc in GetItemRateControls())
			{
				irc.UpdateRateValue(BackingStep.GetItemRate(irc.ItemUID, irc.IsProduct));
			}
			if (MultiplierNumeric.Value != BackingStep.GetMultiplier())
			{
				MultiplierNumeric.Value = BackingStep.GetMultiplier();
			}
			MachineCountLabel.Text = mainForm.Encoders[BackingStep.Recipe.MachineUID].DisplayName + ": " + BackingStep.CalculateMachineCount() + " x " + BackingStep.CalculateMachineClockPercentage() + "%";
			PowerConsumptionLabel.Text = "Power Consumption: " + BackingStep.GetPowerDraw(mainForm.Encoders).ToPrettyString() + "MW";
			ToggleInput(true);
		}

		public void RateChanged(string itemUID, decimal newRate, bool isProduct)
		{
			if (Math.Abs(BackingStep.GetItemRate(itemUID, isProduct)) != newRate)
			{
				// TODO
			}
		}

		public void ItemClicked(string itemUID, bool isProduct)
		{
			if (!BackingStep.GetItemUIDsWithRelatedStep().Contains(itemUID))
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
					Step ps = new Step(srp.GetSelectedRecipe(), BackingStep, itemUID, isProduct);
					mainForm.Plan.Steps.Add(ps);
					mainForm.Plan.ProcessedPlan.Invalidate();
					mainForm.PlanUpdated();
				}
			}
		}

		private void AddItemRateControl(string itemUID, bool isProduct)
		{
			ItemRateControl irc = new ItemRateControl(this, itemUID, BackingStep.GetItemRate(itemUID, isProduct), isProduct);
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

		private void MultiplierNumeric_ValueChanged(object sender, EventArgs e)
		{
			if (MultiplierNumeric.Value == 0)
			{
				return;
			}
			if (Enabled && initialized)
			{
				BackingStep.SetMultiplier(MultiplierNumeric.Value);
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
			BackingStep.Delete(mainForm.Plan);
			mainForm.PlanUpdated();
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
