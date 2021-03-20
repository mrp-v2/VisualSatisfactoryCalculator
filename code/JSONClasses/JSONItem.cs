using Newtonsoft.Json;

using VisualSatisfactoryCalculator.code.Extensions;
using VisualSatisfactoryCalculator.code.Interfaces;
using VisualSatisfactoryCalculator.code.Utility;

namespace VisualSatisfactoryCalculator.code.JSONClasses
{
	public class JSONItem : IItem, IFromJson
	{
		public string UID { get; }
		public string DisplayName { get; }
		public string Form { get; }
		public decimal EnergyValue { get; }
		public bool IsLiquid { get { return Form.Equals("RF_LIQUID"); } }
		public string DocsCategory { get; }
		public string NativeClass { get; }

		[JsonConstructor]
		public JSONItem(string ClassName, string mDisplayName, string mForm, string mEnergyValue)
		{
			UID = ClassName;
			DisplayName = mDisplayName;
			Form = mForm;
			EnergyValue = decimal.Parse(mEnergyValue);
			NativeClass = FileInteractor.ActiveNativeClass;
		}

		public JSONItem(JSONItem item) : this(item.UID, item.DisplayName, item.Form, item.EnergyValue.ToString()) { }

		public override int GetHashCode()
		{
			return UID.GetHashCode();
		}

		public bool EqualID(string id)
		{
			return UID.Equals(id);
		}

		public bool Equals(IItem other)
		{
			if (other == null)
			{
				return false;
			}
			if (!(other is JSONItem))
			{
				return false;
			}
			return EqualID(other);
		}

		public bool EqualID(IHasUID obj)
		{
			return obj.EqualID(UID);
		}

		public string ToString(decimal rate)
		{
			return (IsLiquid ? rate / 1000 : rate).ToPrettyString();
		}
	}
}
