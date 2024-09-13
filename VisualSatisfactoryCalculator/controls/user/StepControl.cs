using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using VisualSatisfactoryCalculator.satisfactory.Extensions;
using VisualSatisfactoryCalculator.satisfactory.Numbers;
using VisualSatisfactoryCalculator.satisfactory.Production;
using VisualSatisfactoryCalculator.satisfactory.Utility;
using VisualSatisfactoryCalculator.forms;
using VisualSatisfactoryCalculator.model.production;
using VisualSatisfactoryCalculator.satisfactory.JSONClasses;

using Connection = VisualSatisfactoryCalculator.model.production.Connection<VisualSatisfactoryCalculator.satisfactory.JSONClasses.JSONItem, VisualSatisfactoryCalculator.satisfactory.Production.Step, VisualSatisfactoryCalculator.satisfactory.DataStorage.BasicRecipe>;

namespace VisualSatisfactoryCalculator.controls.user
{
	public partial class StepControl : UserControl, PlanLayoutMaker.ILayoutControl
	{
		public readonly Step backingStep;
		public readonly MainForm mainForm;
		private bool _initialized = false;
		public readonly Dictionary<JSONItem, ItemRateControl> productRateControls = new Dictionary<JSONItem, ItemRateControl>();
		public readonly Dictionary<JSONItem, ItemRateControl> ingredientRateControls = new Dictionary<JSONItem, ItemRateControl>();
		public StepControl TopControl { get { return this; } }

		public StepControl(Step backingStep, MainForm mainForm)
		{
			InitializeComponent();
			this.backingStep = backingStep;
			this.mainForm = mainForm;
			backingStep.SetControl(this);
			foreach (JSONItem item in backingStep.recipe.products.Keys)
			{
				AddItemRateControl(item, true);
			}
			foreach (JSONItem item in backingStep.recipe.ingredients.Keys)
			{
				AddItemRateControl(item, false);
			}
			RecipeLabel.Text = backingStep.recipe.ToString(mainForm.Encoders, "{name} | {conversion} | {time} seconds");
			MultiplierNumberControl.SetNumber(this.backingStep.Multiplier);
			UpdateNumerics();
			FinishInitialization();
			Disposed += OnDisposed;
			MultiplierNumberControl.AddNumberChangedListener(MultiplierValueChanged);
		}

		private void OnDisposed(object sender, EventArgs e)
		{
			backingStep.SetControl(null);
		}

		public void UpdateNumerics()
		{
			ToggleInput(false);
			foreach (ItemRateControl irc in GetItemRateControls())
			{
				irc.UpdateRateValue(backingStep.GetItemRate(irc.Item, irc.IsProduct));
			}
			if (MultiplierNumberControl.GetNumber() != backingStep.Multiplier)
			{
				MultiplierNumberControl.SetNumber(backingStep.Multiplier);
			}
			MachineCountLabel.Text = mainForm.Encoders[backingStep.Recipe.MachineUID].DisplayName + ": " + backingStep.CalculateMachineCount() + " x " + backingStep.CalculateMachineClockPercentage() + "%";
			double powerDraw = backingStep.GetPowerDraw(mainForm.Encoders);
			PowerConsumptionLabel.Text = powerDraw > 0 ? $"Power Consumption: {powerDraw} MW" : $"Power Production: {-powerDraw} MW";
			ToggleInput(true);
		}

		private void RateChanged(string itemUID, RationalNumber newRate, bool isProduct)
		{
			if (backingStep.GetItemRate(itemUID, isProduct).AbsoluteValue() != newRate)
			{
				backingStep.SetMultiplier(backingStep.CalculateMultiplierForRate(itemUID, newRate, isProduct));
			}
		}

		private void ItemClicked(string itemUID, bool isProduct)
		{
			if (mainForm.CurrentConnectionIRC != null)
			{
				if (itemUID == mainForm.CurrentConnectionIRC.Item)
				{
					if (isProduct != mainForm.CurrentConnectionIRC.IsProduct)
					{
						Connection connection = isProduct ? backingStep.HasProductConnectionFor(itemUID) ? backingStep.GetProductConnection(itemUID) : new Connection(itemUID).AddProducer(backingStep) : backingStep.HasIngredientConnectionFor(itemUID) ? backingStep.GetIngredientConnection(itemUID) : new Connection(itemUID).AddConsumer(backingStep);
						connection.MergeWith(mainForm.CurrentConnectionFunc());
						mainForm.CurrentConnectionIRC = null;
						mainForm.CurrentConnectionFunc = null;
						mainForm.Plan.processedPlan.Invalidate();
						mainForm.PlanUpdated();
					}
					else
					{
						goto Else;
					}
				}
				return;
			Else:
				mainForm.CurrentConnectionIRC.ItemButton.Enabled = true;
				mainForm.CurrentConnectionIRC = null;
				mainForm.CurrentConnectionFunc = null;
			}
			else if (mainForm.ControlKeyPressed)
			{
				mainForm.CurrentConnectionIRC = isProduct ? productRateControls[itemUID] : ingredientRateControls[itemUID];
				mainForm.CurrentConnectionFunc = () => isProduct ? backingStep.HasProductConnectionFor(itemUID) ? backingStep.GetProductConnection(itemUID) : new Connection(itemUID).AddProducer(backingStep) : backingStep.HasIngredientConnectionFor(itemUID) ? backingStep.GetIngredientConnection(itemUID) : new Connection(itemUID).AddConsumer(backingStep);
				mainForm.CurrentConnectionIRC.ItemButton.Enabled = false;
			}
			else
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
					mainForm.Plan.steps.Add(ps);
					mainForm.Plan.processedPlan.Invalidate();
					mainForm.PlanUpdated();
				}
			}
		}

		private void AddItemRateControl(string itemUID, bool isProduct)
		{
			ItemRateControl irc = new ItemRateControl(mainForm, itemUID, backingStep.GetItemRate(itemUID, isProduct), isProduct, 4, RateChanged, ItemClicked);
			if (isProduct)
			{
				irc.Anchor = AnchorStyles.Bottom;
				ProductsPanel.Controls.Add(irc);
				productRateControls.Add(itemUID, irc);
			}
			else
			{
				IngredientsPanel.Controls.Add(irc);
				ingredientRateControls.Add(itemUID, irc);
			}
			irc.FinishInitialization();
		}

		private List<ItemRateControl> GetItemRateControls()
		{
			List<ItemRateControl> list = new List<ItemRateControl>();
			list.AddRange(productRateControls.Values);
			list.AddRange(ingredientRateControls.Values);
			return list;
		}

		private void MultiplierValueChanged()
		{
			if (!MultiplierNumberControl.GetNumber().isNonZero)
			{
				return;
			}
			if (Enabled && _initialized)
			{
				backingStep.SetMultiplier(MultiplierNumberControl.GetNumber());
				mainForm.UpdateTotalView();
			}
		}

		private void ToggleInput(bool on)
		{
			Enabled = on;
		}

		public void FinishInitialization()
		{
			_initialized = true;
		}

		private void DeleteStepButton_Click(object sender, EventArgs e)
		{
			backingStep.Delete(mainForm.Plan);
			mainForm.PlanUpdated();
		}

		private bool _placed = false;

		public void Place(int xStart, int yStart)
		{
			if (_placed)
			{
				throw new InvalidOperationException("Can't place a step control that has already been placed");
			}
			Location = new Point(xStart, yStart);
			_placed = true;
		}
	}
}
