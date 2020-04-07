using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VisualSatisfactoryCalculator.code.DataStorage;
using VisualSatisfactoryCalculator.code.Extensions;
using VisualSatisfactoryCalculator.code.Interfaces;

namespace VisualSatisfactoryCalculator.code.JSONClasses
{
	class JSONGenerator
	{
		private readonly string uniqueID;
		private readonly string[] defaultFuelClasses;
		private readonly string fuelResourceForm;
		private readonly string powerProduction;
		private readonly string displayName;

		[JsonConstructor]
		public JSONGenerator(string ClassName, string mDefaultFuelClasses, string mFuelResourceForm, string mPowerProduction, string mDisplayName)
		{
			uniqueID = ClassName;
			defaultFuelClasses = mDefaultFuelClasses.Split(',');
			fuelResourceForm = mFuelResourceForm;
			powerProduction = mPowerProduction;
			displayName = mDisplayName;
		}

		public List<IRecipe> GenerateRecipes(List<JSONItem> items)
		{
			List<IRecipe> recipes = new List<IRecipe>();
			foreach (string item in defaultFuelClasses)
			{
				string itemID = item.Substring(item.IndexOf('.') + 1);
				JSONItem jItem = items.GetJSONItemFor(itemID);
				List<ItemCount> counts = new List<ItemCount>
				{
					new ItemCount(jItem, (int)(decimal.Parse(powerProduction) / jItem.GetEnergyValue() / (16m + 2m/3m))),
					new ItemCount(new SimpleCustomItem("MegaWatt", "MW"), (int)double.Parse(powerProduction))
				};
				//
				IRecipe recipe = new SimpleCustomRecipe(uniqueID + item, 60, displayName, counts);
				recipes.Add(recipe);
			}
			return recipes;
		}
	}
}
