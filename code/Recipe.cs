using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.code
{
	public class Recipe
	{
		protected List<ItemCount> itemCounts;
		protected int craftTime;
		protected string machineName;
		
		public Recipe(List<ItemCount> itemCounts, int craftTime, string machineName)
		{
			this.itemCounts = itemCounts;
			this.craftTime = craftTime;
			this.machineName = machineName;
		}

		public override string ToString()
		{
			string str = "";
			foreach (ItemCount ic in itemCounts.GetIngredients().Inverse())
			{
				str += ic.ToString() + ", ";
			}
			str += "-> ";
			foreach (ItemCount ic in itemCounts.GetProducts())
			{
				str += ic.ToString() + ", ";
			}
			str += "in " + craftTime + " seconds using a " + machineName;
			return str;
		}

		public override int GetHashCode()
		{
			int hash = craftTime.GetHashCode() * machineName.GetHashCode();
			foreach (ItemCount ic in itemCounts)
			{
				hash += ic.GetHashCode();
			}
			return hash;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (!(obj is Recipe))
			{
				return false;
			}
			if ((obj as Recipe).craftTime != craftTime)
			{
				return false;
			}
			if ((obj as Recipe).machineName != machineName)
			{
				return false;
			}
			return (obj as Recipe).itemCounts.EqualContents(itemCounts);
		}
	}
}
