using System.Collections.Immutable;

using VisualSatisfactoryCalculator.model.production;

namespace VisualSatisfactoryCalculator.satisfactory.model.production
{
	public class Recipe : BasicRecipe<Item>
	{
		public readonly string id;
		public readonly string displayName;
		public readonly string conversionString;
		public readonly Building building;

		public Recipe(string id, string displayName, string conversionString, decimal time, Building building, ImmutableDictionary<Item, decimal> ingredients, ImmutableDictionary<Item, decimal> products) : base(time, ingredients, products)
		{
			this.id = id;
			this.displayName = displayName;
			this.conversionString = conversionString;
			this.building = building;
		}

		public override string ToString()
		{
			return displayName + ": " + conversionString;
		}
	}
}
