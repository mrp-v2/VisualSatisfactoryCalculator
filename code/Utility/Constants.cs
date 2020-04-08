using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSatisfactoryCalculator.code.DataStorage;
using VisualSatisfactoryCalculator.code.Interfaces;

namespace VisualSatisfactoryCalculator.code.Utility
{
	public static class Constants
	{
		public static readonly IItem MWItem = new SimpleCustomItem("FillerEnergyItemMW", "MW");
		public static readonly decimal GeneratorEnergyDivisor = 16m + 2m / 3m;
	}
}
