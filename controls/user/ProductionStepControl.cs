using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisualSatisfactoryCalculator.code;
using VisualSatisfactoryCalculator.forms;

namespace VisualSatisfactoryCalculator.controls.user
{
	public partial class ProductionStepControl : UserControl, IReceives<Recipe>
	{
		private readonly ProductionStep parentStep;
		private readonly MainForm mainForm;
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
			foreach (ItemCount ic in parentStep.GetItemCounts().GetProducts())
			{
				ProductsPanel.Controls.Add(new ItemRateControl(this, ic.ToItem(), parentStep.GetItemRate(ic)));
			}
			foreach (ItemCount ic in parentStep.GetItemCounts().GetIngredients())
			{
				IngredientsPanel.Controls.Add(new ItemRateControl(this, ic.ToItem(), parentStep.GetItemRate(ic)));
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
			MachineCountLabel.Text = parentStep.GetMachine() + ": " + parentStep.CalculateMachineCount();
		}

		public void RateChanged(Item item, decimal newRate)
		{
			if (Math.Abs(parentStep.GetItemRate(item)) != newRate)
			{
				parentStep.SetMultiplierAndRelatedRelative(item, newRate);
			}
		}

		public void ItemClicked(Item item)
		{
			if (parentStep.GetItemsWithRelatedStep().Contains(item))
			{

			}
			else
			{
				if (parentStep.GetProductItems().Contains(item))
				{
					new SelectRecipePrompt(mainForm.GetAllRecipes().GetRecipesThatConsume(item), this, null).ShowDialog();
				} else if (parentStep.GetIngredientItems().Contains(item))
				{
					new SelectRecipePrompt(mainForm.GetAllRecipes().GetRecipesThatProduce(item), this, null).ShowDialog();
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
			}
		}

		public void SendObject(Recipe recipe, string purpose)
		{
			ProductionStep ps = new ProductionStep(recipe, parentStep);
			parentStep.AddRelatedStep(ps);
			mainForm.AddProductionStep(ps);
		}

		public bool ItemHasRelatedRecipe(Item item)
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
