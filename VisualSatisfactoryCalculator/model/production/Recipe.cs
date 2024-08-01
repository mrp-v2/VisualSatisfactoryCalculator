using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VisualSatisfactoryCalculator.code.Numbers;

namespace VisualSatisfactoryCalculator.model.production
{
	public class Recipe<ItemType> where ItemType : AbstractItem
	{
		private readonly RationalNumber time;
		private readonly Dictionary<ItemType, RationalNumber> ingredients;
		private readonly Dictionary<ItemType, RationalNumber> products;
	}
}
