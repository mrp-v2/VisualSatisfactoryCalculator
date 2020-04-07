using Newtonsoft.Json;
using System.Collections.Generic;

namespace VisualSatisfactoryCalculator.code.JSONClasses
{
	public class JSONItem : IEqualityComparer<JSONItem>
	{
		protected string ClassName;
		protected string DisplayName;
		protected string Form;
		protected string EnergyValue;

		[JsonConstructor]
		public JSONItem(string ClassName, string mDisplayName, string mForm, string mEnergyValue)
		{
			this.ClassName = ClassName;
			DisplayName = mDisplayName;
			Form = mForm;
			EnergyValue = mEnergyValue;
		}

		public JSONItem(JSONItem item) : this(item.ClassName, item.DisplayName, item.Form, item.EnergyValue) { }

		public override int GetHashCode()
		{
			return GetHashCode(this);
		}

		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (!(obj is JSONItem)) return false;
			return Equals(this, obj as JSONItem);
		}

		public bool EqualID(string id)
		{
			return ClassName.Equals(id);
		}

		public override string ToString()
		{
			return DisplayName;
		}

		public bool IsLiquid()
		{
			return Form == "RF_LIQUID";
		}

		public bool Equals(JSONItem x, JSONItem y)
		{
			return x.ClassName.Equals(y.ClassName);
		}

		public int GetHashCode(JSONItem obj)
		{
			return obj.ClassName.GetHashCode();
		}

		public static readonly JSONItem comparer = new JSONItem(null, null, null, null);
	}
}
