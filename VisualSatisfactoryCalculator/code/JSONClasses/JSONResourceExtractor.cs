using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using VisualSatisfactoryCalculator.satisfactory.model.production;

using Util = VisualSatisfactoryCalculator.satisfactory.Utility.Util;

namespace VisualSatisfactoryCalculator.satisfactory.JSONClasses
{
	public class JSONResourceExtractor : JSONBuilding
	{
		public static Dictionary<string, decimal> NODE_CYCLE_TIME_DIVISORS = new Dictionary<string, decimal>
		{
			{"Impure", 0.5m }, { "Normal", 1 }, { "Pure", 2 }
		};

		private decimal CycleTime { get; }
		private decimal ItemsPerCycle { get; }
		private string AllowedResourceForms { get; }
		private bool OnlySpecificResources { get; }
		private string[] AllowedResources { get; }

		public JSONResourceExtractor(string ClassName, string mPowerConsumption, string mPowerConsumptionExponent, string mDisplayName, string mExtractCycleTime, string mItemsPerCycle, string mAllowedResourceForms, string mOnlyAllowCertainResources, string mAllowedResources) : base(ClassName, mPowerConsumption, mPowerConsumptionExponent, mDisplayName)
		{
			CycleTime = decimal.Parse(mExtractCycleTime);
			ItemsPerCycle = decimal.Parse(mItemsPerCycle);
			AllowedResourceForms = mAllowedResourceForms;
			OnlySpecificResources = bool.Parse(mOnlyAllowCertainResources);
			AllowedResources = Util.ParseUIDList(mAllowedResources);
		}

		public virtual IEnumerable<Recipe> ProcessRecipes(ImmutableDictionary<string, Item> items, HashSet<JSONItem> resourceItems, ImmutableDictionary<string, Building> buildings)
		{
			HashSet<Recipe> recipes = new HashSet<Recipe>();
			foreach (JSONItem resourceItem in resourceItems)
			{
				if (AllowedResourceForms.Contains(resourceItem.Form))
				{
					if (OnlySpecificResources)
					{
						if (!AllowedResources.Contains(resourceItem.id))
						{
							continue;
						}
					}
					foreach (string resourceNodeType in NODE_CYCLE_TIME_DIVISORS.Keys)
					{
						Dictionary<Item, decimal> products = new Dictionary<Item, decimal>
						{
							{ items[resourceItem.id], ItemsPerCycle }
						};
						string displayName = resourceNodeType + " " + resourceItem.displayName + " in a " + buildings[ID].displayName;
						Recipe recipe = MakeRecipe(ID + resourceNodeType + resourceItem.id, displayName, CycleTime / NODE_CYCLE_TIME_DIVISORS[resourceNodeType], buildings[ID], products);
						recipes.Add(recipe);
					}
				}
			}
			return recipes;
		}

		public class JSONWaterPump : JSONResourceExtractor
		{
			public JSONWaterPump(string ClassName, string mPowerConsumption, string mPowerConsumptionExponent, string mDisplayName, string mExtractCycleTime, string mItemsPerCycle, string mAllowedResourceForms, string mOnlyAllowCertainResources, string mAllowedResources) : base(ClassName, mPowerConsumption, mPowerConsumptionExponent, mDisplayName, mExtractCycleTime, mItemsPerCycle, mAllowedResourceForms, mOnlyAllowCertainResources, mAllowedResources)
			{
			}

			public override IEnumerable<Recipe> ProcessRecipes(ImmutableDictionary<string, Item> items, HashSet<JSONItem> resourceItems, ImmutableDictionary<string, Building> buildings)
			{
				HashSet<Recipe> recipes = new HashSet<Recipe>();
				foreach (JSONItem resouceItem in resourceItems)
				{
					if (AllowedResourceForms.Contains(resouceItem.Form))
					{
						if (OnlySpecificResources)
						{
							if (!AllowedResources.Contains(resouceItem.id))
							{
								continue;
							}
						}
						Dictionary<Item, decimal> products = new Dictionary<Item, decimal>
						{
							{ items[resouceItem.id], ItemsPerCycle }
						};
						string displayName = resouceItem.displayName + " in a " + buildings[ID].displayName;
						Recipe recipe = MakeRecipe(ID + resouceItem.id, displayName, CycleTime, buildings[ID], products);
						recipes.Add(recipe);
					}
				}
				return recipes;
			}
		}

		private Recipe MakeRecipe(string id, string displayName, decimal time, Building building, Dictionary<Item, decimal> products)
		{
			string conversionString = "";
			bool first = true;
			foreach (KeyValuePair<Item, decimal> pair in products)
			{
				if (!first)
				{
					conversionString += ", ";
				}
				else
				{
					first = false;
				}
				conversionString += pair.Key.ToString(pair.Value);
			}
			return new Recipe(id, displayName, conversionString, time, building, ImmutableDictionary<Item, decimal>.Empty, products.ToImmutableDictionary());
		}
	}
}
