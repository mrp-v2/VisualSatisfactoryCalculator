using System;
using System.Collections.Generic;
using VisualSatisfactoryCalculator.code.Interfaces;

namespace VisualSatisfactoryCalculator.code.DataStorage
{
	public class ItemCount
	{
		public decimal Count { get; }
		public string ItemUID { get; }

		public ItemCount(string itemUID, decimal count)
		{
			ItemUID = itemUID;
			Count = count;
		}

		public string ToString(Dictionary<string, IEncoder> encodings)
		{
			if ((encodings[ItemUID] as IItem).IsLiquid) return Math.Round(Count / 1000, 3) + " " + encodings[ItemUID].DisplayName;
			return Math.Round(Count, 3) + " " + encodings[ItemUID].DisplayName;
		}

		public override int GetHashCode()
		{
			return ItemUID.GetHashCode() * Count.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (!(obj is ItemCount)) return false;
			return ItemUID.Equals((obj as ItemCount).ItemUID) && Count == (obj as ItemCount).Count;
		}

		public ItemCount Inverse()
		{
			return new ItemCount(ItemUID, -Count);
		}
	}
}
