using System;

using VisualSatisfactoryCalculator.satisfactory.model.production;

namespace VisualSatisfactoryCalculator.satisfactory.Utility
{
	public static class Constants
	{
		public const byte CLOCK_SPEED_DECIMALS = 4;
		public static uint CLOCK_SPEED_DECIMAL_FACTOR = (uint)Math.Pow(10, CLOCK_SPEED_DECIMALS);

		public static readonly Item MW_ITEM = new Item("FillerEnergyItemMW", "MW", false);

		public static JsonEncodings FALLBACK_ENCODINGS = new JsonEncodings();

		public static readonly string WATER_ID = "Desc_Water_C";
	}
}
