using System;
using System.Collections.Generic;
using System.Linq;

using VisualSatisfactoryCalculator.code.Interfaces;
using VisualSatisfactoryCalculator.code.Utility;
using VisualSatisfactoryCalculator.controls.user;

namespace VisualSatisfactoryCalculator.code.DataStorage
{
	public class ProductionStep : IEquatable<ProductionStep>
	{
		private decimal _multiplier;
		public Dictionary<ProductionStep, string> ChildIngredientSteps { get; }
		public Dictionary<ProductionStep, string> ChildProductSteps { get; }
		private readonly ProductionStep _parentStep;
		private ProductionStepControl _control;

		private readonly IRecipe _recipe;

		public bool IsProductOfParent { get; }

		public ProductionStep(IRecipe recipe, ProductionStep parent, string itemUID, bool isProductOfParent) : this(recipe, 1m)
		{
			_parentStep = parent;
			IsProductOfParent = isProductOfParent;
			if (isProductOfParent)
			{
				_parentStep.ChildProductSteps.Add(this, itemUID);
			}
			else
			{
				_parentStep.ChildIngredientSteps.Add(this, itemUID);
			}

			UpdateMultiplierRelativeTo(parent);
		}

		public ProductionStep(IRecipe recipe, decimal multiplier)
		{
			_recipe = recipe;
			_multiplier = multiplier;
			ChildIngredientSteps = new Dictionary<ProductionStep, string>();
			ChildProductSteps = new Dictionary<ProductionStep, string>();
			_control = default;
		}

		public IRecipe GetRecipe()
		{
			return _recipe;
		}

		public int CalculateMachineCount()
		{
			return (int)Math.Ceiling(_multiplier);
		}

		public decimal CalculateMachineClockPercentage()
		{
			return Math.Ceiling(_multiplier * (int)Math.Pow(10, Constants.CLOCK_DECIMALS + 2) / CalculateMachineCount()) / (int)Math.Pow(10, Constants.CLOCK_DECIMALS);
		}

		public void AddChildStep(ProductionStep child, string itemUID, bool isChildProduct)
		{
			if (isChildProduct)
			{
				ChildProductSteps.Add(child, itemUID);
			}
			else
			{
				ChildIngredientSteps.Add(child, itemUID);
			}
		}

		private void SetMultiplier(decimal multiplier)
		{
			_multiplier = multiplier;
			if (_control != default(ProductionStepControl))
			{
				_control.ToggleInput(false);
				_control.MultiplierChanged();
				_control.ToggleInput(true);
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
			if (_parentStep != null)
			{
				_parentStep.UpdateMultiplierFromChild(this);
			}
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
			if (_parentStep != null)
			{
				_parentStep.UpdateMultiplierFromChild(this);
			}
		}

		private void UpdateMultiplierFromParent()
		{
			UpdateMultiplierRelativeTo(_parentStep);
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
			if (ChildIngredientSteps.ContainsKey(step))
			{
				SetMultiplier(CalculateMultiplierForRate(ChildIngredientSteps[step], step.GetItemRate(ChildIngredientSteps[step], true), false));
			}
			else if (ChildProductSteps.ContainsKey(step))
			{
				SetMultiplier(CalculateMultiplierForRate(ChildProductSteps[step], step.GetItemRate(ChildProductSteps[step], false), true));
			}
			else if (step.ChildIngredientSteps.ContainsKey(this))
			{
				SetMultiplier(CalculateMultiplierForRate(step.ChildIngredientSteps[this], step.GetItemRate(step.ChildIngredientSteps[this], false), true));
			}
			else if (step.ChildProductSteps.ContainsKey(this))
			{
				SetMultiplier(CalculateMultiplierForRate(step.ChildProductSteps[this], step.GetItemRate(step.ChildProductSteps[this], true), false));
			}
			else
			{
				throw new ArgumentException("Could not find a similarity between the given step and this.");
			}
		}

		private decimal CalculateDefaultItemRate(string itemUID, bool isItemProduct)
		{
			return 60m / _recipe.CraftTime * _recipe.GetCountFor(itemUID, isItemProduct);
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
			return CalculateDefaultItemRate(itemUID, isItemProduct) * _multiplier;
		}

		public void SetControl(ProductionStepControl control)
		{
			_control = control;
		}

		public List<string> GetItemUIDsWithRelatedStep()
		{
			List<string> uids = new List<string>();
			uids.AddRange(ChildIngredientSteps.Values);
			uids.AddRange(ChildProductSteps.Values);
			if (_parentStep != null)
			{
				if (IsProductOfParent)
				{
					uids.Add(_parentStep.ChildProductSteps[this]);
				}
				else
				{
					uids.Add(_parentStep.ChildIngredientSteps[this]);
				}
			}
			return uids;
		}

		public decimal GetMultiplier()
		{
			return _multiplier;
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
			return _recipe.Equals(obj._recipe);
		}

		public override int GetHashCode()
		{
			return _recipe.GetHashCode();
		}

		public void RemoveStep()
		{
			if (IsProductOfParent)
			{
				_ = _parentStep.ChildProductSteps.Remove(this);
			}
			else
			{
				_ = _parentStep.ChildIngredientSteps.Remove(this);
			}
		}

		public decimal GetPowerDraw(Dictionary<string, IEncoder> encodings)
		{
			return (encodings[_recipe.MachineUID] as IBuilding).PowerConsumption * (decimal)Math.Pow((double)(CalculateMachineClockPercentage() / 100m), (double)(encodings[_recipe.MachineUID] as IBuilding).PowerConsumptionExponent) * CalculateMachineCount();
		}

		public decimal GetRecursivePowerDraw(Dictionary<string, IEncoder> encodings)
		{
			decimal total = GetPowerDraw(encodings);
			foreach (ProductionStep childIngredientStep in ChildIngredientSteps.Keys)
			{
				total += childIngredientStep.GetRecursivePowerDraw(encodings);
			}

			foreach (ProductionStep childProductStep in ChildProductSteps.Keys)
			{
				total += childProductStep.GetRecursivePowerDraw(encodings);
			}

			return total;
		}

		public bool IsChildStepIngredient(ProductionStep child)
		{
			if (!IsStepChild(child))
			{
				throw new ArgumentException("The given step was not a child!");
			}

			return _recipe.Ingredients.Keys.Contains(ChildIngredientSteps[child]);
		}

		public bool IsChildStepProduct(ProductionStep child)
		{
			if (!IsStepChild(child))
			{
				throw new ArgumentException("The given step was not a child!");
			}

			return _recipe.Products.Keys.Contains(ChildProductSteps[child]);
		}

		public bool IsStepChild(ProductionStep test)
		{
			return ChildIngredientSteps.ContainsKey(test) || ChildProductSteps.ContainsKey(test);
		}
	}
}
