using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using VisualSatisfactoryCalculator.code.DataStorage;
using VisualSatisfactoryCalculator.code.Extensions;
using VisualSatisfactoryCalculator.code.Interfaces;

namespace VisualSatisfactoryCalculator.code.JSONClasses
{
	public class JSONRecipe : IRecipe
	{
		private readonly string UID;
		private readonly string displayName;
		private readonly List<ItemCount> itemCounts;
		private readonly string machineUID;
		private readonly decimal craftTime;

		[JsonConstructor]
		public JSONRecipe(string ClassName, string mDisplayName, string mIngredients, string mProduct, string mManufactoringDuration, string mProducedIn)
		{
			UID = ClassName;
			displayName = mDisplayName;
			craftTime = decimal.Parse(mManufactoringDuration);
			//machine
			string[] machines = mProducedIn.Split(',');
			foreach (string str in machines)
			{
				if (str.Contains("Buildable/Factory"))
				{
					machineUID = str.Substring(str.IndexOf('.') + 1);
					if (machineUID.Contains(")")) machineUID = machineUID.Substring(0, machineUID.IndexOf(")"));
				}
			}
			//ingredients
			itemCounts = new List<ItemCount>();
			string[] ingredientsList = mIngredients.Split(',');
			Trace.Assert(ingredientsList.Length % 2 == 0);
			for (int i = 0; i < ingredientsList.Length; i += 2) itemCounts.Add(ParseItemCount(ingredientsList[i], ingredientsList[i + 1], false));
			//products
			string[] productsList = mProduct.Split(',');
			Trace.Assert(productsList.Length % 2 == 0);
			for (int i = 0; i < productsList.Length; i += 2) itemCounts.Add(ParseItemCount(productsList[i], productsList[i + 1], true));
		}

		/// <summary>
		/// Should only be used in Initialize()
		/// </summary>
		/// <param name="items"></param>
		/// <param name="itemString"></param>
		/// <param name="countString"></param>
		/// <param name="positive"></param>
		/// <returns></returns>
		private ItemCount ParseItemCount(string itemString, string countString, bool positive)
		{
			itemString = itemString.Substring(itemString.IndexOf(".") + 1);
			itemString = itemString.Substring(0, itemString.LastIndexOf("\""));
			countString = countString.Replace(")", "");
			countString = countString.Remove(0, "Amount=".Length);
			int itemCount = int.Parse(countString);
			if (positive) return new ItemCount(itemString, itemCount);
			else return new ItemCount(itemString, -itemCount);
		}

		public string ToString(List<IEncoder> encodings)
		{
			string str = displayName + ": ";
			List<ItemCount> ingredients = itemCounts.GetIngredients().Inverse();
			for (int i = 0; i < ingredients.Count; i++)
			{
				if (i > 0) str += ", ";
				str += ingredients[i].ToString(encodings);
			}
			str += " -> ";
			List<ItemCount> products = itemCounts.GetProducts();
			for (int i = 0; i < products.Count; i++)
			{
				if (i > 0) str += ", ";
				str += products[i].ToString(encodings);
			}
			str += " in " + Math.Round(craftTime, 3) + " seconds using a " + encodings.GetDisplayNameFor(machineUID);
			return str;
		}

		public override int GetHashCode() 
		{ 
			return base.GetHashCode();
		}

		public bool Equals(IRecipe obj)
		{
			if (obj == null) return false;
			if (!(obj is JSONRecipe)) return false;
			return EqualID(obj);
		}

		public bool EqualID(string id)
		{
			return UID.Equals(id);
		}

		public bool EqualID(IHasUID obj)
		{
			return obj.EqualID(UID);
		}

		public string GetMachineUID()
		{
			return machineUID;
		}

		public List<ItemCount> GetItemCounts()
		{
			return itemCounts;
		}

		public decimal GetCraftTime()
		{
			return craftTime;
		}

		public string GetUID()
		{
			return UID;
		}

		public string GetDisplayName()
		{
			return displayName;
		}
	}
}
