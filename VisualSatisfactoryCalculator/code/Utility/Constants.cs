using System.Collections.Generic;

using VisualSatisfactoryCalculator.satisfactory.DataStorage;
using VisualSatisfactoryCalculator.satisfactory.Interfaces;
using VisualSatisfactoryCalculator.satisfactory.JSONClasses;

namespace VisualSatisfactoryCalculator.satisfactory.Utility
{
	public static class Constants
	{
		public const int CLOCK_DECIMALS = 4;

		public static readonly JSONItem MWItem = new JSONItem("FillerEnergyItemMW", "MW", "POWER", false, 1.0m);

		public static readonly Dictionary<string, IEncoder> AllConstantEncoders = new Dictionary<string, IEncoder>() { { MWItem.id, MWItem } };

		public static Encodings LastResortEncoderList = new Encodings();

		public static readonly string WaterID = "Desc_Water_C";
	}
}
