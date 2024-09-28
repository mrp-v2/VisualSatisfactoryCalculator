using System.Collections.Generic;
using System.Collections.Immutable;

using Newtonsoft.Json;

using VisualSatisfactoryCalculator.satisfactory.Interfaces;
using VisualSatisfactoryCalculator.satisfactory.model.production;

namespace VisualSatisfactoryCalculator.satisfactory.JSONClasses
{
	public class JSONGenerator
	{
		public readonly string id;
		private readonly decimal _powerProduction;
		public readonly string displayName;
		private readonly decimal _powerConsumptionExponent;
		private readonly Fuel[] _fuels;

		private readonly bool _requiresSupplementalResource;
		private readonly decimal _supplementalToPowerRatio;

		[JsonConstructor]
		public JSONGenerator(string ClassName, bool mRequiresSupplementalResource, decimal mSupplementalToPowerRatio, string mPowerProduction, string mDisplayName, Fuel[] mFuel)
		{
			id = ClassName;
			_powerProduction = decimal.Parse(mPowerProduction);
			_powerConsumptionExponent = 1;
			displayName = mDisplayName;
			_requiresSupplementalResource = mRequiresSupplementalResource;
			_supplementalToPowerRatio = mSupplementalToPowerRatio;
			_fuels = mFuel;
		}

		public static readonly decimal ENERGY_DIVISOR = 50m / 3;
		public static readonly decimal SUPPLEMENTAL_RESOURCE_FACTOR = 60m;

		public bool EqualID(string id)
		{
			return this.id.Equals(id);
		}

		public bool EqualID(IHasID obj)
		{
			return obj.EqualID(id);
		}

		public Building Process()
		{
			return new Building(id, displayName, -_powerProduction, _powerConsumptionExponent);
		}

		public IEnumerable<Recipe> ProcessRecipes(Dictionary<string, JSONItem> items, Dictionary<string, HashSet<string>> itemsByNativeClass, ImmutableDictionary<string, Item> processedItems, ImmutableDictionary<string, Building> buildings)
		{
			HashSet<Recipe> recipes = new HashSet<Recipe>();
			foreach (Fuel fuel in _fuels)
			{
				JSONItem fuelItem = items[fuel.itemID];
				decimal itemEnergy = fuelItem.EnergyValue / 1000;
				if (itemEnergy == 0)
				{
					continue;
				}
				Dictionary<Item, decimal> ingredients = new Dictionary<Item, decimal>()
				{
					{ processedItems[fuel.itemID], _powerProduction / itemEnergy / ENERGY_DIVISOR }
				};
				if (_requiresSupplementalResource)
				{
					ingredients.Add(processedItems[fuel.supplementalItemID], _powerProduction * _supplementalToPowerRatio * SUPPLEMENTAL_RESOURCE_FACTOR);
				}
				Dictionary<Item, decimal> products = new Dictionary<Item, decimal>();
				if (fuel.byproductItemID.Length > 0)
				{
					products.Add(processedItems[fuel.byproductItemID], fuel.byproductAmount);
				}
				recipes.Add(new Recipe(id + fuel.itemID, fuelItem.displayName + " to Power in a " + buildings[id].displayName, MakeRecipeConversionString(ingredients, products), 60, buildings[id], ingredients.ToImmutableDictionary(), products.ToImmutableDictionary()));
			}
			return recipes;
		}

		public class Fuel
		{
			public readonly string itemID;
			public readonly string supplementalItemID;
			public readonly string byproductItemID;
			public readonly decimal byproductAmount;

			[JsonConstructor]
			public Fuel(string mFuelClass, string mSupplementalResourceClass, string mByproduct, string mByproductAmount)
			{
				itemID = mFuelClass;
				supplementalItemID = mSupplementalResourceClass;
				byproductItemID = mByproduct;
				if (mByproduct.Length > 0)
				{
					byproductAmount = mByproductAmount.Length > 0 ? decimal.Parse(mByproductAmount) : 0;
				}
			}
		}

		private string MakeRecipeConversionString(Dictionary<Item, decimal> ingredients, Dictionary<Item, decimal> products)
		{
			string conversionString = "";
			bool first = true;
			foreach (KeyValuePair<Item, decimal> pair in ingredients)
			{
				if (!first)
				{
					conversionString += ", ";
				}
				else
				{
					first = false;
				}
				conversionString += pair.Key.ToString(pair.Value);
			}
			conversionString += " -> ";
			foreach (KeyValuePair<Item, decimal> pair in ingredients)
			{
				conversionString += pair.Key.ToString(pair.Value) + ", ";
			}
			conversionString += _powerProduction.ToString() + " MW";
			return conversionString;
		}
	}
}
