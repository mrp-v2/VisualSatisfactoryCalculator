using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using VisualSatisfactoryCalculator.code.DataStorage;
using VisualSatisfactoryCalculator.code.Extensions;
using VisualSatisfactoryCalculator.code.Interfaces;

namespace VisualSatisfactoryCalculator.code.JSONClasses
{
	public class JSONRecipe : IRecipe
	{
		private readonly string uniqueID;
		private readonly string displayName;
		private readonly string ingredients;
		private readonly string product;
		private readonly string manufactoringDuration;
		private readonly string producedIn;

		private List<ItemCount> itemCounts;
		private string machineName;
		private decimal craftTime;

		[JsonConstructor]
		public JSONRecipe(string ClassName, string mDisplayName, string mIngredients, string mProduct, string mManufactoringDuration, string mProducedIn)
		{
			uniqueID = ClassName;
			displayName = mDisplayName;
			ingredients = mIngredients;
			product = mProduct;
			manufactoringDuration = mManufactoringDuration;
			producedIn = mProducedIn;
		}

		public JSONRecipe(JSONRecipe recipe) : this(recipe.uniqueID, recipe.displayName, recipe.ingredients, recipe.product, recipe.manufactoringDuration, recipe.producedIn)
		{
			itemCounts = recipe.itemCounts;
			machineName = recipe.machineName;
			craftTime = recipe.craftTime;
		}

		public void Initialize(List<JSONItem> items)
		{
			//set item Counts
			itemCounts = new List<ItemCount>();
			//ingredients
			string[] ingredients = this.ingredients.Split(',');
			Trace.Assert(ingredients.Length % 2 == 0);
			for (int i = 0; i < ingredients.Length; i += 2)
			{
				itemCounts.Add(ParseItemCount(items, ingredients[i], ingredients[i + 1], false));
			}
			//products
			string[] products = product.Split(',');
			Trace.Assert(products.Length % 2 == 0);
			for (int i = 0; i < products.Length; i += 2)
			{
				itemCounts.Add(ParseItemCount(items, products[i], products[i + 1], true));
			}
			machineName = GetMachineName();
			craftTime = decimal.Parse(manufactoringDuration);
		}

		/// <summary>
		/// Should only be used in Initialize()
		/// </summary>
		/// <param name="items"></param>
		/// <param name="itemString"></param>
		/// <param name="countString"></param>
		/// <param name="positive"></param>
		/// <returns></returns>
		private ItemCount ParseItemCount(List<JSONItem> items, string itemString, string countString, bool positive)
		{
			itemString = itemString.Substring(itemString.IndexOf(".") + 1);
			itemString = itemString.Substring(0, itemString.LastIndexOf("\""));
			countString = countString.Replace(")", "");
			countString = countString.Remove(0, "Amount=".Length);
			int itemCount = int.Parse(countString);
			JSONItem item = items.GetJSONItemFor(itemString);
			if (item.IsLiquid())
			{
				itemCount /= 1000;
			}
			if (!positive)
			{
				itemCount *= -1;
			}
			return new ItemCount(item, itemCount);
		}

		/// <summary>
		/// Should only be used in Initialize()
		/// </summary>
		/// <returns></returns>
		private string GetMachineName()
		{
			string[] machines = producedIn.Split(',');
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

		public override string ToString()
		{
			string str = displayName + ": ";
			List<ItemCount> ingredients = itemCounts.GetIngredients().Inverse();
			for (int i = 0; i < ingredients.Count; i++)
			{
				if (i > 0)
				{
					str += ", ";
				}
				str += ingredients[i].ToString();
			}
			str += " -> ";
			List<ItemCount> products = itemCounts.GetProducts();
			for (int i = 0; i < products.Count; i++)
			{
				if (i > 0)
				{
					str += ", ";
				}
				str += products[i].ToString();
			}
			str += " in " + craftTime + " seconds using a " + machineName;
			return str;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public bool Equals(IRecipe obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (!(obj is JSONRecipe))
			{
				return false;
			}
			return uniqueID.Equals((obj as JSONRecipe).uniqueID);
		}

		public bool EqualID(string id)
		{
			return uniqueID.Equals(id);
		}

		public string GetMachine()
		{
			return machineName;
		}

		public List<ItemCount> GetItemCounts()
		{
			return itemCounts;
		}

		public decimal GetCraftTime()
		{
			return craftTime;
		}

		public IRecipe Clone()
		{
			JSONRecipe clone = new JSONRecipe(this)
			{
				itemCounts = itemCounts,
				machineName = machineName,
				craftTime = craftTime
			};
			return clone;
		}
	}
}
