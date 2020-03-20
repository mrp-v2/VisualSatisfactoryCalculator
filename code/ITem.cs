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
		protected readonly string item;

		public Item(string item)
		{
			this.item = item;
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
			return item.Equals((obj as Item).item);
		}

		public override string ToString()
		{
			return item;
		}

		public string ToItemString()
		{
			return item;
		}
	}
}
