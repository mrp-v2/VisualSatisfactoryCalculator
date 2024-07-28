using System.Collections.Generic;
using System.Diagnostics;

using Newtonsoft.Json;

using VisualSatisfactoryCalculator.code.DataStorage;
using VisualSatisfactoryCalculator.code.Interfaces;
using VisualSatisfactoryCalculator.code.Numbers;
using VisualSatisfactoryCalculator.code.Utility;

namespace VisualSatisfactoryCalculator.code.JSONClasses
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
				List<ItemRate> ingredients = new List<ItemRate>
				{
					new ItemRate(fuelItemID, powerProduction / d / EnergyDivisor)
				};
				if (requiresSupplementalResource)
				{
					ingredients.Add(new ItemRate(Constants.WaterID, powerProduction * supplementalToPowerRatio * SupplementalResourceFactor));
				}
				IRecipe recipe = new JSONGeneratorRecipe(ID + fuelItemID, 60, ID, ingredients, new List<ItemRate>(), jItem.DisplayName + " to Power", powerProduction);
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

			public JSONGeneratorRecipe(string UID, decimal craftTime, string machineUID, List<ItemRate> ingredients, List<ItemRate> products, string displayName, RationalNumber powerProduction) : base(UID, craftTime, machineUID, ingredients, products, displayName)
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
				str += " -> " + powerProduction.ToString() + " MW";
				return str;
			}
		}
	}
}
