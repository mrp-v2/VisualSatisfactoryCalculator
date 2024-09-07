using System.Collections.Generic;

using VisualSatisfactoryCalculator.satisfactory.DataStorage;
using VisualSatisfactoryCalculator.satisfactory.Interfaces;

namespace VisualSatisfactoryCalculator.satisfactory.Utility
{
	public static class Constants
	{
		public const int CLOCK_DECIMALS = 4;

		public static readonly IItem MWItem = new SimpleCustomItem("FillerEnergyItemMW", "MW");

		public static readonly Dictionary<string, IEncoder> AllConstantEncoders = new Dictionary<string, IEncoder>() { { MWItem.ID, MWItem } };

		public static Encodings LastResortEncoderList = new Encodings();

		public static readonly string WaterID = "Desc_Water_C";
	}
}
