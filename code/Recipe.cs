using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.code
{
	[Serializable]
	public class Recipe : ICloneable
	{
		protected List<ItemCount> itemCounts;
		protected int craftTime;
		protected string machineName;

		public Recipe(List<ItemCount> itemCounts, int craftTime, string machineName)
		{
			this.itemCounts = itemCounts;
			this.craftTime = craftTime;
			this.machineName = machineName;
		}

		public Recipe(Recipe recipe) : this(recipe.itemCounts, recipe.craftTime, recipe.machineName)
		{

		}

		public override string ToString()
		{
			string str = "";
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
			int hash = craftTime.GetHashCode() * machineName.GetHashCode();
			foreach (ItemCount ic in itemCounts)
			{
				hash += ic.GetHashCode();
			}
			return hash;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (!(obj is Recipe))
			{
				return false;
			}
			if ((obj as Recipe).craftTime != craftTime)
			{
				return false;
			}
			if ((obj as Recipe).machineName != machineName)
			{
				return false;
			}
			return (obj as Recipe).itemCounts.EqualContents(itemCounts);
		}

		public List<ItemCount> GetItemCounts()
		{
			return itemCounts;
		}

		public List<Item> GetIngredientItems()
		{
			return itemCounts.GetIngredients().GetItems();
		}

		public List<Item> GetProductItems()
		{
			return itemCounts.GetProducts().GetItems();
		}

		public ItemCount GetItemCount(Item item)
		{
			foreach (ItemCount ic in itemCounts)
			{
				if (ic.ToItem().Equals(item))
				{
					return ic;
				}
			}
			return default;
		}

		public int GetCraftTime()
		{
			return craftTime;
		}

		public string GetMachine()
		{
			return machineName;
		}

		public object Clone()
		{
			return new Recipe(itemCounts.Clone().CastToItemCountList(), craftTime, machineName);
		}
	}
}
