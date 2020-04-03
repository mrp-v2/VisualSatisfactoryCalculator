using System;
using System.Collections.Generic;
using VisualSatisfactoryCalculator.controls.user;

namespace VisualSatisfactoryCalculator.code
{
	public class ProductionStep : JSONRecipe
	{
		private decimal multiplier;
		private readonly List<ProductionStep> relatedSteps;
		private ProductionStepControl control;

		public ProductionStep(JSONRecipe recipe, ProductionStep related) : this(recipe, 1m)
		{
			relatedSteps.Add(related);
			UpdateMultiplierRelativeTo(related);
		}

		public ProductionStep(JSONRecipe recipe, decimal multiplier) : base(recipe)
		{
			this.multiplier = multiplier;
			relatedSteps = new List<ProductionStep>();
			control = default;
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
			JSONItem match = itemCounts.GetProducts().FindMatch(origin.itemCounts.GetIngredients(), JSONItem.blank);
			if (match == null)
			{
				match = itemCounts.GetIngredients().FindMatch(origin.itemCounts.GetProducts(), JSONItem.blank);
			}
			SetMultiplier(CalculateMultiplierForRate(match, origin.CalculateDefaultItemRate(match) * origin.multiplier));
		}

		private decimal CalculateDefaultItemRate(JSONItem item)
		{
			return 60m / craftTime * itemCounts.GetCountFor(item).GetCount();
		}

		public void SetMultiplierAndRelatedRelative(JSONItem item, decimal rate)
		{
			SetMultiplierAndRelated(CalculateMultiplierForRate(item, rate));
		}

		private decimal CalculateMultiplierForRate(JSONItem item, decimal rate)
		{
			return Math.Abs(rate / CalculateDefaultItemRate(item));
		}

		public decimal GetItemRate(JSONItem item)
		{
			return CalculateDefaultItemRate(item) * multiplier;
		}

		public void SetControl(ProductionStepControl control)
		{
			this.control = control;
		}

		public List<JSONItem> GetItemsWithRelatedStep()
		{
			List<JSONItem> items = new List<JSONItem>();
			foreach (ProductionStep step in relatedSteps)
			{
				items.AddRangeIfNew(step.itemCounts.GetIngredients().FindMatches(itemCounts.GetProducts(), JSONItem.blank));
				items.AddRangeIfNew(step.itemCounts.GetProducts().FindMatches(itemCounts.GetIngredients(), JSONItem.blank));
			}
			return items;
		}

		public decimal GetMultiplier()
		{
			return multiplier;
		}
	}
}
