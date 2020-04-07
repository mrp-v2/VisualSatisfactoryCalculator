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
		private readonly string uniqueID;
		private readonly string displayName;

		public SimpleCustomItem(string uniqueID, string displayName)
		{
			this.uniqueID = uniqueID;
			this.displayName = displayName;
		}

		public override string ToString()
		{
			return displayName;
		}

		public override int GetHashCode()
		{
			return uniqueID.GetHashCode();
		}

		public bool EqualID(string id)
		{
			throw new NotImplementedException();
		}

		public bool Equals(IItem other)
		{
			throw new NotImplementedException();
		}

		public string GetUniqueID()
		{
			throw new NotImplementedException();
		}
	}
}
