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
		public readonly RationalNumber countDisplayFactor;

		public Item(string id, string displayName, RationalNumber countDisplayFactor) : base(id, displayName)
		{
			this.countDisplayFactor = countDisplayFactor;
		}

		public string ToString(RationalNumber count)
		{
			return (count / countDisplayFactor) + " " + ToString();
		}
	}
}
