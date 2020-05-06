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
			: base(ClassName, decimal.Parse(mManufactoringDuration), GetMachineUID(mProducedIn), GetIngredients(mIngredients), GetProducts(mProduct), mDisplayName) { }

		private static string GetMachineUID(string mProducedIn)
		{
			string[] machines = mProducedIn.Split(',');
			string machineUID = default;
			foreach (string str in machines)
			{
				if (str.Contains("Buildable/Factory") && !str.Contains("WorkBench"))
				{
					machineUID = str.Substring(str.IndexOf('.') + 1);
					if (machineUID.Contains(")")) machineUID = machineUID.Substring(0, machineUID.IndexOf(")"));
				}
			}
			return machineUID;
		}

		private static List<ItemCount> GetIngredients(string mIngredients)
		{
			List<ItemCount> ingredientsList = new List<ItemCount>();
			string[] ingredientsArray = mIngredients.Split(',');
			Trace.Assert(ingredientsArray.Length % 2 == 0);
			for (int i = 0; i < ingredientsArray.Length; i += 2) ingredientsList.Add(ParseItemCount(ingredientsArray[i], ingredientsArray[i + 1]));
			return ingredientsList;
		}

		private static List<ItemCount> GetProducts(string mProduct)
		{
			List<ItemCount> productsList = new List<ItemCount>();
			string[] productsArray = mProduct.Split(',');
			Trace.Assert(productsArray.Length % 2 == 0);
			for (int i = 0; i < productsArray.Length; i += 2) productsList.Add(ParseItemCount(productsArray[i], productsArray[i + 1]));
			return productsList;
		}

		private static ItemCount ParseItemCount(string itemString, string countString)
		{
			itemString = itemString.Substring(itemString.IndexOf(".") + 1);
			itemString = itemString.Substring(0, itemString.LastIndexOf("\""));
			countString = countString.Replace(")", "");
			countString = countString.Remove(0, "Amount=".Length);
			int itemCount = int.Parse(countString);
			return new ItemCount(itemString, itemCount);
		}
	}
}
