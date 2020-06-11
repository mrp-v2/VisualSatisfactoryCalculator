using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Newtonsoft.Json;

using VisualSatisfactoryCalculator.code.DataStorage;
using VisualSatisfactoryCalculator.code.Interfaces;

namespace VisualSatisfactoryCalculator.code.Utility
{
	public class ResearchToRecipeMapping
	{
		private readonly Dictionary<string, string[]> _mappings;

		public ResearchToRecipeMapping()
		{
			_mappings = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(File.ReadAllText(".\\data\\mappings.json"));
		}

		public Dictionary<string, IRecipe> GetRecipesForResearch(string research, Dictionary<string, IRecipe> recipes)
		{
			if (_mappings.ContainsKey(research))
			{
				Dictionary<string, IRecipe> unlockedRecipes = new Dictionary<string, IRecipe>();
				foreach (string str in _mappings[research])
				{
					unlockedRecipes.Add(recipes[str].UID, recipes[str]);
				}
				return unlockedRecipes;
			}
			if (research.Contains("ResourceSink") || research.Contains("HardDrive") || research.Contains("Inventory"))
			{
				return new Dictionary<string, IRecipe>();
			}
			/*if (research.StartsWith("Schematic_Alternate"))
			{
				string temp = research.Remove(0, "Schematic".Length);
				temp = temp.Insert(0, "Recipe");
				temp = SeperateNumbers(temp);
				return new Dictionary<string, IRecipe>() { { recipes[temp].UID, recipes[temp] } };
			}*/
			Debug.Fail("Research " + research + " does not have a mapping!");
			return new Dictionary<string, IRecipe>();
		}

		public Dictionary<string, IRecipe> GetAllRelevantRecipes(Dictionary<string, IRecipe> recipes)
		{
			Dictionary<string, IRecipe> allRecipes = new Dictionary<string, IRecipe>();
			foreach (string key in _mappings.Keys)
			{
				foreach (string str in _mappings[key])
				{
					allRecipes.Add(str, recipes[str]);
				}
			}
			return allRecipes;
		}
		/*
		public static readonly char[] numbers = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
		public static readonly char[] letters = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
		
		private static string SeperateNumbers(string str)
		{
			string lowerStr = str.ToLower();
			for (int i = 0; i < lowerStr.Length - 1; i++)
			{
				if ((numbers.Contains(lowerStr[i]) && letters.Contains(lowerStr[i + 1])) || (numbers.Contains(lowerStr[i + 1]) && letters.Contains(lowerStr[i])))
				{
					str = str.Insert(i + 1, "_");
					lowerStr = str.ToLower();
				}
			}
			return str;
		}*/
	}
}
