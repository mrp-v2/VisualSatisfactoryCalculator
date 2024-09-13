using System;

using Newtonsoft.Json;

using VisualSatisfactoryCalculator.model.production;
using VisualSatisfactoryCalculator.satisfactory.Interfaces;
using VisualSatisfactoryCalculator.satisfactory.model.production;
using VisualSatisfactoryCalculator.satisfactory.Numbers;
using VisualSatisfactoryCalculator.satisfactory.Utility;

namespace VisualSatisfactoryCalculator.satisfactory.JSONClasses
{
	public class JSONItem : BasicItem, IFromJson, IEquatable<JSONItem>, IEncoder
	{
		public string Form { get; }
		public decimal EnergyValue { get; }
		public bool IsFluid { get; }
		public string NativeClass { get; }
		string IHasID.ID
		{
			get
			{
				return id;
			}
		}
		string IHasDisplayName.DisplayName
		{
			get
			{
				return displayName;
			}
		}

		[JsonConstructor]
		public JSONItem(string ClassName, string mDisplayName, string mForm, string mEnergyValue) : base(ClassName, mDisplayName)
		{
			Form = mForm;
			EnergyValue = decimal.Parse(mEnergyValue);
			NativeClass = FileInteractor.ActiveNativeClass;
			IsFluid = Form.Equals("RF_LIQUID") || Form.Equals("RF_GAS");
		}

		public JSONItem(JSONItem item) : this(item.id, item.displayName, item.Form, item.EnergyValue.ToString()) { }

		public JSONItem(string id, string displayName, string form, bool isFluid, decimal energyValue) : base(id, displayName)
		{
			Form = form;
			IsFluid = isFluid;
			EnergyValue = energyValue;
			NativeClass = FileInteractor.ActiveNativeClass;
		}

		public string ToString(RationalNumber count)
		{
			if (IsFluid)
			{
				return (count / 1000) + " " + ToString();
			}
			else
			{
				return count + " " + ToString();
			}
		}

		public override int GetHashCode()
		{
			return id.GetHashCode();
		}

		public bool EqualID(string id)
		{
			return base.id.Equals(id);
		}

		public bool Equals(JSONItem other)
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
			return obj.EqualID(id);
		}

		public Item Process()
		{
			return new Item(id, displayName, IsFluid);
		}
	}
}
