using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSatisfactoryCalculator.code.Interfaces;

namespace VisualSatisfactoryCalculator.code.DataStorage
{
	public class SimpleCustomItem : IItem
	{
		private readonly string UID;
		private readonly string displayName;

		public SimpleCustomItem(string UID, string displayName)
		{
			this.UID = UID;
			this.displayName = displayName;
		}

		public override string ToString()
		{
			return displayName;
		}

		public override int GetHashCode()
		{
			return UID.GetHashCode();
		}

		public bool EqualID(string id)
		{
			return UID.Equals(id);
		}

		public bool Equals(IItem other)
		{
			if (!(other is SimpleCustomItem)) return false;
			return UID.Equals((other as SimpleCustomItem).UID);
		}
	}
}
