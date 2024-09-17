using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VisualSatisfactoryCalculator.model.production;
using VisualSatisfactoryCalculator.satisfactory.Numbers;

namespace VisualSatisfactoryCalculator.satisfactory.model.production
{
	public class Recipe : BasicRecipe<Item>
	{
		public readonly string id;
		public readonly string displayName;
		public readonly string conversionString;
		public readonly Building building;

		public Recipe(string id, string displayName, string conversionString, RationalNumber time, Building building, ImmutableDictionary<Item, RationalNumber> ingredients, ImmutableDictionary<Item, RationalNumber> products) : base(time, ingredients, products)
		{
			this.id = id;
			this.displayName = displayName;
			this.conversionString = conversionString;
			this.building = building;
		}
	}
}
