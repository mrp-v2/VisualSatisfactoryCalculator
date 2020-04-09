using System.Collections.Generic;
using VisualSatisfactoryCalculator.code.Extensions;
using VisualSatisfactoryCalculator.code.Interfaces;

namespace VisualSatisfactoryCalculator.code.DataStorage
{
	class SimpleCustomRecipe : IRecipe
	{
		private readonly string UID;
		private readonly decimal craftTime;
		private readonly string machine;
		private readonly List<ItemCount> itemCounts;
		private readonly string displayName;

		public SimpleCustomRecipe(string UID, decimal craftTime, string machine, List<ItemCount> itemCounts, string displayName)
		{
			this.UID = UID;
			this.craftTime = craftTime;
			this.machine = machine;
			this.itemCounts = itemCounts;
			this.displayName = displayName;
		}

		public IRecipe Clone()
		{
			return new SimpleCustomRecipe(UID, craftTime, machine, itemCounts, displayName);
		}

		public bool EqualID(string id)
		{
			return UID.Equals(id);
		}

		public decimal GetCraftTime()
		{
			return craftTime;
		}

		public List<ItemCount> GetItemCounts()
		{
			return itemCounts;
		}

		public string GetMachine()
		{
			return machine;
		}

		public bool Equals(IRecipe other)
		{
			if (other == null) return false;
			if (!(other is SimpleCustomRecipe)) return false;
			return UID.Equals((other as SimpleCustomRecipe).UID);
		}

		public override int GetHashCode()
		{
			return UID.GetHashCode();
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
			str += " in " + craftTime + " seconds using a " + machine;
			return str;
		}

		public string GetUID()
		{
			return UID;
		}
	}
}
