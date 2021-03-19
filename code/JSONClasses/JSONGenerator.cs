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
	internal class JSONGenerator : IBuilding
	{
		public string UID { get; }
		private readonly string[] defaultFuelClasses;
		private readonly decimal powerProduction;
		public string DisplayName { get; }
		public decimal PowerConsumption { get { return -powerProduction; } }
		public decimal PowerConsumptionExponent { get; }
		private readonly bool requiresSupplementalResource;
		private readonly decimal supplementalToPowerRatio;

		[JsonConstructor]
		public JSONGenerator(string ClassName, string mDefaultFuelClasses, bool mRequiresSupplementalResource, decimal mSupplementalToPowerRatio, string mPowerProduction, string mPowerProductionExponent, string mDisplayName)
		{
			UID = ClassName;
			defaultFuelClasses = mDefaultFuelClasses.Split(',');
			powerProduction = decimal.Parse(mPowerProduction);
			PowerConsumptionExponent = decimal.Parse(mPowerProductionExponent);
			DisplayName = mDisplayName;
			requiresSupplementalResource = mRequiresSupplementalResource;
			supplementalToPowerRatio = mSupplementalToPowerRatio;
		}

		public static readonly decimal EnergyDivisor = 16m + (2m / 3m);
		public static readonly decimal SupplementalResourceFactor = 60m;

		public Dictionary<string, IRecipe> GetRecipes(Dictionary<string, IEncoder> encodings)
		{
			Dictionary<string, IRecipe> recipes = new Dictionary<string, IRecipe>();
			foreach (string item in defaultFuelClasses)
			{
				string itemID = item.Substring(item.IndexOf('.') + 1);
				if (itemID.IndexOf(")") >= 0)
				{
					itemID = itemID.Substring(0, itemID.IndexOf(")"));
				}
				IEncoder encodingItem = encodings[itemID];
				Trace.Assert(encodingItem is JSONItem);
				JSONItem jItem = encodingItem as JSONItem;
				List<ItemCount> ingredients = new List<ItemCount>
				{
					new ItemCount(itemID, powerProduction / (jItem.EnergyValue / 1000) / EnergyDivisor)
				};
				if (requiresSupplementalResource)
				{
					ingredients.Add(new ItemCount(Constants.WaterID, powerProduction * supplementalToPowerRatio * SupplementalResourceFactor));
				}
				IRecipe recipe = new GeneratorRecipe(UID + itemID, 60, UID, ingredients, new List<ItemCount>(), jItem.DisplayName + " to Power", this.powerProduction);
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

		private class GeneratorRecipe : BasicRecipe
		{
			private readonly decimal powerProduction;

			public GeneratorRecipe(string UID, decimal craftTime, string machineUID, List<ItemCount> ingredients, List<ItemCount> products, string displayName, decimal powerProduction) : base(UID, craftTime, machineUID, ingredients, products, displayName)
			{
				this.powerProduction = powerProduction;
			}

			protected override string GetConversionString(Dictionary<string, IEncoder> encodings)
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
