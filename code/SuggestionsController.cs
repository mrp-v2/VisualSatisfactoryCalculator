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

		public SuggestionsController()
		{
			items = new List<string>();
			machines = new List<string>();
		}

		public List<string> GetItems()
		{
			return items;
		}

		public List<string> GetMachines()
		{
			return machines;
		}

		public void AddItem(string item)
		{
			items.Add(item);
		}

		public void AddMachine(string machine)
		{
			machines.Add(machine);
		}
	}
}
