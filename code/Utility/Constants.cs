using System.Collections.Generic;

using VisualSatisfactoryCalculator.code.DataStorage;
using VisualSatisfactoryCalculator.code.Interfaces;

namespace VisualSatisfactoryCalculator.code.Utility
{
	public static class Constants
	{
		public static readonly IItem MWItem = new SimpleCustomItem("FillerEnergyItemMW", "MW");

		public static readonly Dictionary<string, IEncoder> AllConstantEncoders = new Dictionary<string, IEncoder>() { { MWItem.UID, MWItem } };

		public static Dictionary<string, IEncoder> LastResortEncoderList = new Dictionary<string, IEncoder>();
	}
}
