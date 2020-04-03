﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VisualSatisfactoryCalculator.code
{
	public class JSONItem : IEqualityComparer<JSONItem>
	{
		protected string ClassName;
		protected string mDisplayName;
		protected string mForm;

		[JsonConstructor]
		public JSONItem(string ClassName, string mDisplayName, string mForm)
		{
			this.ClassName = ClassName;
			this.mDisplayName = mDisplayName;
			this.mForm = mForm;
		}

		public JSONItem(JSONItem item) : this(item.ClassName, item.mDisplayName, item.mForm) { }

		public override int GetHashCode()
		{
			return ClassName.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (!(obj is JSONItem)) return false;
			return ClassName.Equals((obj as JSONItem).ClassName);
		}

		public bool EqualID(string id)
		{
			return ClassName.Equals(id);
		}

		public override string ToString()
		{
			return mDisplayName;
		}

		public bool IsLiquid()
		{
			return mForm == "RF_LIQUID";
		}

		public bool Equals(JSONItem x, JSONItem y)
		{
			return x.Equals(y);
		}

		public int GetHashCode(JSONItem obj)
		{
			return obj.GetHashCode();
		}

		public static readonly JSONItem blank = new JSONItem(null, null, null);
	}
}
