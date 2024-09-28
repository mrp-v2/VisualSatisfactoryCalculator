using System;

namespace VisualSatisfactoryCalculator.model.production
{
	/// <summary>
	/// Represents an item and an amount of that item.
	/// </summary>
	/// <typeparam name="ItemType"></typeparam>
	[Serializable]
	public sealed class ItemCount<ItemType> where ItemType : BasicItem
	{
		public readonly decimal rate;
		public readonly ItemType item;

		public ItemCount(ItemType item, decimal rate)
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
			if (obj is ItemCount<ItemType> other)
			{
				return item.Equals(other.item) && rate == other.rate;
			}
			else
			{
				return false;
			}
		}

		public static ItemCount<ItemType> operator *(ItemCount<ItemType> rate, decimal multiplier)
		{
			return new ItemCount<ItemType>(rate.item, rate.rate * multiplier);
		}

		public static ItemCount<ItemType> operator +(ItemCount<ItemType> a, decimal b)
		{
			return new ItemCount<ItemType>(a.item, a.rate + b);
		}

		public static ItemCount<ItemType> operator -(ItemCount<ItemType> a, decimal b)
		{
			return new ItemCount<ItemType>(a.item, a.rate - b);
		}
	}
}
