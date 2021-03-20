using System.Collections.Generic;

using VisualSatisfactoryCalculator.code.DataStorage;
using VisualSatisfactoryCalculator.code.Interfaces;

namespace VisualSatisfactoryCalculator.code.Utility
{
	public static class Constants
	{
		public static readonly int CLOCK_DECIMALS = 4;
		public static readonly int DECIMALS = CLOCK_DECIMALS + 3;

		public static readonly IItem MWItem = new SimpleCustomItem("FillerEnergyItemMW", "MW");

		public static readonly Dictionary<string, IEncoder> AllConstantEncoders = new Dictionary<string, IEncoder>() { { MWItem.UID, MWItem } };

		public static Encodings LastResortEncoderList = new Encodings();

		public static readonly string WaterID = "Desc_Water_C";
	}
}
