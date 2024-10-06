using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;

using Newtonsoft.Json;

using VisualSatisfactoryCalculator.model.production;
using VisualSatisfactoryCalculator.satisfactory.model.production;

namespace VisualSatisfactoryCalculator.satisfactory.DataStorage
{
	public class JSONRecipe
	{
		public readonly string id;
		private readonly string _producedIn;
		private readonly string _displayName;
		private readonly decimal _craftTime;
		private readonly string _ingredients;
		private readonly string _products;

		[JsonConstructor]
		public JSONRecipe(string ClassName, string mDisplayName, string mIngredients, string mProduct, string mManufactoringDuration, string mProducedIn, string mVariablePowerConsumptionConstant, string mVariablePowerConsumptionFactor) : this(ClassName, decimal.Parse(mManufactoringDuration), mProducedIn, mIngredients, mProduct, mDisplayName)
		{
		}

		public JSONRecipe(string id, decimal craftTime, string producedIn, string ingredients, string products, string displayName)
		{
			this.id = id;
			_producedIn = producedIn;
			_displayName = displayName;
			_craftTime = craftTime;
			_ingredients = ingredients;
			_products = products;
		}

		public override int GetHashCode()
		{
			return id.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj == this)
			{
				return true;
			}
			if (obj is JSONRecipe other)
			{
				return id.Equals(other.id);
			}
			return false;
		}

		private ImmutableDictionary<Item, decimal> GetIngredients(ImmutableDictionary<string, Item> items)
		{
			if (_ingredients.Length == 0)
			{
				return ImmutableDictionary<Item, decimal>.Empty;
			}
			Dictionary<Item, decimal> ingredientsList = new Dictionary<Item, decimal>();
			string[] ingredientsArray = _ingredients.Split(',');
			Trace.Assert(ingredientsArray.Length % 2 == 0);
			for (int i = 0; i < ingredientsArray.Length; i += 2)
			{
				ItemCount<Item> itemCount = ParseItemCount(ingredientsArray[i], ingredientsArray[i + 1], items);
				ingredientsList.Add(itemCount.item, itemCount.rate);
			}
			return ingredientsList.ToImmutableDictionary();
		}

		private ImmutableDictionary<Item, decimal> GetProducts(ImmutableDictionary<string, Item> items)
		{
			Dictionary<Item, decimal> productsList = new Dictionary<Item, decimal>();
			string[] productsArray = _products.Split(',');
			Trace.Assert(productsArray.Length % 2 == 0);
			for (int i = 0; i < productsArray.Length; i += 2)
			{
				ItemCount<Item> itemCount = ParseItemCount(productsArray[i], productsArray[i + 1], items);
				productsList.Add(itemCount.item, itemCount.rate);
			}
			return productsList.ToImmutableDictionary();
		}

		private static ItemCount<Item> ParseItemCount(string id, string count, ImmutableDictionary<string, Item> items)
		{
			id = id.Substring(id.LastIndexOf(".") + 1);
			id = id.Substring(0, id.LastIndexOf("'"));
			count = count.Replace(")", "");
			count = count.Remove(0, "Amount=".Length);
			decimal itemCount = int.Parse(count);
			return new ItemCount<Item>(items[id], itemCount);
		}

		public virtual Recipe Process(ImmutableDictionary<string, Item> items, ImmutableDictionary<string, Building> buildings)
		{
			ImmutableDictionary<Item, decimal> ingredients = GetIngredients(items);
			ImmutableDictionary<Item, decimal> products = GetProducts(items);
			string conversionString = GetConversionString(ingredients, products);
			Building building = GetBuilding(buildings);
			return new Recipe(id, _displayName, conversionString, _craftTime, building, ingredients, products);
		}

		public Building GetBuilding(ImmutableDictionary<string, Building> buildings)
		{
			string[] machines = Utility.Util.ParseUIDList(_producedIn);
			foreach (string machineUID in machines)
			{
				if (buildings.ContainsKey(machineUID))
				{
					return buildings[machineUID];
				}
			}
			return default;
		}

		protected virtual string GetConversionString(ImmutableDictionary<Item, decimal> ingredients, ImmutableDictionary<Item, decimal> products)
		{
			string str = "";
			bool first = true;
			foreach (KeyValuePair<Item, decimal> pair in ingredients)
			{
				if (!first)
				{
					str += ", ";
				}
				else
				{
					first = false;
				}
				str += pair.Key.ToString(pair.Value);
			}
			if (ingredients.Count > 0)
			{
				str += " -> ";
				first = true;
			}
			foreach (KeyValuePair<Item, decimal> pair in products)
			{
				if (!first)
				{
					str += ", ";
				}
				else
				{
					first = false;
				}
				str += pair.Key.ToString(pair.Value);
			}
			return str;
		}
	}
}
