using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VisualSatisfactoryCalculator.code.DataStorage;
using VisualSatisfactoryCalculator.code.Extensions;
using VisualSatisfactoryCalculator.code.Interfaces;
using VisualSatisfactoryCalculator.code.Utility;

namespace VisualSatisfactoryCalculator.code.JSONClasses
{
	class JSONGenerator
	{
		private readonly string uniqueID;
		private readonly string[] defaultFuelClasses;
		private readonly string fuelForm;
		private readonly string powerProduction;
		private readonly string displayName;

		[JsonConstructor]
		public JSONGenerator(string ClassName, string mDefaultFuelClasses, string mFuelResourceForm, string mPowerProduction, string mDisplayName)
		{
			uniqueID = ClassName;
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
				decimal divisor = 1;
				if (fuelForm.Equals("RF_LIQUID")) divisor = Constants.LiquidGeneratorDivisor;
				List<ItemCount> counts = new List<ItemCount>
				{
					new ItemCount(jItem, -1 * (decimal.Parse(powerProduction) / jItem.GetEnergyValue() / divisor)),
					new ItemCount(Constants.MWItem, decimal.Parse(powerProduction))
				};
				IRecipe recipe = new SimpleCustomRecipe(uniqueID + itemID, 60, displayName, counts, jItem.ToString() + " to Power");
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
