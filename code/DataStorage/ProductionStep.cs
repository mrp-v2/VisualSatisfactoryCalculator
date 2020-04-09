using System;
using System.Collections.Generic;
using VisualSatisfactoryCalculator.code.Extensions;
using VisualSatisfactoryCalculator.code.Interfaces;
using VisualSatisfactoryCalculator.controls.user;

namespace VisualSatisfactoryCalculator.code.DataStorage
{
	public class ProductionStep : IEquatable<ProductionStep>
	{
		private decimal multiplier;
		private readonly List<ProductionStep> childSteps;
		private readonly ProductionStep parentStep;
		private ProductionStepControl control;

		private readonly IRecipe recipe;

		public ProductionStep(IRecipe recipe, ProductionStep parent) : this(recipe, 1m)
		{
			parentStep = parent;
			UpdateMultiplierRelativeTo(parent);
		}

		public ProductionStep(IRecipe recipe, decimal multiplier)
		{
			this.recipe = recipe;
			this.multiplier = multiplier;
			childSteps = new List<ProductionStep>();
			control = default;
		}

		public IRecipe GetRecipe()
		{
			return recipe;
		}

		public int CalculateMachineCount()
		{
			return (int)Math.Ceiling(multiplier);
		}

		public void AddChildStep(ProductionStep child)
		{
			childSteps.Add(child);
		}

		private void SetMultiplier(decimal multiplier)
		{
			this.multiplier = multiplier;
			if (control != default(ProductionStepControl))
			{
				control.ToggleInput(false);
				control.MultiplierChanged();
				control.ToggleInput(true);
			}
		}

		public void SetMultiplierAndRelated(decimal multiplier)
		{
			SetMultiplier(multiplier);
			foreach (ProductionStep step in childSteps)
			{
				step.UpdateMultiplierFromParent();
			}
			if (parentStep != null) parentStep.UpdateMultiplierFromChild(this);
		}

		private void UpdateMultiplierFromChild(ProductionStep child)
		{
			if (!childSteps.Contains(child)) throw new ArgumentException("The given step is not actually a child!");
			UpdateMultiplierRelativeTo(child);
			foreach (ProductionStep childStep in childSteps)
			{
				if (!childStep.Equals(child))
				{
					childStep.UpdateMultiplierFromParent();
				}
			}
			if (parentStep != null) parentStep.UpdateMultiplierFromChild(this);
		}

		private void UpdateMultiplierFromParent()
		{
			UpdateMultiplierRelativeTo(parentStep);
			foreach (ProductionStep step in childSteps)
			{
				step.UpdateMultiplierFromParent();
			}
		}

		private void UpdateMultiplierRelativeTo(ProductionStep step)
		{
			IItem match = recipe.GetItemCounts().GetProducts().ToItems().FindMatch(step.recipe.GetItemCounts().GetIngredients().ToItems());
			if (match == null)
			{
				match = recipe.GetItemCounts().GetIngredients().ToItems().FindMatch(step.recipe.GetItemCounts().GetProducts().ToItems());
			}
			if (match == null) throw new ArgumentException("Could not find a similarity between the given step an this.");
			SetMultiplier(CalculateMultiplierForRate(match, step.CalculateDefaultItemRate(match) * step.multiplier));
		}

		private decimal CalculateDefaultItemRate(IItem item)
		{
			return 60m / recipe.GetCraftTime() * recipe.GetItemCounts().GetCountFor(item).GetCount();
		}

		public void SetMultiplierAndRelatedRelative(IItem item, decimal rate)
		{
			SetMultiplierAndRelated(CalculateMultiplierForRate(item, rate));
		}

		private decimal CalculateMultiplierForRate(IItem item, decimal rate)
		{
			return Math.Abs(rate / CalculateDefaultItemRate(item));
		}

		public decimal GetItemRate(IItem item)
		{
			return CalculateDefaultItemRate(item) * multiplier;
		}

		public void SetControl(ProductionStepControl control)
		{
			this.control = control;
		}

		public List<IItem> GetItemsWithRelatedStep()
		{
			List<IItem> items = new List<IItem>();
			foreach (ProductionStep child in childSteps)
			{
				items.AddRangeIfNew(child.recipe.GetItemCounts().GetIngredients().ToItems().FindMatches(recipe.GetItemCounts().GetProducts().ToItems()));
				items.AddRangeIfNew(child.recipe.GetItemCounts().GetProducts().ToItems().FindMatches(recipe.GetItemCounts().GetIngredients().ToItems()));
			}
			if (parentStep != null)
			{
				items.AddRangeIfNew(parentStep.recipe.GetItemCounts().GetIngredients().ToItems().FindMatches(recipe.GetItemCounts().GetProducts().ToItems()));
				items.AddRangeIfNew(parentStep.recipe.GetItemCounts().GetProducts().ToItems().FindMatches(recipe.GetItemCounts().GetIngredients().ToItems()));
			}
			return items;
		}

		public decimal GetMultiplier()
		{
			return multiplier;
		}

		protected List<ProductionStep> GetAllStepsRecursively()
		{
			List<ProductionStep> list = new List<ProductionStep>();
			foreach (ProductionStep child in childSteps)
			{
				list.Add(child);
				list.AddRange(child.GetAllStepsRecursively());
			}
			return list;
		}

		public override string ToString()
		{
			return recipe.ToString();
		}

		public List<ProductionStep> GetChildSteps()
		{
			return childSteps;
		}

		public bool Equals(ProductionStep obj)
		{
			return recipe.Equals(obj.recipe);
		}

		public override int GetHashCode()
		{
			return recipe.GetHashCode();
		}

		public void RemoveStep()
		{
			parentStep.childSteps.Remove(this);
		}
	}
}
