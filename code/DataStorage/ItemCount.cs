using System;
using VisualSatisfactoryCalculator.code.Interfaces;
using VisualSatisfactoryCalculator.code.JSONClasses;

namespace VisualSatisfactoryCalculator.code.DataStorage
{
	[Serializable]
	public class ItemCount : JSONItem, IMyCloneable<ItemCount>, ICastAndCopy<JSONItem>
	{
		protected readonly int count;

		public ItemCount(JSONItem item, int count) : base(item)
		{
			this.count = count;
		}

		public int GetCount()
		{
			return count;
		}

		public override string ToString()
		{
			return count + " " + base.ToString();
		}

		public string ItemToString()
		{
			return base.ToString();
		}

		public override int GetHashCode()
		{
			return base.GetHashCode() * count;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is JSONItem))
			{
				return false;
			}
			if (!base.Equals(obj as JSONItem))
			{
				return false;
			}
			if (!(obj is ItemCount))
			{
				return false;
			}
			return count == (obj as ItemCount).count;
		}

		public ItemCount Inverse()
		{
			return new ItemCount(this, -count);
		}

		public ItemCount Clone()
		{
			return new ItemCount(this, count);
		}

		public JSONItem CastAndCopy()
		{
			return new JSONItem(this);
		}
	}
}
