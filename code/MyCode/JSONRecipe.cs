using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.code
{
	public class JSONRecipe
	{
		public string ClassName;
		public string mDisplayName;
		public string mIngredients;
		public string mProduct;
		public string mManufactoringDuration;
		public string mProducedIn;

		private List<ItemCount> GetItemCounts(List<JSONItem> itemsDescriptors)
		{
			List<ItemCount> items = new List<ItemCount>();
			//ingredients
			string[] ingredients = mIngredients.Split(',');
			Trace.Assert(ingredients.Length % 2 == 0);
			for (int i = 0; i < ingredients.Length; i += 2)
			{
				string itemString = ingredients[i];
				itemString = itemString.Substring(itemString.IndexOf(".") + 1);
				itemString = itemString.Substring(0, itemString.LastIndexOf("\""));
				string countString = ingredients[i + 1];
				countString = countString.Replace(")", "");
				countString = countString.Remove(0, "Amount=".Length);
				int itemCount = int.Parse(countString);
				if (itemsDescriptors.IsLiquid(itemString))
				{
					itemCount /= 1000;
				}
				items.Add(new ItemCount(itemsDescriptors.GetDisplayNameFor(itemString), -itemCount));
			}
			//products
			string[] products = mProduct.Split(',');
			Trace.Assert(products.Length % 2 == 0);
			for (int i = 0; i < products.Length; i += 2)
			{
				string itemString = products[i];
				itemString = itemString.Substring(itemString.IndexOf(".") + 1);
				itemString = itemString.Substring(0, itemString.LastIndexOf("\""));
				string countString = products[i + 1];
				countString = countString.Replace(")", "");
				countString = countString.Remove(0, "Amount=".Length);
				int itemCount = int.Parse(countString);
				if (itemsDescriptors.IsLiquid(itemString))
				{
					itemCount /= 1000;
				}
				items.Add(new ItemCount(itemsDescriptors.GetDisplayNameFor(itemString), itemCount));
			}
			return items;
		}

		public bool IsResourceRecipe()
		{
			return mProduct.Contains("/Game/FactoryGame/Resource");
		}

		public bool IsBuildableRecipe()
		{
			return mProduct.Contains("/Game/FactoryGame/Buildable");
		}
		
		private static readonly string[] recognizedMachines = new string[] {"ConstructorMk1", "SmelterMk1", "AssemblerMk1", "OilRefinery", "FoundryMk1", "ManufacturerMk1"};

		public bool IsProducedInMachine()
		{
			if (mProducedIn.Contains("BuildGun")) return false;
			foreach (string str in recognizedMachines)
			{
				if (mProducedIn.Contains(str)) return true;
			}
			return false;
		}

		private string GetMachineName()
		{
			string[] machines = mProducedIn.Split(',');
			foreach (string str in machines)
			{
				if (str.Contains("Buildable/Factory"))
				{
					string tempStr = str.Substring(0, str.LastIndexOf("/"));
					tempStr = tempStr.Substring(tempStr.LastIndexOf("/") + 1);
					switch (tempStr)
					{
						case "ConstructorMk1":
							return "Constructor";
						case "SmelterMk1":
							return "Smelter";
						case "AssemblerMk1":
							return "Assembler";
						case "OilRefinery":
							return "Refinery";
						case "FoundryMk1":
							return "Foundry";
						case "ManufacturerMk1":
							return "Manufacturer";
						default:
							//Console.WriteLine(tempStr);
							return "Unknown Machine";
					}

				}
			}
			return default;
		}

		private int GetProductionTime()
		{
			return (int)double.Parse(mManufactoringDuration);
		}

		public Recipe ToRecipe(List<JSONItem> itemsDescriptors)
		{
			return new Recipe(GetItemCounts(itemsDescriptors), GetProductionTime(), GetMachineName());
		}
	}
}
