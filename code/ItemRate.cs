using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.code
{
	public class ItemRate : Item
	{
		protected double rate;

		public ItemRate(string item, double rate) : base(item)
		{
			this.rate = rate;
		}

		public double GetRate()
		{
			return rate;
		}
	}
}
