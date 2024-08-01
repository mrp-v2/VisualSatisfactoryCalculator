using System;

using VisualSatisfactoryCalculator.code.Interfaces;
using VisualSatisfactoryCalculator.code.Numbers;
using VisualSatisfactoryCalculator.code.Utility;

namespace VisualSatisfactoryCalculator.model.production
{
	[Serializable]
	public class ItemRate
	{
		public RationalNumber Rate { get; }
		public string ItemUID { get; }

		public ItemRate(string itemUID, RationalNumber rate)
		{
			ItemUID = itemUID;
			Rate = rate;
		}

		public string ToString(Encodings encodings)
		{
			IItem item = encodings[ItemUID] as IItem;
			return Rate + " " + item.DisplayName;
		}

		public override int GetHashCode()
		{
			return ItemUID.GetHashCode() * Rate.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj is ItemRate other)
			{
				return ItemUID.Equals(other.ItemUID) && Rate == other.Rate;
			}
			else
			{
				return false;
			}
		}
	}
}
