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

		public ItemRate(ItemType item, RationalNumber rate)
		{
			Item = item;
			Rate = rate;
		}

		public override string ToString()
		{
			return Rate + " " + Item.ToString();
		}

		public override int GetHashCode()
		{
			return Item.GetHashCode() * Rate.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj is ItemRate<ItemType> other)
			{
				return Item.Equals(other.Item) && Rate == other.Rate;
			}
			else
			{
				return false;
			}
		}

		public static ItemRate<ItemType> operator *(ItemRate<ItemType> rate, RationalNumber multiplier)
		{
			return new ItemRate<ItemType>(rate.Item, rate.Rate * multiplier);
		}
	}
}
