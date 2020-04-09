using System;
using System.Collections.Generic;
using VisualSatisfactoryCalculator.code.Extensions;
using VisualSatisfactoryCalculator.code.Interfaces;

namespace VisualSatisfactoryCalculator.code.DataStorage
{
	class SimpleCustomRecipe : IRecipe
	{
		private readonly string UID;
		private readonly decimal craftTime;
		private readonly string machineUID;
		private readonly List<ItemCount> itemCounts;
		private readonly string displayName;

		public SimpleCustomRecipe(string UID, decimal craftTime, string machineUID, List<ItemCount> itemCounts, string displayName)
		{
			this.UID = UID;
			this.craftTime = craftTime;
			this.machineUID = machineUID;
			this.itemCounts = itemCounts;
			this.displayName = displayName;
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

		public string GetMachineUID()
		{
			return machineUID;
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

		public string GetUID()
		{
			return UID;
		}

		public bool EqualID(IHasUID obj)
		{
			return obj.EqualID(UID);
		}

		public string GetDisplayName()
		{
			return displayName;
		}
	}
}
