using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.model.production
{
	public class AbstractItem
	{
		public readonly string id;
		public readonly string displayName;

		protected AbstractItem(string id, string displayName)
		{
			this.id = id;
			this.displayName = displayName;
		}

		public override int GetHashCode()
		{
			return id.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return ReferenceEquals(this, obj);
		}

		public override string ToString()
		{
			return displayName;
		}
	}
}
