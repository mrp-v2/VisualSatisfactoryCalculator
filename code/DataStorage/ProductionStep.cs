using System;
using System.Collections.Generic;
using VisualSatisfactoryCalculator.code.Extensions;
using VisualSatisfactoryCalculator.code.Interfaces;
using VisualSatisfactoryCalculator.code.JSONClasses;
using VisualSatisfactoryCalculator.controls.user;

namespace VisualSatisfactoryCalculator.code.DataStorage
{
	public class ProductionStep
	{
		private decimal multiplier;
		private readonly List<ProductionStep> relatedSteps;
		private ProductionStepControl control;

		private readonly IRecipe recipe;

		public ProductionStep(IRecipe recipe, ProductionStep related) : this(recipe, 1m)
		{
			relatedSteps.Add(related);
			UpdateMultiplierRelativeTo(related);
		}

		public ProductionStep(IRecipe recipe, decimal multiplier)
		{
			this.recipe = recipe;
			this.multiplier = multiplier;
			relatedSteps = new List<ProductionStep>();
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

		public void AddRelatedStep(ProductionStep step)
		{
			relatedSteps.Add(step);
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
			foreach (ProductionStep step in relatedSteps)
			{
				step.UpdateMultiplierAndRelated(this);
			}
		}

		private void UpdateMultiplierAndRelated(ProductionStep caller)
		{
			UpdateMultiplierRelativeTo(caller);
			foreach (ProductionStep step in relatedSteps)
			{
				if (!step.Equals(caller))
				{
					step.UpdateMultiplierAndRelated(this);
				}
			}
		}

		private void UpdateMultiplierRelativeTo(ProductionStep origin)
		{
			IItem match = recipe.GetItemCounts().GetProducts().ToItems().FindMatch(origin.recipe.GetItemCounts().GetIngredients().ToItems());
			if (match == null)
			{
				match = recipe.GetItemCounts().GetIngredients().ToItems().FindMatch(origin.recipe.GetItemCounts().GetProducts().ToItems());
			}
			SetMultiplier(CalculateMultiplierForRate(match, origin.CalculateDefaultItemRate(match) * origin.multiplier));
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
			foreach (ProductionStep step in relatedSteps)
			{
				items.AddRangeIfNew(step.recipe.GetItemCounts().GetIngredients().ToItems().FindMatches(recipe.GetItemCounts().GetProducts().ToItems()));
				items.AddRangeIfNew(step.recipe.GetItemCounts().GetProducts().ToItems().FindMatches(recipe.GetItemCounts().GetIngredients().ToItems()));
			}
			return items;
		}

		public decimal GetMultiplier()
		{
			return multiplier;
		}

		protected Dictionary<sbyte, List<ProductionStep>> GetRelativeTiersRecursively(ProductionStep relativeTo, sbyte origin)
		{
			Dictionary<sbyte, List<ProductionStep>> tiers = new Dictionary<sbyte, List<ProductionStep>>();
			sbyte above = origin, below = origin;
			above -= 1;
			below += 1;
			foreach (ProductionStep step in relatedSteps)
			{
				if (!step.Equals(relativeTo))
				{
					if (step.recipe.GetItemCounts().GetIngredients().ToItems().ContainsAny(recipe.GetItemCounts().GetProducts().ToItems()))
					{
						if (!tiers.ContainsKey(above))
						{
							tiers.Add(above, new List<ProductionStep>());
						}
						tiers[above].Add(step);
						tiers.AddRange(step.GetRelativeTiersRecursively(this, above));
					}
					else if (step.recipe.GetItemCounts().GetProducts().ToItems().ContainsAny(recipe.GetItemCounts().GetIngredients().ToItems()))
					{
						if (!tiers.ContainsKey(below))
						{
							tiers.Add(below, new List<ProductionStep>());
						}
						tiers[below].Add(step);
						tiers.AddRange(step.GetRelativeTiersRecursively(this, below));
					}
					else
					{
						throw new ArgumentException("Unable to find a product/ingredient relationship between this and the given step.");
					}
				}
			}
			return tiers;
		}

		protected List<ProductionStep> GetAllStepsRecursively(ProductionStep origin)
		{
			List<ProductionStep> list = new List<ProductionStep>();
			foreach (ProductionStep step in relatedSteps)
			{
				if (!step.Equals(origin))
				{
					list.Add(step);
					list.AddRange(step.GetAllStepsRecursively(this));
				}
			}
			return list;
		}
	}
}
