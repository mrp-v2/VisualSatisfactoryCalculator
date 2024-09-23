using System;
using System.Collections.Generic;
using System.Diagnostics;

using Newtonsoft.Json;

using VisualSatisfactoryCalculator.satisfactory.DataStorage;
using VisualSatisfactoryCalculator.satisfactory.Extensions;
using VisualSatisfactoryCalculator.satisfactory.Interfaces;
using VisualSatisfactoryCalculator.satisfactory.Numbers;
using VisualSatisfactoryCalculator.satisfactory.Utility;
using VisualSatisfactoryCalculator.model.production;
using Util = VisualSatisfactoryCalculator.satisfactory.Utility.Util;
using VisualSatisfactoryCalculator.satisfactory.model.production;
using System.Collections.Immutable;

namespace VisualSatisfactoryCalculator.satisfactory.JSONClasses
{
	public class JSONGenerator
	{
		public readonly string id;
		private readonly RationalNumber _powerProduction;
		public readonly string displayName;
		private readonly RationalNumber _powerConsumptionExponent;
		private readonly Fuel[] _fuels;

		private readonly bool _requiresSupplementalResource;
		private readonly RationalNumber _supplementalToPowerRatio;

		[JsonConstructor]
		public JSONGenerator(string ClassName, bool mRequiresSupplementalResource, decimal mSupplementalToPowerRatio, string mPowerProduction, string mDisplayName, Fuel[] mFuel)
		{
			id = ClassName;
			_powerProduction = RationalNumber.FromDecimalString(mPowerProduction);
			_powerConsumptionExponent = 1;
			displayName = mDisplayName;
			_requiresSupplementalResource = mRequiresSupplementalResource;
			_supplementalToPowerRatio = mSupplementalToPowerRatio;
			_fuels = mFuel;
		}

		public static readonly RationalNumber ENERGY_DIVISOR = new RationalNumber(50, 3, true);
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
				Dictionary<Item, RationalNumber> ingredients = new Dictionary<Item, RationalNumber>()
				{
					{ processedItems[fuel.itemID], _powerProduction / itemEnergy / ENERGY_DIVISOR }
				};
				if (_requiresSupplementalResource)
				{
					ingredients.Add(processedItems[fuel.supplementalItemID], _powerProduction * _supplementalToPowerRatio * SUPPLEMENTAL_RESOURCE_FACTOR);
				}
				Dictionary<Item, RationalNumber> products = new Dictionary<Item, RationalNumber>();
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
			public readonly RationalNumber byproductAmount;

			[JsonConstructor]
			public Fuel(string mFuelClass, string mSupplementalResourceClass, string mByproduct, string mByproductAmount)
			{
				itemID = mFuelClass;
				supplementalItemID = mSupplementalResourceClass;
				byproductItemID = mByproduct;
				if (mByproduct.Length > 0)
				{
					byproductAmount = mByproductAmount.Length > 0 ? RationalNumber.FromDecimalString(mByproductAmount) : null;
				}
			}
		}

		private string MakeRecipeConversionString(Dictionary<Item, RationalNumber> ingredients, Dictionary<Item, RationalNumber> products)
		{
			string conversionString = "";
			bool first = true;
			foreach (KeyValuePair<Item, RationalNumber> pair in ingredients)
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
			foreach (KeyValuePair<Item, RationalNumber> pair in ingredients)
			{
				conversionString += pair.Key.ToString(pair.Value) + ", ";
			}
			conversionString += _powerProduction.ToString() + " MW";
			return conversionString;
		}
	}
}
