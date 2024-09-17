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
			MachineCountNumeric.Value = this.backingStep.MachineCount;
			ClockSpeedNumeric.Value = this.backingStep.ClockSpeedDecimal / 1000m;
			UpdateNumerics();
			FinishInitialization();
			Disposed += OnDisposed;
			MachineCountNumeric.ValueChanged += MachineCountValueChanged;
			ClockSpeedNumeric.ValueChanged += ClockSpeedValueChanged;
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
				irc.UpdateRateValue(backingStep.GetRate(irc.Item, irc.IsProduct));
			}
			if (MachineCountNumeric.Value != backingStep.MachineCount)
			{
				MachineCountNumeric.Value = backingStep.MachineCount;
			}
			if (ClockSpeedNumeric.Value != backingStep.ClockSpeedDecimal / 1000m)
			{
				ClockSpeedNumeric.Value = backingStep.ClockSpeedDecimal / 1000m;
			}
			double powerDraw = backingStep.GetPowerDraw(mainForm.Encoders);
			PowerConsumptionLabel.Text = powerDraw > 0 ? $"Power Consumption: {powerDraw} MW" : $"Power Production: {-powerDraw} MW";
			ToggleInput(true);
		}

		private void RateChanged(JSONItem item, RationalNumber oldRate, RationalNumber newRate, bool isProduct)
		{
			if (backingStep.GetRate(item, isProduct).AbsoluteValue() != newRate)
			{
				backingStep.CascadingUpdateRatesFrom(new ItemCount<JSONItem>(item, newRate), isProduct);
			}
		}

		private void ItemClicked(JSONItem item, bool isProduct)
		{
			if (mainForm.currentConnectionIRC != null)
			{
				throw new NotImplementedException();
				if (item == mainForm.currentConnectionIRC.Item)
				{
					if (isProduct != mainForm.currentConnectionIRC.IsProduct)
					{
						Connection connection = isProduct ? backingStep.HasProductConnectionFor(item) ? backingStep.GetProductConnection(item) : new Connection(item).AddProducer(backingStep) : backingStep.HasIngredientConnectionFor(item) ? backingStep.GetIngredientConnection(item) : new Connection(item).AddConsumer(backingStep);
						//connection.MergeWith(mainForm.CurrentConnectionFunc());
						mainForm.currentConnectionIRC = null;
						mainForm.currentConnectionFunc = null;
						mainForm.plan.processedPlan.Invalidate();
						mainForm.PlanUpdated();
					}
					else
					{
						goto Else;
					}
				}
				return;
			Else:
				mainForm.currentConnectionIRC.ItemButton.Enabled = true;
				mainForm.currentConnectionIRC = null;
				mainForm.currentConnectionFunc = null;
			}
			else if (mainForm.ControlKeyPressed)
			{
				throw new NotImplementedException();
				mainForm.currentConnectionIRC = isProduct ? productRateControls[item] : ingredientRateControls[item];
				mainForm.currentConnectionFunc = () => isProduct ? backingStep.HasProductConnectionFor(item) ? backingStep.GetProductConnection(item) : new Connection(item).AddProducer(backingStep) : backingStep.HasIngredientConnectionFor(item) ? backingStep.GetIngredientConnection(item) : new Connection(item).AddConsumer(backingStep);
				mainForm.currentConnectionIRC.ItemButton.Enabled = false;
			}
			else
			{
				SelectRecipePrompt srp;
				if (isProduct)
				{
					srp = new SelectRecipePrompt(mainForm.Encoders.Recipes.GetRecipesThatConsume(item));
				}
				else
				{
					srp = new SelectRecipePrompt(mainForm.Encoders.Recipes.GetRecipesThatProduce(item));
				}
				if (srp.ShowDialog() == DialogResult.OK)
				{
					Step ps = new Step(srp.GetSelectedRecipe(), backingStep, item, isProduct);
					mainForm.plan.steps.Add(ps);
					mainForm.plan.processedPlan.Invalidate();
					mainForm.PlanUpdated();
				}
			}
		}

		private void AddItemRateControl(JSONItem item, bool isProduct)
		{
			ItemRateControl irc = new ItemRateControl(mainForm, item, backingStep.GetRate(item, isProduct), isProduct, 4, RateChanged, ItemClicked);
			if (isProduct)
			{
				irc.Anchor = AnchorStyles.Bottom;
				ProductsPanel.Controls.Add(irc);
				productRateControls.Add(item, irc);
			}
			else
			{
				IngredientsPanel.Controls.Add(irc);
				ingredientRateControls.Add(item, irc);
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

		private void MachineCountValueChanged(object sender, EventArgs args)
		{
			if (Enabled && _initialized)
			{
				backingStep.SetMachineCount((uint)MachineCountNumeric.Value);
				mainForm.UpdateTotalView();
			}
		}

		private void ClockSpeedValueChanged(object sender, EventArgs args)
		{
			if (Enabled && _initialized)
			{
				backingStep.SetClockSpeedThousandths((ushort)(ClockSpeedNumeric.Value * Constants.CLOCK_SPEED_DECIMAL_FACTOR));
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
			backingStep.Delete(mainForm.plan);
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
