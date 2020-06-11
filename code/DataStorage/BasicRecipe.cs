using System;
using System.Collections.Generic;

using VisualSatisfactoryCalculator.code.Interfaces;
using VisualSatisfactoryCalculator.code.Utility;

namespace VisualSatisfactoryCalculator.code.DataStorage
{
	public class BasicRecipe : IRecipe
	{
		public string UID { get; }
		public decimal CraftTime { get; }
		public string MachineUID { get; }
		public Dictionary<string, ItemCount> Ingredients { get; }
		public Dictionary<string, ItemCount> Products { get; }
		public string DisplayName { get; }

		public BasicRecipe(string UID, decimal craftTime, string machineUID, List<ItemCount> ingredients, List<ItemCount> products, string displayName)
		{
			this.UID = UID;
			CraftTime = craftTime;
			MachineUID = machineUID;
			Ingredients = new Dictionary<string, ItemCount>();
			foreach (ItemCount itemCount in ingredients)
			{
				Ingredients.Add(itemCount.ItemUID, itemCount);
			}
			Products = new Dictionary<string, ItemCount>();
			foreach (ItemCount itemCount in products)
			{
				Products.Add(itemCount.ItemUID, itemCount);
			}
			DisplayName = displayName;
		}

		public bool EqualID(string id)
		{
			return UID.Equals(id);
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
			return UID.GetHashCode();
		}

		public string ToString(Dictionary<string, IEncoder> encodings)
		{
			string str = DisplayName + ": ";
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
			str += " in " + Math.Round(CraftTime, 3) + " seconds using a " + encodings[MachineUID].DisplayName;
			return str;
		}

		public bool EqualID(IHasUID obj)
		{
			return obj.EqualID(UID);
		}

		public override string ToString()
		{
			return ToString(Constants.LastResortEncoderList);
		}

		public decimal GetCountFor(string itemUID, bool isProduct)
		{
			if (isProduct)
			{
				return Products[itemUID].Count;
			}
			else
			{
				return Ingredients[itemUID].Count;
			}
		}
	}
}
