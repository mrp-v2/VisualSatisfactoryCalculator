using Newtonsoft.Json;
using System.Collections.Generic;
using VisualSatisfactoryCalculator.code.Interfaces;

namespace VisualSatisfactoryCalculator.code.JSONClasses
{
	public class JSONItem : IItem
	{
		private readonly string uniqueID;
		private readonly string displayName;
		private readonly string form;
		private readonly string energyValue;


		[JsonConstructor]
		public JSONItem(string ClassName, string mDisplayName, string mForm, string mEnergyValue)
		{
			uniqueID = ClassName;
			displayName = mDisplayName;
			form = mForm;
			energyValue = mEnergyValue;
		}

		public JSONItem(JSONItem item) : this(item.uniqueID, item.displayName, item.form, item.energyValue) { }

		public override int GetHashCode()
		{
			return uniqueID.GetHashCode();
		}

		public bool EqualID(string id)
		{
			return uniqueID.Equals(id);
		}

		public override string ToString()
		{
			return displayName;
		}

		public bool IsLiquid()
		{
			return form == "RF_LIQUID";
		}

		public decimal GetEnergyValue()
		{
			return decimal.Parse(energyValue);
		}

		public string GetDisplayName()
		{
			return displayName;
		}

		public string GetForm()
		{
			return form;
		}

		public bool Equals(IItem other)
		{
			if (other == null) return false;
			if (!(other is JSONItem)) return false;
			return uniqueID.Equals((other as JSONItem).uniqueID);
		}
	}
}
