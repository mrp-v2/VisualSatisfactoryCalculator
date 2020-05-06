using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using VisualSatisfactoryCalculator.code.Extensions;
using VisualSatisfactoryCalculator.code.Interfaces;
using VisualSatisfactoryCalculator.controls.user;

namespace VisualSatisfactoryCalculator.code.DataStorage
{
	public class ProductionStep : IEquatable<ProductionStep>
	{
		private decimal multiplier;
		public Dictionary<ProductionStep, string> ChildIngredientSteps { get; }
		public Dictionary<ProductionStep, string> ChildProductSteps { get; }
		private readonly ProductionStep parentStep;
		private ProductionStepControl control;

		private readonly IRecipe recipe;

		public bool IsProductOfParent { get; }

		public ProductionStep(IRecipe recipe, ProductionStep parent, string itemUID, bool isProductOfParent) : this(recipe, 1m)
		{
			parentStep = parent;
			IsProductOfParent = isProductOfParent;
			if (isProductOfParent) parentStep.ChildProductSteps.Add(this, itemUID);
			else parentStep.ChildIngredientSteps.Add(this, itemUID);
			UpdateMultiplierRelativeTo(parent);
		}

		public ProductionStep(IRecipe recipe, decimal multiplier)
		{
			this.recipe = recipe;
			this.multiplier = multiplier;
			ChildIngredientSteps = new Dictionary<ProductionStep, string>();
			ChildProductSteps = new Dictionary<ProductionStep, string>();
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

		public void AddChildStep(ProductionStep child, string itemUID, bool isChildProduct)
		{
			if (isChildProduct) ChildProductSteps.Add(child, itemUID);
			else ChildIngredientSteps.Add(child, itemUID);
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
			foreach (ProductionStep step in ChildIngredientSteps.Keys)
			{
				step.UpdateMultiplierFromParent();
			}
			foreach (ProductionStep step in ChildProductSteps.Keys)
			{
				step.UpdateMultiplierFromParent();
			}
			if (parentStep != null) parentStep.UpdateMultiplierFromChild(this);
		}

		private void UpdateMultiplierFromChild(ProductionStep child)
		{
			UpdateMultiplierRelativeTo(child);
			foreach (ProductionStep childStep in ChildIngredientSteps.Keys)
			{
				if (!childStep.Equals(child))
				{
					childStep.UpdateMultiplierFromParent();
				}
			}
			foreach (ProductionStep childStep in ChildProductSteps.Keys)
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
			foreach (ProductionStep step in ChildIngredientSteps.Keys)
			{
				step.UpdateMultiplierFromParent();
			}
			foreach (ProductionStep step in ChildProductSteps.Keys)
			{
				step.UpdateMultiplierFromParent();
			}
		}

		private void UpdateMultiplierRelativeTo(ProductionStep step)
		{
			if (ChildIngredientSteps.ContainsKey(step)) SetMultiplier(CalculateMultiplierForRate(ChildIngredientSteps[step], step.GetItemRate(ChildIngredientSteps[step], true), false));
			else if (ChildProductSteps.ContainsKey(step)) SetMultiplier(CalculateMultiplierForRate(ChildProductSteps[step], step.GetItemRate(ChildProductSteps[step], false), true));
			else if (step.ChildIngredientSteps.ContainsKey(this)) SetMultiplier(CalculateMultiplierForRate(step.ChildIngredientSteps[this], step.GetItemRate(step.ChildIngredientSteps[this], false), true));
			else if (step.ChildProductSteps.ContainsKey(this)) SetMultiplier(CalculateMultiplierForRate(step.ChildProductSteps[this], step.GetItemRate(step.ChildProductSteps[this], true), false));
			else throw new ArgumentException("Could not find a similarity between the given step and this.");
		}

		private decimal CalculateDefaultItemRate(string itemUID, bool isItemProduct)
		{
			return 60m / recipe.CraftTime * recipe.GetCountFor(itemUID, isItemProduct);
		}

		public void SetMultiplierAndRelatedRelative(string itemUID, decimal rate, bool isItemProduct)
		{
			SetMultiplierAndRelated(CalculateMultiplierForRate(itemUID, rate, isItemProduct));
		}

		private decimal CalculateMultiplierForRate(string itemUID, decimal rate, bool isItemProduct)
		{
			return Math.Abs(rate / CalculateDefaultItemRate(itemUID, isItemProduct));
		}

		public decimal GetItemRate(string itemUID, bool isItemProduct)
		{
			return CalculateDefaultItemRate(itemUID, isItemProduct) * multiplier;
		}

		public void SetControl(ProductionStepControl control)
		{
			this.control = control;
		}

		public List<string> GetItemUIDsWithRelatedStep()
		{
			List<string> uids = new List<string>();
			uids.AddRange(ChildIngredientSteps.Values);
			uids.AddRange(ChildProductSteps.Values);
			if (parentStep != null)
			{
				if (IsProductOfParent) uids.Add(parentStep.ChildProductSteps[this]);
				else uids.Add(parentStep.ChildIngredientSteps[this]);
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
			foreach (ProductionStep child in ChildIngredientSteps.Keys)
			{
				list.Add(child);
				list.AddRange(child.GetAllStepsRecursively());
			}
			foreach (ProductionStep child in ChildProductSteps.Keys)
			{
				list.Add(child);
				list.AddRange(child.GetAllStepsRecursively());
			}
			return list;
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
			if (IsProductOfParent)
			{
				parentStep.ChildProductSteps.Remove(this);
			}
			else
			{
				parentStep.ChildIngredientSteps.Remove(this);
			}
		}

		public decimal GetPowerDraw(Dictionary<string, IEncoder> encodings)
		{
			return (encodings[recipe.MachineUID] as IBuilding).PowerConsumption * multiplier;
		}

		public decimal GetRecursivePowerDraw(Dictionary<string, IEncoder> encodings)
		{
			decimal total = GetPowerDraw(encodings);
			foreach (ProductionStep childIngredientStep in ChildIngredientSteps.Keys) total += childIngredientStep.GetRecursivePowerDraw(encodings);
			foreach (ProductionStep childProductStep in ChildProductSteps.Keys) total += childProductStep.GetRecursivePowerDraw(encodings);
			return total;
		}

		public bool IsChildStepIngredient(ProductionStep child)
		{
			if (!IsStepChild(child)) throw new ArgumentException("The given step was not a child!");
			return recipe.Ingredients.Keys.Contains(ChildIngredientSteps[child]);
		}

		public bool IsChildStepProduct(ProductionStep child)
		{
			if (!IsStepChild(child)) throw new ArgumentException("The given step was not a child!");
			return recipe.Products.Keys.Contains(ChildProductSteps[child]);
		}

		public bool IsStepChild(ProductionStep test)
		{
			return ChildIngredientSteps.ContainsKey(test) || ChildProductSteps.ContainsKey(test);
		}
	}
}
