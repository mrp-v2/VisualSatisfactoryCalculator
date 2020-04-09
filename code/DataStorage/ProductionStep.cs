using System;
using System.Collections.Generic;
using VisualSatisfactoryCalculator.code.Extensions;
using VisualSatisfactoryCalculator.code.Interfaces;
using VisualSatisfactoryCalculator.code.JSONClasses;
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
			string matchUID = recipe.GetItemCounts().GetProducts().ToItemUIDs().FindMatch(step.recipe.GetItemCounts().GetIngredients().ToItemUIDs());
			if (matchUID == default)
			{
				matchUID = recipe.GetItemCounts().GetIngredients().ToItemUIDs().FindMatch(step.recipe.GetItemCounts().GetProducts().ToItemUIDs());
			}
			if (matchUID == null) throw new ArgumentException("Could not find a similarity between the given step an this.");
			SetMultiplier(CalculateMultiplierForRate(matchUID, step.CalculateDefaultItemRate(matchUID) * step.multiplier));
		}

		private decimal CalculateDefaultItemRate(string itemUID)
		{
			return 60m / recipe.GetCraftTime() * recipe.GetItemCounts().GetCountFor(itemUID).GetCount();
		}

		public void SetMultiplierAndRelatedRelative(string itemUID, decimal rate)
		{
			SetMultiplierAndRelated(CalculateMultiplierForRate(itemUID, rate));
		}

		private decimal CalculateMultiplierForRate(string itemUID, decimal rate)
		{
			return Math.Abs(rate / CalculateDefaultItemRate(itemUID));
		}

		public decimal GetItemRate(string itemUID)
		{
			return CalculateDefaultItemRate(itemUID) * multiplier;
		}

		public void SetControl(ProductionStepControl control)
		{
			this.control = control;
		}

		public List<string> GetItemUIDsWithRelatedStep()
		{
			List<string> items = new List<string>();
			foreach (ProductionStep child in childSteps)
			{
				items.AddRangeIfNew(child.recipe.GetItemCounts().GetIngredients().ToItemUIDs().FindMatches(recipe.GetItemCounts().GetProducts().ToItemUIDs()));
				items.AddRangeIfNew(child.recipe.GetItemCounts().GetProducts().ToItemUIDs().FindMatches(recipe.GetItemCounts().GetIngredients().ToItemUIDs()));
			}
			if (parentStep != null)
			{
				items.AddRangeIfNew(parentStep.recipe.GetItemCounts().GetIngredients().ToItemUIDs().FindMatches(recipe.GetItemCounts().GetProducts().ToItemUIDs()));
				items.AddRangeIfNew(parentStep.recipe.GetItemCounts().GetProducts().ToItemUIDs().FindMatches(recipe.GetItemCounts().GetIngredients().ToItemUIDs()));
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

		public decimal GetPowerDraw(List<IEncoder> encodings)
		{
			return (encodings.FindByID(recipe.GetMachineUID()) as IBuilding).GetPowerConsumption() * multiplier;
		}

		public decimal GetRecursivePowerDraw(List<IEncoder> encodings)
		{
			decimal total = GetPowerDraw(encodings);
			foreach (ProductionStep childStep in childSteps) total += childStep.GetRecursivePowerDraw(encodings);
			return total;
		}
	}
}
