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
	public class JSONResourceExtractor : JSONBuilding, IBuilding, IFromJson
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

		public override IEnumerable<Recipe> ProcessRecipes(Dictionary<string, Item> items, HashSet<JSONItem> resourceItems, Dictionary<string, Building> buildings)
		{
			List<Recipe> recipes = new List<Recipe>();
			foreach (JSONItem item in resourceItems)
			{
				if (AllowedResourceForms.Contains(item.Form))
				{
					if (OnlySpecificResources)
					{
						if (!AllowedResources.Contains(item.id))
						{
							continue;
						}
					}
					foreach (string resourceNodeType in NODE_CYCLE_TIME_DIVISORS.Keys)
					{
						List<ItemCount<Item>> products = new List<ItemCount<Item>>
						{
							new ItemCount<Item>(items[item.id], ItemsPerCycle)
						};
						Recipe recipe = new JSONResourceExtractorRecipe(ID + resourceNodeType + item.id, CycleTime / NODE_CYCLE_TIME_DIVISORS[resourceNodeType], ID, new List<ItemCount<JSONItem>>(), products, resourceNodeType + " " + item.displayName);
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

			public override Dictionary<string, IRecipe> GetRecipes(JsonEncodings encodings)
			{
				Dictionary<string, IRecipe> recipes = new Dictionary<string, IRecipe>();
				foreach (JSONItem item in encodings.ResourceItems)
				{
					if (AllowedResourceForms.Contains(item.Form))
					{
						if (OnlySpecificResources)
						{
							if (!AllowedResources.Contains(item.id))
							{
								continue;
							}
						}
						List<ItemCount<JSONItem>> products = new List<ItemCount<JSONItem>>
						{
							new ItemCount<JSONItem>(encodings.GetItem(item.id), ItemsPerCycle)
						};
						IRecipe recipe = new JSONResourceExtractorRecipe(ID + item.id, CycleTime, ID, new List<ItemCount<JSONItem>>(), products, item.displayName);
						recipe = MakeRecipe()
						recipes.Add(recipe.ID, recipe);
					}
				}
				return recipes;
			}
		}

		private Recipe MakeRecipe(string id, string displayName, RationalNumber time, Building building, Dictionary<Item, RationalNumber> ingredients, Dictionary<Item, RationalNumber> products)
		{
			string conversionString = "";
			bool first = true;
			foreach (Item key in products.Keys)
			{
				if (!first)
				{
					conversionString += ", ";
				}
				else
				{
					first = false;
				}
				conversionString += key.ToString(products[key]);
			}
			return new Recipe(id, displayName, conversionString, time, building, ingredients.ToImmutableDictionary(), products.ToImmutableDictionary());
		}

		public class JSONResourceExtractorRecipe : BasicRecipe
		{
			public JSONResourceExtractorRecipe(string UID, RationalNumber craftTime, string machineUID, List<ItemCount<JSONItem>> ingredients, List<ItemCount<JSONItem>> products, string displayName) : base(UID, craftTime, machineUID, ingredients, products, displayName)
			{
			}

			protected override string GetConversionString()
			{
				string str = "";
				bool first = true;
				foreach (JSONItem key in products.Keys)
				{
					if (!first)
					{
						str += ", ";
					}
					else
					{
						first = false;
					}
					str += key.ToString(products[key]);
				}
				return str;
			}
		}
	}
}
