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
		private readonly string[] _fuelItemIDs;
		private readonly RationalNumber _powerProduction;
		public readonly string displayName;
		private readonly RationalNumber _powerConsumptionExponent;

		private readonly bool _requiresSupplementalResource;
		private readonly decimal _supplementalToPowerRatio;

		[JsonConstructor]
		public JSONGenerator(string ClassName, string mDefaultFuelClasses, bool mRequiresSupplementalResource, decimal mSupplementalToPowerRatio, string mPowerProduction, string mDisplayName)
		{
			id = ClassName;
			_fuelItemIDs = Util.ParseUIDList(mDefaultFuelClasses);
			_powerProduction = decimal.Parse(mPowerProduction);
			_powerConsumptionExponent = 1;
			displayName = mDisplayName;
			_requiresSupplementalResource = mRequiresSupplementalResource;
			_supplementalToPowerRatio = mSupplementalToPowerRatio;
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

		public IEnumerable<Recipe> ProcessRecipes(Dictionary<string, JSONItem> items, ImmutableDictionary<string, Item> processedItems, ImmutableDictionary<string, Building> buildings)
		{
			HashSet<Recipe> recipes = new HashSet<Recipe>();
			foreach (string fuelItemID in _fuelItemIDs)
			{
				JSONItem fuelItem = items[fuelItemID];
				decimal itemEnergy = fuelItem.EnergyValue / 1000;
				Dictionary<Item, RationalNumber> ingredients = new Dictionary<Item, RationalNumber>() {
					{ processedItems[fuelItemID], _powerProduction / itemEnergy / ENERGY_DIVISOR }
				};
				if (_requiresSupplementalResource)
				{
					ingredients.Add(processedItems[Constants.WATER_ID], _powerProduction * _supplementalToPowerRatio * SUPPLEMENTAL_RESOURCE_FACTOR);
				}
				recipes.Add(new Recipe(id + fuelItemID, fuelItem.displayName + " to Power", MakeRecipeConversionString(ingredients), 60, buildings[id], ingredients.ToImmutableDictionary(), ImmutableDictionary<Item, RationalNumber>.Empty));
			}
			return recipes;
		}

		private string MakeRecipeConversionString(Dictionary<Item, RationalNumber> ingredients)
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
			conversionString += " -> " + _powerProduction.ToString() + " MW";
			return conversionString;
		}
	}
}
