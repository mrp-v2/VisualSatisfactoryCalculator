using System.Collections.Immutable;

using VisualSatisfactoryCalculator.satisfactory.model.production;

namespace VisualSatisfactoryCalculator.code.Utility
{
	public class Encodings
	{
		public readonly ImmutableDictionary<string, Item> items;
		public readonly ImmutableHashSet<Item> resourceItems;
		public readonly ImmutableDictionary<string, Recipe> recipes;
		public readonly ImmutableDictionary<string, Building> buildings;

		public Encodings(ImmutableDictionary<string, Item> items, ImmutableHashSet<Item> resourceItems, ImmutableDictionary<string, Recipe> recipes, ImmutableDictionary<string, Building> buildings)
		{
			this.items = items;
			this.resourceItems = resourceItems;
			this.recipes = recipes;
			this.buildings = buildings;
		}
	}
}
