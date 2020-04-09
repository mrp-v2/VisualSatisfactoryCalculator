using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using VisualSatisfactoryCalculator.code.DataStorage;

namespace VisualSatisfactoryCalculator.code.JSONClasses
{
	public class JSONRecipe : BasicRecipe
	{
		[JsonConstructor]
		public JSONRecipe(string ClassName, string mDisplayName, string mIngredients, string mProduct, string mManufactoringDuration, string mProducedIn)
			: base(ClassName, decimal.Parse(mManufactoringDuration), GetMachineUID(mProducedIn), GetItemCounts(mIngredients, mProduct), mDisplayName) { }

		private static string GetMachineUID(string mProducedIn)
		{
			string[] machines = mProducedIn.Split(',');
			string machineUID = default;
			foreach (string str in machines)
			{
				if (str.Contains("Buildable/Factory"))
				{
					machineUID = str.Substring(str.IndexOf('.') + 1);
					if (machineUID.Contains(")")) machineUID = machineUID.Substring(0, machineUID.IndexOf(")"));
				}
			}
			return machineUID;
		}

		private static List<ItemCount> GetItemCounts(string mIngredients, string mProduct)
		{
			List<ItemCount> itemCounts = new List<ItemCount>();
			//ingredients
			string[] ingredientsList = mIngredients.Split(',');
			Trace.Assert(ingredientsList.Length % 2 == 0);
			for (int i = 0; i < ingredientsList.Length; i += 2) itemCounts.Add(ParseItemCount(ingredientsList[i], ingredientsList[i + 1], false));
			//products
			string[] productsList = mProduct.Split(',');
			Trace.Assert(productsList.Length % 2 == 0);
			for (int i = 0; i < productsList.Length; i += 2) itemCounts.Add(ParseItemCount(productsList[i], productsList[i + 1], true));
			return itemCounts;
		}

		private static ItemCount ParseItemCount(string itemString, string countString, bool positive)
		{
			itemString = itemString.Substring(itemString.IndexOf(".") + 1);
			itemString = itemString.Substring(0, itemString.LastIndexOf("\""));
			countString = countString.Replace(")", "");
			countString = countString.Remove(0, "Amount=".Length);
			int itemCount = int.Parse(countString);
			if (positive) return new ItemCount(itemString, itemCount);
			else return new ItemCount(itemString, -itemCount);
		}
	}
}
