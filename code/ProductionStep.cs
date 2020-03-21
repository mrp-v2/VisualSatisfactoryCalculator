using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSatisfactoryCalculator.controls.user;

namespace VisualSatisfactoryCalculator.code
{
	public class ProductionStep : Recipe
	{
		private decimal multiplier;
		private readonly List<ProductionStep> relatedSteps;
		private ProductionStepControl control;

		public ProductionStep(Recipe recipe, ProductionStep related) : this(recipe, 1m)
		{
			relatedSteps.Add(related);
			UpdateMultiplierRelativeTo(related);
		}

		public ProductionStep(Recipe recipe, decimal multiplier) : base(recipe)
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
			Item match = itemCounts.GetProducts().GetItems().FindMatch(origin.itemCounts.GetIngredients().GetItems());
			if (match == null)
			{
				match = itemCounts.GetIngredients().GetItems().FindMatch(origin.itemCounts.GetProducts().GetItems());
			}
			SetMultiplier(CalculateMultiplierForRate(match, origin.CalculateDefaultItemRate(match) * origin.multiplier));
		}

		private decimal CalculateDefaultItemRate(Item item)
		{
			return 60m / GetCraftTime() * GetItemCount(item).GetCount();
		}

		public void SetMultiplierAndRelatedRelative(Item item, decimal rate)
		{
			SetMultiplierAndRelated(CalculateMultiplierForRate(item, rate));
		}

		private decimal CalculateMultiplierForRate(Item item, decimal rate)
		{
			return Math.Abs(rate / CalculateDefaultItemRate(item));
		}

		public decimal GetItemRate(Item item)
		{
			return CalculateDefaultItemRate(item) * multiplier;
		}

		public void SetControl(ProductionStepControl control)
		{
			this.control = control;
		}

		public List<Item> GetItemsWithRelatedStep()
		{
			List<Item> items = new List<Item>();
			foreach (ProductionStep step in relatedSteps)
			{
				items.AddRangeIfNew(step.GetIngredientItems().FindMatches(GetProductItems()));
				items.AddRangeIfNew(step.GetProductItems().FindMatches(GetIngredientItems()));
			}
			return items;
		}

		public decimal GetMultiplier()
		{
			return multiplier;
		}
	}
}
