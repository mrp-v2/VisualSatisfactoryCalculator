﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VisualSatisfactoryCalculator.code.DataStorage;
using VisualSatisfactoryCalculator.code.Interfaces;
using VisualSatisfactoryCalculator.code.Utility;

namespace VisualSatisfactoryCalculator.code.JSONClasses
{
	class JSONResourceExtractor : JSONBuilding, IResourceExtractor, IFromJson
	{
		public static Dictionary<string, decimal> NODE_CYCLE_TIME_DIVISORS = new Dictionary<string, decimal>
		{
			{"Impure", 0.5m }, { "Normal", 1.0m }, { "Pure", 2.0m }
		};

		public decimal CycleTime { get; }
		public decimal ItemsPerCycle { get; }
		public string AllowedResourceForms { get; }
		public bool OnlySpecificResources { get; }
		public string[] AllowedResources { get; }

		public JSONResourceExtractor(string ClassName, string mPowerConsumption, string mPowerConsumptionExponent, string mDisplayName, string mExtractCycleTime, string mItemsPerCycle, string mAllowedResourceForms, string mOnlyAllowCertainResources, string mAllowedResources) : base(ClassName, mPowerConsumption, mPowerConsumptionExponent, mDisplayName)
		{
			CycleTime = decimal.Parse(mExtractCycleTime);
			ItemsPerCycle = decimal.Parse(mItemsPerCycle);
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
						if (!AllowedResources.Contains(item.UID))
						{
							continue;
						}
					}
					foreach (string resourceNodeType in NODE_CYCLE_TIME_DIVISORS.Keys)
					{
						List<ItemCount> products = new List<ItemCount>();
						products.Add(new ItemCount(item.UID, ItemsPerCycle));
						IRecipe recipe = new BasicRecipe(UID + resourceNodeType + item.UID, CycleTime / NODE_CYCLE_TIME_DIVISORS[resourceNodeType], UID, new List<ItemCount>(), products, resourceNodeType + " " + item.DisplayName);
						recipes.Add(recipe.UID, recipe);
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
							if (!AllowedResources.Contains(item.UID))
							{
								continue;
							}
						}
						List<ItemCount> products = new List<ItemCount>();
						products.Add(new ItemCount(item.UID, ItemsPerCycle));
						IRecipe recipe = new BasicRecipe(UID + item.UID, CycleTime, UID, new List<ItemCount>(), products, item.DisplayName);
						recipes.Add(recipe.UID, recipe);
					}
				}
				return recipes;
			}
		}
	}
}