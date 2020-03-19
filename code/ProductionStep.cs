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
		private double multiplier;
		private readonly List<ProductionStep> relatedSteps;
		private ProductionStepControl control;

		public ProductionStep(Recipe recipe, double multiplier) : base(recipe)
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

		public void SetMultiplier(double multiplier)
		{
			this.multiplier = multiplier;
			if (control != default(ProductionStepControl))
			{
				control.MultiplierChanged();
			}
		}

		private void SetMultiplierAndRelated(double multiplier)
		{
			SetMultiplier(multiplier);
			foreach (ProductionStep step in relatedSteps)
			{
				step.UpdateMultiplierAndRelated(this, this.multiplier);
			}
		}

		private void UpdateMultiplierAndRelated(ProductionStep caller, double callerMultiplier)
		{
			UpdateMultiplierRelativeTo(caller, callerMultiplier);
			foreach (ProductionStep step in relatedSteps)
			{
				if (!step.Equals(caller))
				{
					step.UpdateMultiplierAndRelated(this, multiplier);
				}
			}
		}

		private void UpdateMultiplierRelativeTo(ProductionStep origin, double originMultiplier)
		{
			Item match = itemCounts.FindMatch<Item>(origin.itemCounts);
			SetMultiplier(CalculateMultiplierForRate(match, origin.CalculateDefaultItemRate(match) * originMultiplier));
		}

		private double CalculateDefaultItemRate(Item item)
		{
			return 60.0 / GetCraftTime() * GetItemCount(item).GetCount();
		}

		public void SetMultiplierAndRelatedRelative(Item item, double rate)
		{
			SetMultiplierAndRelated(CalculateMultiplierForRate(item, rate));
		}

		private double CalculateMultiplierForRate(Item item, double rate)
		{
			return Math.Abs(rate / CalculateDefaultItemRate(item));
		}

		public double GetItemRate(Item item)
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

		public double GetMultiplier()
		{
			return multiplier;
		}
	}
}
