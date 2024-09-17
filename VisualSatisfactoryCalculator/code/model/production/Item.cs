using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VisualSatisfactoryCalculator.model.production;
using VisualSatisfactoryCalculator.satisfactory.Numbers;

namespace VisualSatisfactoryCalculator.satisfactory.model.production
{
	public class Item : BasicItem
	{
		private readonly bool _isFluid;

		public Item(string id, string displayName, bool isFluid) : base(id, displayName)
		{
			_isFluid = isFluid;
		}

		public string ToString(RationalNumber count)
		{
			if (_isFluid)
			{
				return (count / 1000) + " " + ToString();
			}
			else
			{
				return count + " " + ToString();
			}
		}
	}
}
