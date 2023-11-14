using Newtonsoft.Json;

using VisualSatisfactoryCalculator.code.Interfaces;
using VisualSatisfactoryCalculator.code.Numbers;
using VisualSatisfactoryCalculator.code.Utility;

namespace VisualSatisfactoryCalculator.code.JSONClasses
{
	internal class JSONBuilding : IBuilding, IFromJson
	{
		public string ID { get; }
		public RationalNumber PowerConsumption { get; }
		public RationalNumber PowerConsumptionExponent { get; }
		public string DisplayName { get; }
		public string NativeClass { get; }

		[JsonConstructor]
		public JSONBuilding(string ClassName, string mPowerConsumption, string mPowerConsumptionExponent, string mDisplayName)
		{
			ID = ClassName;
			PowerConsumption = RationalNumber.FromDecimalString(mPowerConsumption);
			PowerConsumptionExponent = RationalNumber.FromDecimalString(mPowerConsumptionExponent);
			DisplayName = mDisplayName;
			NativeClass = FileInteractor.ActiveNativeClass;
		}

		public bool EqualID(string id)
		{
			return ID.Equals(id);
		}

		public bool EqualID(IHasID obj)
		{
			return obj.EqualID(ID);
		}
	}
}
