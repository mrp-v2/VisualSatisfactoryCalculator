using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.code
{
	[Serializable]
	public class ItemCount : Item, ICloneable
	{
		protected int count;

		public ItemCount(string item, int count)
		{
			this.item = item;
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

		public static ItemCount FromString(string str)
		{
			return new ItemCount(str.Substring(str.IndexOf(' ') + 1), int.Parse(str.Substring(0, str.IndexOf(' '))));
		}

		public override int GetHashCode()
		{
			return base.GetHashCode() * count;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Item))
			{
				return false;
			}
			if (!base.Equals(obj as Item))
			{
				return false;
			}
			if (!(obj is ItemCount))
			{
				return false;
			}
			return count == (obj as ItemCount).count;
		}

		public ItemCount Inverse()
		{
			return new ItemCount(item, -count);
		}

		public object Clone()
		{
			return new ItemCount(item, count);
		}
	}
}
