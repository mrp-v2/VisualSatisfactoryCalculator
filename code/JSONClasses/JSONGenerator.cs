using System.Collections.Generic;
using System.Diagnostics;

using Newtonsoft.Json;

using VisualSatisfactoryCalculator.code.DataStorage;
using VisualSatisfactoryCalculator.code.Interfaces;
using VisualSatisfactoryCalculator.code.Utility;

namespace VisualSatisfactoryCalculator.code.JSONClasses
{
	internal class JSONGenerator : IBuilding
	{
		public string UID { get; }
		private readonly string[] defaultFuelClasses;
		private readonly string powerProduction;
		public string DisplayName { get; }
		public decimal PowerConsumption { get { return 0; } }

		[JsonConstructor]
		public JSONGenerator(string ClassName, string mDefaultFuelClasses, string mPowerProduction, string mDisplayName)
		{
			UID = ClassName;
			defaultFuelClasses = mDefaultFuelClasses.Split(',');
			powerProduction = mPowerProduction;
			DisplayName = mDisplayName;
		}

		public static readonly decimal GeneratorEnergyDivisor = 16m + (2m / 3m);

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
					new ItemCount(itemID, decimal.Parse(powerProduction) / (jItem.EnergyValue / 1000) / GeneratorEnergyDivisor)
				};
				List<ItemCount> products = new List<ItemCount>
				{
					new ItemCount(Constants.MWItem.UID, decimal.Parse(powerProduction))
				};
				IRecipe recipe = new BasicRecipe(UID + itemID, 60, UID, ingredients, products, jItem.DisplayName + " to Power");
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
	}
}
