using System;

using VisualSatisfactoryCalculator.satisfactory.model.production;

namespace VisualSatisfactoryCalculator.satisfactory.Utility
{
	public static class Constants
	{
		public const byte CLOCK_SPEED_DECIMALS = 4;
		public static readonly uint CLOCK_SPEED_PERCENT_FACTOR = (uint)Math.Pow(10, CLOCK_SPEED_DECIMALS);
		public static readonly uint CLOCK_SPEED_FACTOR = CLOCK_SPEED_PERCENT_FACTOR * 100;
		public static readonly decimal MINIMUM_CLOCK_SPEED_CHANGE = (decimal)Math.Pow(10, -CLOCK_SPEED_DECIMALS);

		public static readonly Item MW_ITEM = new Item("FillerEnergyItemMW", "MW", false);

		public static readonly string WATER_ID = "Desc_Water_C";
	}
}
