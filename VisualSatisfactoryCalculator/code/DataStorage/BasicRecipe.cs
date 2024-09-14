using System;
using System.Collections.Generic;

using VisualSatisfactoryCalculator.satisfactory.Extensions;
using VisualSatisfactoryCalculator.satisfactory.Interfaces;
using VisualSatisfactoryCalculator.satisfactory.Numbers;
using VisualSatisfactoryCalculator.satisfactory.Utility;
using VisualSatisfactoryCalculator.model.production;
using VisualSatisfactoryCalculator.satisfactory.JSONClasses;

namespace VisualSatisfactoryCalculator.satisfactory.DataStorage
{
	public class BasicRecipe : BasicRecipe<JSONItem>, IRecipe
	{
		public string ID { get; }
		public string MachineUID { get; }
		public string DisplayName { get; }

		public BasicRecipe(string ID, RationalNumber craftTime, string machineUID, List<ItemCount<JSONItem>> ingredients, List<ItemCount<JSONItem>> products, string displayName) : base(craftTime, ingredients, products)
		{
			this.ID = ID;
			MachineUID = machineUID;
			DisplayName = displayName;
		}

		public bool EqualID(string id)
		{
			return ID.Equals(id);
		}

		public bool Equals(IRecipe other)
		{
			if (other == null)
			{
				return false;
			}

			if (!(other is BasicRecipe))
			{
				return false;
			}

			return EqualID(other);
		}

		public override int GetHashCode()
		{
			return ID.GetHashCode();
		}

		protected virtual string GetConversionString()
		{
			string str = "";
			bool first = true;
			foreach (JSONItem key in ingredients.Keys)
			{
				if (!first)
				{
					str += ", ";
				}
				else
				{
					first = false;
				}
				str += key.ToString(ingredients[key]);
			}
			str += " -> ";
			first = true;
			foreach (JSONItem key in products.Keys)
			{
				if (!first)
				{
					str += ", ";
				}
				else
				{
					first = false;
				}
				str += key.ToString(products[key]);
			}
			return str;
		}

		public string ToString(JsonEncodings encodings)
		{
			string str = DisplayName + ": ";
			str += GetConversionString();
			str += " in " + time.ToString() + " seconds using a " + encodings[MachineUID].DisplayName;
			return str;
		}

		public bool EqualID(IHasID obj)
		{
			return obj.EqualID(ID);
		}

		public override string ToString()
		{
			return ToString(Constants.FALLBACK_ENCODINGS);
		}

		/// <summary>
		/// Always positive
		/// </summary>
		public RationalNumber GetCountFor(JSONItem item, bool isProduct)
		{
			if (isProduct)
			{
				return products[item];
			}
			else
			{
				return ingredients[item];
			}
		}

		public string ToString(JsonEncodings encodings, string format)
		{
			format = format.Replace("{name}", DisplayName);
			format = format.Replace("{conversion}", GetConversionString());
			format = format.Replace("{time}", time.ToString());
			format = format.Replace("{machine}", encodings[MachineUID].DisplayName);
			return format;
		}
	}
}
