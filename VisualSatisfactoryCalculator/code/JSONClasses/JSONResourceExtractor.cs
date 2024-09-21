using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VisualSatisfactoryCalculator.satisfactory.DataStorage;
using VisualSatisfactoryCalculator.satisfactory.Interfaces;
using VisualSatisfactoryCalculator.satisfactory.Numbers;
using VisualSatisfactoryCalculator.satisfactory.Utility;
using VisualSatisfactoryCalculator.model.production;
using Util = VisualSatisfactoryCalculator.satisfactory.Utility.Util;
using VisualSatisfactoryCalculator.satisfactory.model.production;
using System.Collections.Immutable;

namespace VisualSatisfactoryCalculator.satisfactory.JSONClasses
{
	public class JSONResourceExtractor : JSONBuilding
	{
		public static Dictionary<string, RationalNumber> NODE_CYCLE_TIME_DIVISORS = new Dictionary<string, RationalNumber>
		{
			{"Impure", new RationalNumber(1, 2, true) }, { "Normal", 1 }, { "Pure", 2 }
		};

		private RationalNumber CycleTime { get; }
		private RationalNumber ItemsPerCycle { get; }
		private string AllowedResourceForms { get; }
		private bool OnlySpecificResources { get; }
		private string[] AllowedResources { get; }

		public JSONResourceExtractor(string ClassName, string mPowerConsumption, string mPowerConsumptionExponent, string mDisplayName, string mExtractCycleTime, string mItemsPerCycle, string mAllowedResourceForms, string mOnlyAllowCertainResources, string mAllowedResources) : base(ClassName, mPowerConsumption, mPowerConsumptionExponent, mDisplayName)
		{
			CycleTime = RationalNumber.FromDecimalString(mExtractCycleTime);
			ItemsPerCycle = RationalNumber.FromDecimalString(mItemsPerCycle);
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
						Dictionary<Item, RationalNumber> products = new Dictionary<Item, RationalNumber>
						{
							{ items[resourceItem.id], ItemsPerCycle }
						};
						Recipe recipe = MakeRecipe(ID + resourceNodeType + resourceItem.id, resourceNodeType + " " + resourceItem.displayName, CycleTime / NODE_CYCLE_TIME_DIVISORS[resourceNodeType], buildings[ID], products);
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
						Dictionary<Item, RationalNumber> products = new Dictionary<Item, RationalNumber>
						{
							{ items[resouceItem.id], ItemsPerCycle }
						};
						Recipe recipe = MakeRecipe(ID + resouceItem.id, resouceItem.displayName, CycleTime, buildings[ID], products);
						recipes.Add(recipe);
					}
				}
				return recipes;
			}
		}

		private Recipe MakeRecipe(string id, string displayName, RationalNumber time, Building building, Dictionary<Item, RationalNumber> products)
		{
			string conversionString = "";
			bool first = true;
			foreach (KeyValuePair<Item, RationalNumber> pair in products)
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
			return new Recipe(id, displayName, conversionString, time, building, ImmutableDictionary<Item, RationalNumber>.Empty, products.ToImmutableDictionary());
		}
	}
}
