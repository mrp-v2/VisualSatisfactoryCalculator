using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.code
{
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
	}
}
