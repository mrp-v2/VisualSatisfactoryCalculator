using System;
using System.Collections.Generic;
using VisualSatisfactoryCalculator.code.Extensions;
using VisualSatisfactoryCalculator.code.Interfaces;

namespace VisualSatisfactoryCalculator.code.DataStorage
{
	public class ItemCount
	{
		private readonly decimal count;
		private readonly string itemUID;

		public ItemCount(string itemUID, decimal count)
		{
			this.itemUID = itemUID;
			this.count = count;
		}

		public decimal GetCount()
		{
			return count;
		}

		public string ToString(List<IEncoder> encodings)
		{
			if ((encodings.FindByID(itemUID) as IItem).IsLiquid()) return Math.Round(count / 1000, 3) + " " + encodings.GetDisplayNameFor(itemUID);
			return Math.Round(count, 3) + " " + encodings.GetDisplayNameFor(itemUID);
		}

		public override int GetHashCode()
		{
			return itemUID.GetHashCode() * count.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (!(obj is ItemCount)) return false;
			return itemUID.Equals((obj as ItemCount).itemUID) && count == (obj as ItemCount).count;
		}

		public ItemCount Inverse()
		{
			return new ItemCount(itemUID, -count);
		}

		public string GetItemUID()
		{
			return itemUID;
		}
	}
}
