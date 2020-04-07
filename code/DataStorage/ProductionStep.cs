using System;
using System.Collections.Generic;
using VisualSatisfactoryCalculator.code.Extensions;
using VisualSatisfactoryCalculator.code.JSONClasses;
using VisualSatisfactoryCalculator.controls.user;

namespace VisualSatisfactoryCalculator.code.DataStorage
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
			JSONItem match = itemCounts.GetProducts().FindMatch(origin.itemCounts.GetIngredients(), JSONItem.comparer);
			if (match == null)
			{
				match = itemCounts.GetIngredients().FindMatch(origin.itemCounts.GetProducts(), JSONItem.comparer);
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
				items.AddRangeIfNew(step.itemCounts.GetIngredients().FindMatches(itemCounts.GetProducts(), JSONItem.comparer));
				items.AddRangeIfNew(step.itemCounts.GetProducts().FindMatches(itemCounts.GetIngredients(), JSONItem.comparer));
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
					if (step.itemCounts.GetIngredients().ContainsAny(itemCounts.GetProducts(), JSONItem.comparer))
					{
						if (!tiers.ContainsKey(above))
						{
							tiers.Add(above, new List<ProductionStep>());
						}
						tiers[above].Add(step);
						tiers.AddRange(step.GetRelativeTiersRecursively(this, above));
					}
					else if (step.itemCounts.GetProducts().ContainsAny(itemCounts.GetIngredients(), JSONItem.comparer))
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
