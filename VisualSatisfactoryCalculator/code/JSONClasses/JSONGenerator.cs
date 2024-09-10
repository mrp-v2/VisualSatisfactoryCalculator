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

namespace VisualSatisfactoryCalculator.satisfactory.JSONClasses
{
	internal class JSONGenerator : IBuilding, IFromJson
	{
		public string ID { get; }
		private readonly string[] fuelItemIDs;
		private readonly RationalNumber powerProduction;
		public string DisplayName { get; }
		public RationalNumber PowerConsumption { get { return -powerProduction; } }
		public RationalNumber PowerConsumptionExponent { get; }
		public string NativeClass { get; }

		private readonly bool requiresSupplementalResource;
		private readonly decimal supplementalToPowerRatio;

		[JsonConstructor]
		public JSONGenerator(string ClassName, string mDefaultFuelClasses, bool mRequiresSupplementalResource, decimal mSupplementalToPowerRatio, string mPowerProduction, string mDisplayName)
		{
			ID = ClassName;
			fuelItemIDs = Util.ParseUIDList(mDefaultFuelClasses);
			powerProduction = decimal.Parse(mPowerProduction);
			PowerConsumptionExponent = 1;
			DisplayName = mDisplayName;
			requiresSupplementalResource = mRequiresSupplementalResource;
			supplementalToPowerRatio = mSupplementalToPowerRatio;
			NativeClass = FileInteractor.ActiveNativeClass;
		}

		public static readonly RationalNumber EnergyDivisor = new RationalNumber(50, 3, true);
		public static readonly decimal SupplementalResourceFactor = 60m;

		public Dictionary<string, IRecipe> GetRecipes(Encodings encodings)
		{
			Dictionary<string, IRecipe> recipes = new Dictionary<string, IRecipe>();
			foreach (string fuelItemID in fuelItemIDs)
			{
				IEncoder encodingItem = encodings[fuelItemID];
				Trace.Assert(encodingItem is JSONItem);
				JSONItem jItem = encodingItem as JSONItem;
				decimal d = jItem.EnergyValue / 1000;
				List<ItemCount<JSONItem>> ingredients = new List<ItemCount<JSONItem>>
				{
					new ItemCount<JSONItem>(FileInteractor.CurrentEncodings[fuelItemID] as JSONItem, powerProduction / d / EnergyDivisor)
				};
				if (requiresSupplementalResource)
				{
					ingredients.Add(new ItemCount<JSONItem>(FileInteractor.CurrentEncodings[Constants.WaterID] as JSONItem, powerProduction * supplementalToPowerRatio * SupplementalResourceFactor));
				}
				IRecipe recipe = new JSONGeneratorRecipe(ID + fuelItemID, 60, ID, ingredients, new List<ItemCount<JSONItem>>(), jItem.displayName + " to Power", powerProduction);
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
			private readonly RationalNumber powerProduction;

			public JSONGeneratorRecipe(string UID, decimal craftTime, string machineUID, List<ItemCount<JSONItem>> ingredients, List<ItemCount<JSONItem>> products, string displayName, RationalNumber powerProduction) : base(UID, craftTime, machineUID, ingredients, products, displayName)
			{
				this.powerProduction = powerProduction;
			}

			protected override string GetConversionString(Encodings encodings)
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
				str += " -> " + powerProduction.ToString() + " MW";
				return str;
			}
		}
	}
}
