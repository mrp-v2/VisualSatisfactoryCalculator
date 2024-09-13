using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VisualSatisfactoryCalculator.satisfactory.Numbers;

namespace VisualSatisfactoryCalculator.model.production
{
	internal static class Util
	{
		public static ItemCount<ItemType> ToCount<ItemType>(this RationalNumber count, ItemType item) where ItemType : BasicItem
		{
			return new ItemCount<ItemType>(item, count);
		}
	}
}
