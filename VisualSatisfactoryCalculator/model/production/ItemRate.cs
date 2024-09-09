using System;

using VisualSatisfactoryCalculator.satisfactory.Interfaces;
using VisualSatisfactoryCalculator.satisfactory.Numbers;
using VisualSatisfactoryCalculator.satisfactory.Utility;

namespace VisualSatisfactoryCalculator.model.production
{
	[Serializable]
	public sealed class ItemRate<ItemType> where ItemType : AbstractItem
	{
		public readonly RationalNumber rate;
		public readonly ItemType item;

		public ItemRate(ItemType item, RationalNumber rate)
		{
			this.item = item;
			this.rate = rate;
		}

		public override string ToString()
		{
			return rate + " " + item.ToString();
		}

		public override int GetHashCode()
		{
			return item.GetHashCode() * rate.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj is ItemRate<ItemType> other)
			{
				return item.Equals(other.item) && rate == other.rate;
			}
			else
			{
				return false;
			}
		}

		public static ItemRate<ItemType> operator *(ItemRate<ItemType> rate, RationalNumber multiplier)
		{
			return new ItemRate<ItemType>(rate.item, rate.rate * multiplier);
		}

		public static ItemRate<ItemType> operator +(ItemRate<ItemType> a, RationalNumber b)
		{
			return new ItemRate<ItemType>(a.item, a.rate + b);
		}

		public static ItemRate<ItemType> operator -(ItemRate<ItemType> a, RationalNumber b)
		{
			return new ItemRate<ItemType>(a.item, a.rate - b);
		}
	}
}
