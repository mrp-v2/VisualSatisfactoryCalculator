using System.Collections.Generic;

using VisualSatisfactoryCalculator.satisfactory.DataStorage;
using VisualSatisfactoryCalculator.satisfactory.Interfaces;
using VisualSatisfactoryCalculator.satisfactory.JSONClasses;

namespace VisualSatisfactoryCalculator.satisfactory.Utility
{
	public static class Constants
	{
		public const int CLOCK_DECIMALS = 4;

		public static readonly JSONItem MW_ITEM = new JSONItem("FillerEnergyItemMW", "MW", "POWER", false, 1.0m);

		public static JsonEncodings FALLBACK_ENCODINGS = new JsonEncodings();

		public static readonly string WATER_ID = "Desc_Water_C";
	}
}
