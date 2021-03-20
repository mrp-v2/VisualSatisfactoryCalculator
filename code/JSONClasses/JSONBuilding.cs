using Newtonsoft.Json;

using VisualSatisfactoryCalculator.code.Interfaces;
using VisualSatisfactoryCalculator.code.Utility;

namespace VisualSatisfactoryCalculator.code.JSONClasses
{
	internal class JSONBuilding : IBuilding, IFromJson
	{
		public string UID { get; }
		public decimal PowerConsumption { get; }
		public decimal PowerConsumptionExponent { get; }
		public string DisplayName { get; }
		public string NativeClass { get; }

		[JsonConstructor]
		public JSONBuilding(string ClassName, string mPowerConsumption, string mPowerConsumptionExponent, string mDisplayName)
		{
			UID = ClassName;
			PowerConsumption = decimal.Parse(mPowerConsumption);
			PowerConsumptionExponent = decimal.Parse(mPowerConsumptionExponent);
			DisplayName = mDisplayName;
			NativeClass = FileInteractor.ActiveNativeClass;
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
