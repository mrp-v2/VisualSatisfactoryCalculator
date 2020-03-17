using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.code
{
	public class ItemCount : Item
	{
		protected int count;

		public ItemCount(string item, int count) : base(item)
		{
			this.count = count;
		}

		public int GetCount()
		{
			return count;
		}

		public override string ToString()
		{
			return count + " " + base.ToString();
		}

		new public static ItemCount FromString(string str)
		{
			return new ItemCount(str.Substring(str.IndexOf(' ') + 1), int.Parse(str.Substring(0, str.IndexOf(' '))));
		}
	}
}
