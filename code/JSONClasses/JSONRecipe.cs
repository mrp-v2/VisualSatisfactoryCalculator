using System.Collections.Generic;
using System.Diagnostics;

using Newtonsoft.Json;

using VisualSatisfactoryCalculator.code.DataStorage;
using VisualSatisfactoryCalculator.code.Interfaces;
using VisualSatisfactoryCalculator.code.Utility;

namespace VisualSatisfactoryCalculator.code.JSONClasses
{
	public class JSONRecipe : BasicRecipe, IFromJson
	{
		public string NativeClass { get; }

		[JsonConstructor]
		public JSONRecipe(string ClassName, string mDisplayName, string mIngredients, string mProduct, string mManufactoringDuration, string mProducedIn, string mVariablePowerConsumptionConstant, string mVariablePowerConsumptionFactor)
			: base(ClassName, decimal.Parse(mManufactoringDuration), GetMachineUID(mProducedIn), GetIngredients(mIngredients), GetProducts(mProduct), mDisplayName)
		{
			NativeClass = FileInteractor.ActiveNativeClass;
		}

		private static string GetMachineUID(string mProducedIn)
		{
			string[] machines = Util.ParseUIDList(mProducedIn);
			foreach (string machineUID in machines)
			{
				if (FileInteractor.CurrentEncodings.ContainsKey(machineUID))
				{
					return machineUID;
				}
			}
			return default;
		}

		private static List<ItemCount> GetIngredients(string mIngredients)
		{
			List<ItemCount> ingredientsList = new List<ItemCount>();
			string[] ingredientsArray = mIngredients.Split(',');
			Trace.Assert(ingredientsArray.Length % 2 == 0);
			for (int i = 0; i < ingredientsArray.Length; i += 2)
			{
				ingredientsList.Add(ParseItemCount(ingredientsArray[i], ingredientsArray[i + 1]));
			}
			return ingredientsList;
		}

		private static List<ItemCount> GetProducts(string mProduct)
		{
			List<ItemCount> productsList = new List<ItemCount>();
			string[] productsArray = mProduct.Split(',');
			Trace.Assert(productsArray.Length % 2 == 0);
			for (int i = 0; i < productsArray.Length; i += 2)
			{
				productsList.Add(ParseItemCount(productsArray[i], productsArray[i + 1]));
			}
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
