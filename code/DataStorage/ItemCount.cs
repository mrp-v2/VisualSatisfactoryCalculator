using System;
using System.Collections.Generic;

using VisualSatisfactoryCalculator.code.Extensions;
using VisualSatisfactoryCalculator.code.Interfaces;
using VisualSatisfactoryCalculator.code.Utility;

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

		public string ToString(Encodings encodings)
		{
			IItem item = encodings[ItemUID] as IItem;
			return item.ToString(Count) + " " + item.DisplayName;
		}

		public override int GetHashCode()
		{
			return ItemUID.GetHashCode() * Count.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}

			if (!(obj is ItemCount))
			{
				return false;
			}

			return ItemUID.Equals((obj as ItemCount).ItemUID) && Count == (obj as ItemCount).Count;
		}

		public ItemCount Inverse()
		{
			return new ItemCount(ItemUID, -Count);
		}
	}
}
