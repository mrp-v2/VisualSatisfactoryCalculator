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

		public override int GetHashCode()
		{
			return item.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return item.Equals(obj);
		}

		public override string ToString()
		{
			return item;
		}

		public bool SameItem(Item other)
		{
			return item.Equals(other.item);
		}

		public string ToItemString()
		{
			return item;
		}
	}
}
