﻿using System.Collections.Generic;
using VisualSatisfactoryCalculator.code.DataStorage;
using VisualSatisfactoryCalculator.code.Interfaces;

namespace VisualSatisfactoryCalculator.code.Utility
{
	public static class Constants
	{
		public static readonly IItem MWItem = new SimpleCustomItem("FillerEnergyItemMW", "MW");

		public static readonly List<IEncoder> AllConstantEncoders = new List<IEncoder>() { MWItem };

		public static List<IEncoder> LastResortEncoderList = new List<IEncoder>();
	}
}
