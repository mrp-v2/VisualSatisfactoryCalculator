using System;
using System.Collections.Generic;

using VisualSatisfactoryCalculator.code.Extensions;
using VisualSatisfactoryCalculator.code.Interfaces;
using VisualSatisfactoryCalculator.code.Numbers;
using VisualSatisfactoryCalculator.code.Utility;
using VisualSatisfactoryCalculator.model.production;

namespace VisualSatisfactoryCalculator.code.DataStorage
{
	public class BasicRecipe : IRecipe
	{
		public string ID { get; }
		public RationalNumber CraftTime { get; }
		public string MachineUID { get; }
		public Dictionary<string, ItemRate> Ingredients { get; }
		public Dictionary<string, ItemRate> Products { get; }
		public string DisplayName { get; }

		public BasicRecipe(string ID, RationalNumber craftTime, string machineUID, List<ItemRate> ingredients, List<ItemRate> products, string displayName)
		{
			this.ID = ID;
			CraftTime = craftTime;
			MachineUID = machineUID;
			Ingredients = new Dictionary<string, ItemRate>();
			foreach (ItemRate itemCount in ingredients)
			{
				Ingredients.Add(itemCount.ItemUID, itemCount);
			}
			Products = new Dictionary<string, ItemRate>();
			foreach (ItemRate itemCount in products)
			{
				Products.Add(itemCount.ItemUID, itemCount);
			}
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

		protected virtual string GetConversionString(Encodings encodings)
		{
			string str = "";
			bool first = true;
			foreach (string key in Ingredients.Keys)
			{
				if (!first)
				{
					str += ", ";
				}
				else
				{
					first = false;
				}
				str += Ingredients[key].ToString(encodings);
			}
			str += " -> ";
			first = true;
			foreach (string key in Products.Keys)
			{
				if (!first)
				{
					str += ", ";
				}
				else
				{
					first = false;
				}
				str += Products[key].ToString(encodings);
			}
			return str;
		}

		public string ToString(Encodings encodings)
		{
			string str = DisplayName + ": ";
			str += GetConversionString(encodings);
			str += " in " + CraftTime.ToString() + " seconds using a " + encodings[MachineUID].DisplayName;
			return str;
		}

		public bool EqualID(IHasID obj)
		{
			return obj.EqualID(ID);
		}

		public override string ToString()
		{
			return ToString(Constants.LastResortEncoderList);
		}

		/// <summary>
		/// Always positive
		/// </summary>
		public RationalNumber GetCountFor(string itemUID, bool isProduct)
		{
			if (isProduct)
			{
				return Products[itemUID].Rate;
			}
			else
			{
				return Ingredients[itemUID].Rate;
			}
		}

		public string ToString(Encodings encodings, string format)
		{
			format = format.Replace("{name}", DisplayName);
			format = format.Replace("{conversion}", GetConversionString(encodings));
			format = format.Replace("{time}", CraftTime.ToString());
			format = format.Replace("{machine}", encodings[MachineUID].DisplayName);
			return format;
		}
	}
}
