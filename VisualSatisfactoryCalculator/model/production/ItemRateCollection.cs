using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.model.production
{
	public sealed class ItemRateCollection<ItemType> : Dictionary<ItemType, ItemCount<ItemType>> where ItemType : BasicItem
	{
		public void Add(ItemCount<ItemType> rate)
		{
			Add(rate.item, rate);
		}
	}
}
