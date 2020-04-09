using System.Collections.Generic;
using VisualSatisfactoryCalculator.code.DataStorage;
using VisualSatisfactoryCalculator.code.Interfaces;
using VisualSatisfactoryCalculator.code.JSONClasses;

namespace VisualSatisfactoryCalculator.code.Utility
{
	public class SuggestionsController
	{
		public static SuggestionsController SC;

		protected List<string> items;
		protected List<string> machines;

		public SuggestionsController(List<IRecipe> allRecipes)
		{
			items = new List<string>();
			machines = new List<string>();
			foreach (IRecipe rec in allRecipes)
			{
				AddMachine(rec.GetMachineUID());
				foreach (ItemCount ic in rec.GetItemCounts())
				{
					AddItem(ic.GetItemUID().ToString());
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

		public void AddRecipe(JSONRecipe recipe)
		{
			AddMachine(recipe.GetMachineUID());
			foreach (ItemCount ic in recipe.GetItemCounts())
			{
				AddItem(ic.GetItemUID().ToString());
			}
		}
	}
}
