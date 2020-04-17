using System;
using System.Collections.Generic;
using System.Linq;
using VisualSatisfactoryCalculator.code.Extensions;
using VisualSatisfactoryCalculator.code.Interfaces;
using VisualSatisfactoryCalculator.controls.user;

namespace VisualSatisfactoryCalculator.code.DataStorage
{
	public class ProductionStep : IEquatable<ProductionStep>
	{
		private decimal multiplier;
		private readonly Dictionary<ProductionStep, string> childSteps;
		private readonly ProductionStep parentStep;
		private ProductionStepControl control;

		private readonly IRecipe recipe;

		public ProductionStep(IRecipe recipe, ProductionStep parent, string itemUID) : this(recipe, 1m)
		{
			parentStep = parent;
			parentStep.childSteps.Add(this, itemUID);
			UpdateMultiplierRelativeTo(parent);
		}

		public ProductionStep(IRecipe recipe, decimal multiplier)
		{
			this.recipe = recipe;
			this.multiplier = multiplier;
			childSteps = new Dictionary<ProductionStep, string>();
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

		public void AddChildStep(ProductionStep child, string itemUID)
		{
			childSteps.Add(child, itemUID);
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
			foreach (ProductionStep step in childSteps.Keys)
			{
				step.UpdateMultiplierFromParent();
			}
			if (parentStep != null) parentStep.UpdateMultiplierFromChild(this);
		}

		private void UpdateMultiplierFromChild(ProductionStep child)
		{
			if (!childSteps.ContainsKey(child)) throw new ArgumentException("The given step is not actually a child!");
			UpdateMultiplierRelativeTo(child);
			foreach (ProductionStep childStep in childSteps.Keys)
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
			foreach (ProductionStep step in childSteps.Keys)
			{
				step.UpdateMultiplierFromParent();
			}
		}

		private void UpdateMultiplierRelativeTo(ProductionStep step)
		{
			if (childSteps.ContainsKey(step)) SetMultiplier(CalculateMultiplierForRate(childSteps[step], step.CalculateDefaultItemRate(childSteps[step]) * step.multiplier));
			else if (step.childSteps.ContainsKey(this)) SetMultiplier(CalculateMultiplierForRate(step.childSteps[this], step.CalculateDefaultItemRate(step.childSteps[this]) * step.multiplier));
			else throw new ArgumentException("Could not find a similarity between the given step an this.");
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
			List<string> uids = childSteps.Values.ToList();
			if (parentStep != null)
			{
				uids.Add(parentStep.childSteps[this]);
			}
			return uids;
		}

		public decimal GetMultiplier()
		{
			return multiplier;
		}

		protected List<ProductionStep> GetAllStepsRecursively()
		{
			List<ProductionStep> list = new List<ProductionStep>();
			foreach (ProductionStep child in childSteps.Keys)
			{
				list.Add(child);
				list.AddRange(child.GetAllStepsRecursively());
			}
			return list;
		}

		public Dictionary<ProductionStep, string> GetChildSteps()
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
			if (!parentStep.childSteps.ContainsKey(this)) throw new InvalidOperationException("This is not a child of its parent!");
			parentStep.childSteps.Remove(this);
		}

		public decimal GetPowerDraw(List<IEncoder> encodings)
		{
			return (encodings.FindByID(recipe.GetMachineUID()) as IBuilding).GetPowerConsumption() * multiplier;
		}

		public decimal GetRecursivePowerDraw(List<IEncoder> encodings)
		{
			decimal total = GetPowerDraw(encodings);
			foreach (ProductionStep childStep in childSteps.Keys) total += childStep.GetRecursivePowerDraw(encodings);
			return total;
		}

		public bool IsChildStepIngredient(ProductionStep child)
		{
			if (!IsStepChild(child)) throw new ArgumentException("The given step was not a child!");
			return recipe.GetItemCounts().GetIngredients().ToItemUIDs().Contains(childSteps[child]);
		}

		public bool IsChildStepProduct(ProductionStep child)
		{
			if (!IsStepChild(child)) throw new ArgumentException("The given step was not a child!");
			return recipe.GetItemCounts().GetProducts().ToItemUIDs().Contains(childSteps[child]);
		}

		public bool IsStepChild(ProductionStep test)
		{
			return childSteps.ContainsKey(test);
		}
	}
}
