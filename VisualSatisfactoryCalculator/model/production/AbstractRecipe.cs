using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VisualSatisfactoryCalculator.satisfactory.Numbers;

namespace VisualSatisfactoryCalculator.model.production
{
	public abstract class AbstractRecipe<ItemType> where ItemType : AbstractItem
	{
		public readonly RationalNumber time;
		public readonly ImmutableDictionary<ItemType, RationalNumber> ingredients;
		public readonly ImmutableDictionary<ItemType, RationalNumber> products;

		protected AbstractRecipe(RationalNumber time, ImmutableDictionary<ItemType, RationalNumber> ingredients, ImmutableDictionary<ItemType, RationalNumber> products)
		{
			this.time = time;
			this.ingredients = ingredients;
			this.products = products;
		}

		protected AbstractRecipe(RationalNumber time, IEnumerable<ItemRate<ItemType>> ingredients, IEnumerable<ItemRate<ItemType>> products)
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
	}
}
