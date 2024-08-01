using System;

using VisualSatisfactoryCalculator.code.Interfaces;
using VisualSatisfactoryCalculator.code.Numbers;
using VisualSatisfactoryCalculator.code.Utility;

namespace VisualSatisfactoryCalculator.model.production
{
	[Serializable]
	public class ItemRate<ItemType> where ItemType : AbstractItem
	{
		public RationalNumber Rate { get; }
		public ItemType Item { get; }

		public ItemRate(ItemType itemUID, RationalNumber rate)
		{
			this.Item = itemUID;
			Rate = rate;
		}

		public override string ToString()
		{
			return Rate + " " + this.Item.ToString();
		}

		public override int GetHashCode()
		{
			return this.Item.GetHashCode() * Rate.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj is ItemRate<ItemType> other)
			{
				return this.Item.Equals(other.Item) && Rate == other.Rate;
			}
			else
			{
				return false;
			}
		}
	}
}
