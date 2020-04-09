using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VisualSatisfactoryCalculator.code.Interfaces;

namespace VisualSatisfactoryCalculator.code.JSONClasses
{
	class JSONBuilding : IBuilding
	{
		private readonly string UID;
		private readonly decimal powerConsumption;
		private readonly string displayName;

		[JsonConstructor]
		public JSONBuilding(string ClassName, string mPowerConsumption, string mDisplayName)
		{
			UID = ClassName;
			powerConsumption = decimal.Parse(mPowerConsumption);
			displayName = mDisplayName;
		}

		public bool EqualID(string id)
		{
			return UID.Equals(id);
		}

		public bool EqualID(IHasUID obj)
		{
			return obj.EqualID(UID);
		}

		public string GetDisplayName()
		{
			return displayName;
		}

		public decimal GetPowerConsumption()
		{
			return powerConsumption;
		}

		public string GetUID()
		{
			return UID;
		}
	}
}
