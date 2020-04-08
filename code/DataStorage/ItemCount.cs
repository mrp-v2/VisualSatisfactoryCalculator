using System;
using VisualSatisfactoryCalculator.code.Interfaces;
using VisualSatisfactoryCalculator.code.JSONClasses;

namespace VisualSatisfactoryCalculator.code.DataStorage
{
	[Serializable]
	public class ItemCount : IMyCloneable<ItemCount>
	{
		private readonly decimal count;
		private readonly IItem item;

		public ItemCount(IItem item, decimal count)
		{
			this.item = item;
			this.count = count;
		}

		public decimal GetCount()
		{
			return count;
		}

		public override string ToString()
		{
			return count + " " + item.ToString();
		}

		public override int GetHashCode()
		{
			return item.GetHashCode() * count.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (!(obj is ItemCount)) return false;
			return item.Equals((obj as ItemCount).item) && count == (obj as ItemCount).count;
		}

		public ItemCount Inverse()
		{
			return new ItemCount(item, -count);
		}

		public ItemCount Clone()
		{
			return new ItemCount(item, count);
		}

		public IItem GetItem()
		{
			return item;
		}
	}
}
