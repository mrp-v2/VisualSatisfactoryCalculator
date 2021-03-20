using System;
using System.Collections.Generic;
using System.Diagnostics;

using Newtonsoft.Json;

using VisualSatisfactoryCalculator.code.DataStorage;
using VisualSatisfactoryCalculator.code.Extensions;
using VisualSatisfactoryCalculator.code.Interfaces;
using VisualSatisfactoryCalculator.code.Utility;

namespace VisualSatisfactoryCalculator.code.JSONClasses
{
	internal class JSONGenerator : IBuilding, IFromJson
	{
		public string UID { get; }
		private readonly string[] fuelItemIDs;
		private readonly decimal powerProduction;
		public string DisplayName { get; }
		public decimal PowerConsumption { get { return -powerProduction; } }
		public decimal PowerConsumptionExponent { get; }
		public string NativeClass { get; }

		private readonly bool requiresSupplementalResource;
		private readonly decimal supplementalToPowerRatio;

		[JsonConstructor]
		public JSONGenerator(string ClassName, string mDefaultFuelClasses, bool mRequiresSupplementalResource, decimal mSupplementalToPowerRatio, string mPowerProduction, string mPowerProductionExponent, string mDisplayName)
		{
			UID = ClassName;
			fuelItemIDs = Util.ParseUIDList(mDefaultFuelClasses);
			powerProduction = decimal.Parse(mPowerProduction);
			PowerConsumptionExponent = decimal.Parse(mPowerProductionExponent);
			DisplayName = mDisplayName;
			requiresSupplementalResource = mRequiresSupplementalResource;
			supplementalToPowerRatio = mSupplementalToPowerRatio;
			NativeClass = FileInteractor.ActiveNativeClass;
		}

		public static readonly decimal EnergyDivisor = 16m + (2m / 3m);
		public static readonly decimal SupplementalResourceFactor = 60m;

		public Dictionary<string, IRecipe> GetRecipes(Encodings encodings)
		{
			Dictionary<string, IRecipe> recipes = new Dictionary<string, IRecipe>();
			foreach (string fuelItemID in fuelItemIDs)
			{
				IEncoder encodingItem = encodings[fuelItemID];
				Trace.Assert(encodingItem is JSONItem);
				JSONItem jItem = encodingItem as JSONItem;
				List<ItemCount> ingredients = new List<ItemCount>
				{
					new ItemCount(fuelItemID, powerProduction / (jItem.EnergyValue / 1000) / EnergyDivisor)
				};
				if (requiresSupplementalResource)
				{
					ingredients.Add(new ItemCount(Constants.WaterID, powerProduction * supplementalToPowerRatio * SupplementalResourceFactor));
				}
				IRecipe recipe = new JSONGeneratorRecipe(UID + fuelItemID, 60, UID, ingredients, new List<ItemCount>(), jItem.DisplayName + " to Power", powerProduction);
				recipes.Add(recipe.UID, recipe);
			}
			return recipes;
		}

		public bool EqualID(string id)
		{
			return UID.Equals(id);
		}

		public bool EqualID(IHasUID obj)
		{
			return obj.EqualID(UID);
		}

		private class JSONGeneratorRecipe : BasicRecipe
		{
			private readonly decimal powerProduction;

			public JSONGeneratorRecipe(string UID, decimal craftTime, string machineUID, List<ItemCount> ingredients, List<ItemCount> products, string displayName, decimal powerProduction) : base(UID, craftTime, machineUID, ingredients, products, displayName)
			{
				this.powerProduction = powerProduction;
			}

			protected override string GetConversionString(Encodings encodings)
			{
				string str = "";
				bool first = true;
				foreach (string key in Ingredients.Keys)
				{
					if (!first)
					{
						str += ", ";
					}
					else
					{
						first = false;
					}
					str += Ingredients[key].ToString(encodings);
				}
				str += " -> " + powerProduction.ToPrettyString() + " MW";
				return str;
			}
		}
	}
}
