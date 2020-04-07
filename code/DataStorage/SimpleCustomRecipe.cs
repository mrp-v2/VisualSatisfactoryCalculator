using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSatisfactoryCalculator.code.Interfaces;
using VisualSatisfactoryCalculator.code.JSONClasses;

namespace VisualSatisfactoryCalculator.code.DataStorage
{
	class SimpleCustomRecipe : IRecipe
	{
		private readonly string uniqueID;
		private readonly decimal craftTime;
		private readonly string machine;
		private readonly List<ItemCount> itemCounts;

		public SimpleCustomRecipe(string uniqueID, decimal craftTime, string machine, List<ItemCount> itemCounts)
		{
			this.uniqueID = uniqueID;
			this.craftTime = craftTime;
			this.machine = machine;
			this.itemCounts = itemCounts;
		}

		public IRecipe Clone()
		{
			return new SimpleCustomRecipe(uniqueID, craftTime, machine, itemCounts);
		}

		public bool EqualID(string id)
		{
			return uniqueID.Equals(id);
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
			return uniqueID.Equals((other as SimpleCustomRecipe).uniqueID);
		}
	}
}
