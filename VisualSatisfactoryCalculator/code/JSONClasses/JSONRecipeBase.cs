using System;
using System.Collections.Generic;

using VisualSatisfactoryCalculator.satisfactory.Extensions;
using VisualSatisfactoryCalculator.satisfactory.Interfaces;
using VisualSatisfactoryCalculator.satisfactory.Numbers;
using VisualSatisfactoryCalculator.satisfactory.Utility;
using VisualSatisfactoryCalculator.model.production;
using VisualSatisfactoryCalculator.satisfactory.JSONClasses;
using System.Collections.Immutable;
using VisualSatisfactoryCalculator.code.Utility;
using VisualSatisfactoryCalculator.satisfactory.model.production;
using System.Windows.Forms;
using System.Diagnostics;

namespace VisualSatisfactoryCalculator.satisfactory.DataStorage
{
	public class JSONRecipeBase
	{
		public readonly string id;
		private readonly string _producedIn;
		private readonly string _displayName;
		private readonly RationalNumber _craftTime;
		private readonly string _ingredients;
		private readonly string _products;

		public JSONRecipeBase(string id, RationalNumber craftTime, string producedIn, string ingredients, string products, string displayName)
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
			if (obj is JSONRecipeBase other)
			{
				return id.Equals(other.id);
			}
			return false;
		}

		private ImmutableDictionary<Item, RationalNumber> GetIngredients(ImmutableDictionary<string, Item> items)
		{
			Dictionary<Item, RationalNumber> ingredientsList = new Dictionary<Item, RationalNumber>();
			string[] ingredientsArray = _ingredients.Split(',');
			Trace.Assert(ingredientsArray.Length % 2 == 0);
			for (int i = 0; i < ingredientsArray.Length; i += 2)
			{
				ItemCount<Item> itemCount = ParseItemCount(ingredientsArray[i], ingredientsArray[i + 1], items);
				ingredientsList.Add(itemCount.item, itemCount.rate);
			}
			return ingredientsList.ToImmutableDictionary();
		}

		private ImmutableDictionary<Item, RationalNumber> GetProducts(ImmutableDictionary<string, Item> items)
		{
			Dictionary<Item, RationalNumber> productsList = new Dictionary<Item, RationalNumber>();
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
			id = id.Substring(id.IndexOf(".") + 1);
			id = id.Substring(0, id.LastIndexOf("\""));
			count = count.Replace(")", "");
			count = count.Remove(0, "Amount=".Length);
			RationalNumber itemCount = int.Parse(count);
			return new ItemCount<Item>(items[id], itemCount);
		}

		public virtual Recipe Process(ImmutableDictionary<string, Item> items, ImmutableDictionary<string, Building> buildings)
		{
			ImmutableDictionary<Item, RationalNumber> ingredients = GetIngredients(items);
			ImmutableDictionary<Item, RationalNumber> products = GetProducts(items);
			string conversionString = GetConversionString(ingredients, products);
			Building building = GetBuilding(buildings);
			return new Recipe(id, GetDisplayName(conversionString, building), conversionString, _craftTime, building, ingredients, products);
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

		protected virtual string GetConversionString(ImmutableDictionary<Item, RationalNumber> ingredients, ImmutableDictionary<Item, RationalNumber> products)
		{
			string str = "";
			bool first = true;
			foreach (KeyValuePair<Item, RationalNumber> pair in ingredients)
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
			str += " -> ";
			first = true;
			foreach (KeyValuePair<Item, RationalNumber> pair in products)
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

		protected virtual string GetDisplayName(string conversionString, Building building)
		{
			string str = _displayName + ": ";
			str += conversionString;
			str += " in " + _craftTime.ToString() + " seconds using a " + building.displayName;
			return str;
		}
	}
}
