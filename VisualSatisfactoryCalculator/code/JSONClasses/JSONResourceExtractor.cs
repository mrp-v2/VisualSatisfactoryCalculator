using System;
using System.Collections.Generic;
using System.Linq;

using VisualSatisfactoryCalculator.code.DataStorage;
using VisualSatisfactoryCalculator.code.Interfaces;
using VisualSatisfactoryCalculator.code.Numbers;
using VisualSatisfactoryCalculator.code.Utility;

namespace VisualSatisfactoryCalculator.code.JSONClasses
{
	class JSONResourceExtractor : JSONBuilding, IResourceExtractor, IFromJson
	{
		public static Dictionary<string, RationalNumber> NODE_CYCLE_TIME_DIVISORS = new Dictionary<string, RationalNumber>
		{
			{"Impure", new RationalNumber(1, 2, true) }, { "Normal", 1 }, { "Pure", 2 }
		};

		public RationalNumber CycleTime { get; }
		public RationalNumber ItemsPerCycle { get; }
		public string AllowedResourceForms { get; }
		public bool OnlySpecificResources { get; }
		public string[] AllowedResources { get; }

		public JSONResourceExtractor(string ClassName, string mPowerConsumption, string mPowerConsumptionExponent, string mDisplayName, string mExtractCycleTime, string mItemsPerCycle, string mAllowedResourceForms, string mOnlyAllowCertainResources, string mAllowedResources) : base(ClassName, mPowerConsumption, mPowerConsumptionExponent, mDisplayName)
		{
			CycleTime = RationalNumber.FromDecimalString(mExtractCycleTime);
			ItemsPerCycle = RationalNumber.FromDecimalString(mItemsPerCycle);
			AllowedResourceForms = mAllowedResourceForms;
			OnlySpecificResources = bool.Parse(mOnlyAllowCertainResources);
			AllowedResources = Util.ParseUIDList(mAllowedResources);
		}

		public virtual Dictionary<string, IRecipe> GetRecipes(Encodings encodings)
		{
			Dictionary<string, IRecipe> recipes = new Dictionary<string, IRecipe>();
			foreach (JSONItem item in encodings.ResourceItems)
			{
				if (AllowedResourceForms.Contains(item.Form))
				{
					if (OnlySpecificResources)
					{
						if (!AllowedResources.Contains(item.ID))
						{
							continue;
						}
					}
					foreach (string resourceNodeType in NODE_CYCLE_TIME_DIVISORS.Keys)
					{
						List<ItemRate> products = new List<ItemRate>
						{
							new ItemRate(item.ID, ItemsPerCycle)
						};
						IRecipe recipe = new JSONResourceExtractorRecipe(ID + resourceNodeType + item.ID, CycleTime / NODE_CYCLE_TIME_DIVISORS[resourceNodeType], ID, new List<ItemRate>(), products, resourceNodeType + " " + item.DisplayName);
						recipes.Add(recipe.ID, recipe);
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

			public override Dictionary<string, IRecipe> GetRecipes(Encodings encodings)
			{
				Dictionary<string, IRecipe> recipes = new Dictionary<string, IRecipe>();
				foreach (JSONItem item in encodings.ResourceItems)
				{
					if (AllowedResourceForms.Contains(item.Form))
					{
						if (OnlySpecificResources)
						{
							if (!AllowedResources.Contains(item.ID))
							{
								continue;
							}
						}
						List<ItemRate> products = new List<ItemRate>
						{
							new ItemRate(item.ID, ItemsPerCycle)
						};
						IRecipe recipe = new JSONResourceExtractorRecipe(ID + item.ID, CycleTime, ID, new List<ItemRate>(), products, item.DisplayName);
						recipes.Add(recipe.ID, recipe);
					}
				}
				return recipes;
			}
		}

		public class JSONResourceExtractorRecipe : BasicRecipe
		{
			public JSONResourceExtractorRecipe(string UID, RationalNumber craftTime, string machineUID, List<ItemRate> ingredients, List<ItemRate> products, string displayName) : base(UID, craftTime, machineUID, ingredients, products, displayName)
			{
			}

			protected override string GetConversionString(Encodings encodings)
			{
				string str = "";
				bool first = true;
				foreach (string key in Products.Keys)
				{
					if (!first)
					{
						str += ", ";
					}
					else
					{
						first = false;
					}
					str += Products[key].ToString(encodings);
				}
				return str;
			}
		}
	}
}
