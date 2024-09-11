using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.model.production
{
	/// <summary>
	/// A map of items to <see cref="ItemCount{ItemType}"/>s.
	/// </summary>
	public sealed class ItemCountCollection<ItemType> : Dictionary<ItemType, ItemCount<ItemType>> where ItemType : BasicItem
	{
		public void Add(ItemCount<ItemType> rate)
		{
			Add(rate.item, rate);
		}
	}
}
