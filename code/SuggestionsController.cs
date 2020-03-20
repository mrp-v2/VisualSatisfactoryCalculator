using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.code
{
	public class SuggestionsController
	{
		public static SuggestionsController SC;

		protected List<string> items;
		protected List<string> machines;

		public SuggestionsController(List<Recipe> allRecipes)
		{
			items = new List<string>();
			machines = new List<string>();
			foreach (Recipe rec in allRecipes)
			{
				AddMachine(rec.GetMachine());
				foreach (ItemCount ic in rec.GetItemCounts())
				{
					AddItem((ic as Item).ToString());
				}
			}
		}

		public List<string> GetItems()
		{
			return items;
		}

		public List<string> GetMachines()
		{
			return machines;
		}

		private void AddItem(string item)
		{
			if (!items.Contains(item))
			{
				items.Add(item);
			}
		}

		private void AddMachine(string machine)
		{
			if (!machines.Contains(machine))
			{
				machines.Add(machine);
			}
		}

		public void AddRecipe(Recipe recipe)
		{
			AddMachine(recipe.GetMachine());
			foreach (ItemCount ic in recipe.GetItemCounts())
			{
				AddItem(ic.ToItemString());
			}
		}
	}
}
