using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using VisualSatisfactoryCalculator.code.DataStorage;
using VisualSatisfactoryCalculator.code.Extensions;
using VisualSatisfactoryCalculator.code.Interfaces;
using VisualSatisfactoryCalculator.code.Utility;

namespace VisualSatisfactoryCalculator.code.JSONClasses
{
	class JSONGenerator : IBuilding
	{
		private readonly string UID;
		private readonly string[] defaultFuelClasses;
		private readonly string powerProduction;
		private readonly string displayName;

		[JsonConstructor]
		public JSONGenerator(string ClassName, string mDefaultFuelClasses, string mPowerProduction, string mDisplayName)
		{
			UID = ClassName;
			defaultFuelClasses = mDefaultFuelClasses.Split(',');
			powerProduction = mPowerProduction;
			displayName = mDisplayName;
		}

		public static readonly decimal GeneratorEnergyDivisor = 16m + 2m / 3m;

		public List<IRecipe> GetRecipes(List<IEncoder> encodings)
		{
			List<IRecipe> recipes = new List<IRecipe>();
			foreach (string item in defaultFuelClasses)
			{
				string itemID = item.Substring(item.IndexOf('.') + 1);
				if (itemID.IndexOf(")") >= 0)
				{
					itemID = itemID.Substring(0, itemID.IndexOf(")"));
				}
				IEncoder encodingItem = encodings.FindByID(itemID);
				Trace.Assert(encodingItem is JSONItem);
				JSONItem jItem = encodingItem as JSONItem;
				List<ItemCount> counts = new List<ItemCount>
				{
					new ItemCount(itemID, -1 * (decimal.Parse(powerProduction) / (jItem.GetEnergyValue() / 1000) / GeneratorEnergyDivisor)),
					new ItemCount(Constants.MWItem.GetUID(), decimal.Parse(powerProduction))
				};
				IRecipe recipe = new BasicRecipe(UID + itemID, 60, UID, counts, jItem.GetDisplayName() + " to Power");
				recipes.Add(recipe);
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

		public string GetUID()
		{
			return UID;
		}

		public string GetDisplayName()
		{
			return displayName;
		}

		public decimal GetPowerConsumption()
		{
			return 0;
		}
	}
}
