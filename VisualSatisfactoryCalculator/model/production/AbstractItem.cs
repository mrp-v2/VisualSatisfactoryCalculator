using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.model.production
{
	public class AbstractItem
	{
		public readonly string ID;
		public readonly string DisplayName;

		protected AbstractItem(string id, string displayName)
		{
			ID = id;
			DisplayName = displayName;
		}

		public override int GetHashCode()
		{
			return ID.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return ReferenceEquals(this, obj);
		}

		public override string ToString()
		{
			return DisplayName;
		}
	}
}
