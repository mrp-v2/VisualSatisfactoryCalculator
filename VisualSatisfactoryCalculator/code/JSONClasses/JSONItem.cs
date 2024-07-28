using Newtonsoft.Json;

using VisualSatisfactoryCalculator.code.Interfaces;
using VisualSatisfactoryCalculator.code.Utility;

namespace VisualSatisfactoryCalculator.code.JSONClasses
{
	public class JSONItem : IItem, IFromJson
	{
		public string ID { get; }
		public string DisplayName { get; }
		public string Form { get; }
		public decimal EnergyValue { get; }
		public bool IsFluid { get { return Form.Equals("RF_LIQUID") || Form.Equals("RF_GAS"); } }
		public string NativeClass { get; }

		[JsonConstructor]
		public JSONItem(string ClassName, string mDisplayName, string mForm, string mEnergyValue)
		{
			ID = ClassName;
			DisplayName = mDisplayName;
			Form = mForm;
			EnergyValue = decimal.Parse(mEnergyValue);
			NativeClass = FileInteractor.ActiveNativeClass;
		}

		public JSONItem(JSONItem item) : this(item.ID, item.DisplayName, item.Form, item.EnergyValue.ToString()) { }

		public override int GetHashCode()
		{
			return ID.GetHashCode();
		}

		public bool EqualID(string id)
		{
			return ID.Equals(id);
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

		public bool EqualID(IHasID obj)
		{
			return obj.EqualID(ID);
		}
	}
}
