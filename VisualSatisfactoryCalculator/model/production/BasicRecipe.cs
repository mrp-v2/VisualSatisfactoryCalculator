using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VisualSatisfactoryCalculator.satisfactory.Numbers;

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
		public readonly RationalNumber time;
		/// <summary>
		/// The ingredients of the recipe.
		/// </summary>
		public readonly ImmutableDictionary<ItemType, RationalNumber> ingredients;
		/// <summary>
		/// The products of the recipe.
		/// </summary>
		public readonly ImmutableDictionary<ItemType, RationalNumber> products;

		public BasicRecipe(RationalNumber time, ImmutableDictionary<ItemType, RationalNumber> ingredients, ImmutableDictionary<ItemType, RationalNumber> products)
		{
			this.time = time;
			this.ingredients = ingredients;
			this.products = products;
		}

		public BasicRecipe(RationalNumber time, IEnumerable<ItemCount<ItemType>> ingredients, IEnumerable<ItemCount<ItemType>> products)
		{
			this.time = time;
			this.ingredients = ingredients.ToImmutableDictionary((rate) =>
			{
				return rate.item;
			}, (rate) =>
			{
				return rate.rate;
			});
			this.products = products.ToImmutableDictionary((rate) =>
			{
				return rate.item;
			}, (rate) =>
			{
				return rate.rate;
			});
		}

		public RationalNumber GetCount(ItemType item, bool isProduct)
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
