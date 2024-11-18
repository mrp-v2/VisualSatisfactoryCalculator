using System.Collections.Generic;
using System.Collections.Immutable;

namespace VisualSatisfactoryCalculator.model.production
{
	/// <summary>
	/// The basic implementation of a recipe. Extend this class to provide additional functionality.
	/// </summary>
	/// <typeparam name="ItemType"></typeparam>
	public class BasicRecipe<ItemType> where ItemType : BasicItem
	{
		/// <summary>
		/// How long the recipe takes.
		/// </summary>
		public readonly decimal time;
		/// <summary>
		/// The ingredients of the recipe.
		/// </summary>
		public readonly ImmutableDictionary<ItemType, decimal> ingredients;
		/// <summary>
		/// The products of the recipe.
		/// </summary>
		public readonly ImmutableDictionary<ItemType, decimal> products;

		public BasicRecipe(decimal time, ImmutableDictionary<ItemType, decimal> ingredients, ImmutableDictionary<ItemType, decimal> products)
		{
			this.time = time;
			this.ingredients = ingredients;
			this.products = products;
		}

		public decimal GetCount(ItemType item, bool isProduct)
		{
			if (isProduct)
			{
				return products[item];
			}
			else
			{
				return ingredients[item];
			}
		}
	}
}
