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
			foreach (ItemCount ic in itemCounts.GetIngredients())
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
	}
}
