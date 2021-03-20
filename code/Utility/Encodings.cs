using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VisualSatisfactoryCalculator.code.Interfaces;
using VisualSatisfactoryCalculator.code.JSONClasses;

namespace VisualSatisfactoryCalculator.code.Utility
{
	public class Encodings : Dictionary<string, IEncoder>
	{
		public Encodings() : base()
		{
			ResourceItems = new List<JSONItem>();
			Recipes = new Dictionary<string, IRecipe>();
		}

		public List<JSONItem> ResourceItems { get; }
		public Dictionary<string, IRecipe> Recipes { get; }

		new public void Add(string key, IEncoder value)
		{
			base.Add(key, value);
			if (value is JSONItem)
			{
				JSONItem item = value as JSONItem;
				if (item.NativeClass.Equals("FGResourceDescriptor"))
				{
					ResourceItems.Add(item);
				}
			}
			else if (value is IRecipe)
			{
				IRecipe recipe = value as IRecipe;
				Recipes.Add(recipe.UID, recipe);
			}
		}

		public void AddRange<T>(Dictionary<string, T> encodings) where T : IEncoder
		{
			foreach (string UID in encodings.Keys)
			{
				Add(UID, encodings[UID]);
			}
		}
	}
}
