using System;
using System.Collections.Generic;

using VisualSatisfactoryCalculator.code.Utility;
using VisualSatisfactoryCalculator.satisfactory.DataStorage;
using VisualSatisfactoryCalculator.satisfactory.Interfaces;
using VisualSatisfactoryCalculator.satisfactory.JSONClasses;

namespace VisualSatisfactoryCalculator.satisfactory.Utility
{
	public class JsonEncodings
	{
		private readonly Dictionary<string, JSONItem> _items;
		private readonly HashSet<JSONItem> _resourceItems;
		private readonly Dictionary<string, JSONBuilding> _buildings;
		private readonly Dictionary<string, JSONRecipe> _recipes;
		private readonly Dictionary<string, JSONResourceExtractor> _resourceExtractors;
		private readonly Dictionary<string, JSONGenerator> _generators;

		public JsonEncodings() : base()
		{
			_items = new Dictionary<string, JSONItem>();
			_resourceItems = new HashSet<JSONItem>();
			_buildings = new Dictionary<string, JSONBuilding>();
			_recipes = new Dictionary<string, JSONRecipe>();
			_resourceExtractors = new Dictionary<string, JSONResourceExtractor>();
			_generators = new Dictionary<string, JSONGenerator>();
		}

		public IEnumerable<JSONItem> ResourceItems
		{
			get
			{
				return _resourceItems;
			}
		}
		public IEnumerable<KeyValuePair<string, JSONRecipe>> Recipes
		{
			get
			{
				return _recipes;
			}
		}

		public void Add(JSONItem item)
		{
			_items.Add(item.id, item);
			if (item.NativeClass.Equals("FGResourceDescriptor"))
			{
				_resourceItems.Add(item);
			}
		}

		public void Add(JSONBuilding building)
		{
			_buildings.Add(building.ID, building);
		}

		public void Add(JSONRecipe recipe)
		{
			_recipes.Add(recipe.ID, recipe);
		}

		public void Add(JSONResourceExtractor extractor)
		{
			_resourceExtractors.Add(extractor.ID, extractor);
		}

		public void Add(JSONGenerator generator)
		{
			_generators.Add(generator.ID, generator);
		}

		public Encodings Process()
		{
			throw new NotImplementedException();
		}
	}
}
