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

namespace VisualSatisfactoryCalculator.satisfactory.JSONClasses
{
	public class JSONGenerator : IBuilding, IFromJson
	{
		public string ID { get; }
		private readonly string[] _fuelItemIDs;
		private readonly RationalNumber _powerProduction;
		public string DisplayName { get; }
		public RationalNumber PowerConsumption { get { return -_powerProduction; } }
		public RationalNumber PowerConsumptionExponent { get; }
		public string NativeClass { get; }

		private readonly bool _requiresSupplementalResource;
		private readonly decimal _supplementalToPowerRatio;

		[JsonConstructor]
		public JSONGenerator(string ClassName, string mDefaultFuelClasses, bool mRequiresSupplementalResource, decimal mSupplementalToPowerRatio, string mPowerProduction, string mDisplayName)
		{
			ID = ClassName;
			_fuelItemIDs = Util.ParseUIDList(mDefaultFuelClasses);
			_powerProduction = decimal.Parse(mPowerProduction);
			PowerConsumptionExponent = 1;
			DisplayName = mDisplayName;
			_requiresSupplementalResource = mRequiresSupplementalResource;
			_supplementalToPowerRatio = mSupplementalToPowerRatio;
			NativeClass = FileInteractor.ActiveNativeClass;
		}

		public static readonly RationalNumber ENERGY_DIVISOR = new RationalNumber(50, 3, true);
		public static readonly decimal SUPPLEMENTAL_RESOURCE_FACTOR = 60m;

		public Dictionary<string, IRecipe> GetRecipes(JsonEncodings encodings)
		{
			Dictionary<string, IRecipe> recipes = new Dictionary<string, IRecipe>();
			foreach (string fuelItemID in _fuelItemIDs)
			{
				JSONItem fuelItem = encodings.GetItem(fuelItemID);
				decimal d = fuelItem.EnergyValue / 1000;
				List<ItemCount<JSONItem>> ingredients = new List<ItemCount<JSONItem>>
				{
					new ItemCount<JSONItem>(fuelItem, _powerProduction / d / ENERGY_DIVISOR)
				};
				if (_requiresSupplementalResource)
				{
					ingredients.Add(new ItemCount<JSONItem>(encodings.GetItem(Constants.WATER_ID), _powerProduction * _supplementalToPowerRatio * SUPPLEMENTAL_RESOURCE_FACTOR));
				}
				IRecipe recipe = new JSONGeneratorRecipe(ID + fuelItemID, 60, ID, ingredients, new List<ItemCount<JSONItem>>(), fuelItem.displayName + " to Power", _powerProduction);
				recipes.Add(recipe.ID, recipe);
			}
			return recipes;
		}

		public bool EqualID(string id)
		{
			return ID.Equals(id);
		}

		public bool EqualID(IHasID obj)
		{
			return obj.EqualID(ID);
		}

		private class JSONGeneratorRecipe : BasicRecipe
		{
			private readonly RationalNumber _powerProduction;

			public JSONGeneratorRecipe(string UID, decimal craftTime, string machineUID, List<ItemCount<JSONItem>> ingredients, List<ItemCount<JSONItem>> products, string displayName, RationalNumber powerProduction) : base(UID, craftTime, machineUID, ingredients, products, displayName)
			{
				_powerProduction = powerProduction;
			}

			protected override string GetConversionString()
			{
				string str = "";
				bool first = true;
				foreach (JSONItem key in ingredients.Keys)
				{
					if (!first)
					{
						str += ", ";
					}
					else
					{
						first = false;
					}
					str += key.ToString(ingredients[key]);
				}
				str += " -> " + _powerProduction.ToString() + " MW";
				return str;
			}
		}
	}
}
