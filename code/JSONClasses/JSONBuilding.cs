using Newtonsoft.Json;

using VisualSatisfactoryCalculator.code.Interfaces;

namespace VisualSatisfactoryCalculator.code.JSONClasses
{
	internal class JSONBuilding : IBuilding
	{
		public string UID { get; }
		public decimal PowerConsumption { get; }
		public decimal PowerConsumptionExponent { get; }
		public string DisplayName { get; }

		[JsonConstructor]
		public JSONBuilding(string ClassName, string mPowerConsumption, string mPowerConsumptionExponent, string mDisplayName)
		{
			UID = ClassName;
			PowerConsumption = decimal.Parse(mPowerConsumption);
			PowerConsumptionExponent = decimal.Parse(mPowerConsumptionExponent);
			DisplayName = mDisplayName;
		}

		public bool EqualID(string id)
		{
			return UID.Equals(id);
		}

		public bool EqualID(IHasUID obj)
		{
			return obj.EqualID(UID);
		}
	}
}
