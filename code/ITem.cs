using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.code
{
	[Serializable]
	public class Item
	{
		protected string item;

		public Item(string item)
		{
			this.item = item;
		}

		public string GetItem()
		{
			return item;
		}

		public static Item FromString(string str)
		{
			return new Item(str);
		}

		public override string ToString()
		{
			return item;
		}

		public override int GetHashCode()
		{
			return item.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (!(obj is Item))
			{
				return false;
			}
			return item == (obj as Item).item;
		}
	}
}
