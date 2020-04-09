using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using VisualSatisfactoryCalculator.code.DataStorage;
using VisualSatisfactoryCalculator.code.Extensions;
using VisualSatisfactoryCalculator.code.Interfaces;
using VisualSatisfactoryCalculator.code.Utility;

namespace VisualSatisfactoryCalculator.code.JSONClasses
{
	class JSONGenerator
	{
		private readonly string UID;
		private readonly string[] defaultFuelClasses;
		private readonly string fuelForm;
		private readonly string powerProduction;
		private readonly string displayName;

		[JsonConstructor]
		public JSONGenerator(string ClassName, string mDefaultFuelClasses, string mFuelResourceForm, string mPowerProduction, string mDisplayName)
		{
			UID = ClassName;
			defaultFuelClasses = mDefaultFuelClasses.Split(',');
			fuelForm = mFuelResourceForm;
			powerProduction = mPowerProduction;
			displayName = mDisplayName;
		}

		public List<IRecipe> GetRecipes(List<JSONItem> items)
		{
			List<IRecipe> recipes = new List<IRecipe>();
			foreach (string item in defaultFuelClasses)
			{
				string itemID = item.Substring(item.IndexOf('.') + 1);
				if (itemID.IndexOf(")") >= 0)
				{
					itemID = itemID.Substring(0, itemID.IndexOf(")"));
				}
				JSONItem jItem = items.MatchID(itemID);
				List<ItemCount> counts = new List<ItemCount>();
				if (fuelForm.Equals("RF_LIQUID"))
				{
					counts.Add(new ItemCount(jItem, -1 * (decimal.Parse(powerProduction) / jItem.GetEnergyValue() / Constants.GeneratorEnergyDivisor)));
				}
				else if (fuelForm.Equals("RF_SOLID"))
				{
					counts.Add(new ItemCount(jItem, -1 * (decimal.Parse(powerProduction) / (jItem.GetEnergyValue() / 1000m) / Constants.GeneratorEnergyDivisor)));
				}
				else
				{
					throw new ArgumentOutOfRangeException("Form " + fuelForm + " is unrecognized!");
				}
				counts.Add(new ItemCount(Constants.MWItem, decimal.Parse(powerProduction)));
				IRecipe recipe = new SimpleCustomRecipe(UID + itemID, 60, displayName, counts, jItem.ToString() + " to Power");
				recipes.Add(recipe);
			}
			return recipes;
		}

		public override string ToString()
		{
			return displayName;
		}
	}
}
