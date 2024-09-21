using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using VisualSatisfactoryCalculator.code.Utility;
using VisualSatisfactoryCalculator.satisfactory.DataStorage;
using VisualSatisfactoryCalculator.satisfactory.Interfaces;
using VisualSatisfactoryCalculator.satisfactory.JSONClasses;
using VisualSatisfactoryCalculator.satisfactory.model.production;

namespace VisualSatisfactoryCalculator.satisfactory.Utility
{
	public class JsonEncodings
	{
		private readonly Dictionary<string, JSONItem> _items;
		private readonly Dictionary<string, HashSet<string>> _itemsByNativeClass;
		private readonly HashSet<JSONItem> _resourceItems;
		private readonly Dictionary<string, JSONBuilding> _buildings;
		private readonly Dictionary<string, JSONRecipe> _recipes;
		private readonly Dictionary<string, JSONResourceExtractor> _resourceExtractors;
		private readonly Dictionary<string, JSONGenerator> _generators;

		public JsonEncodings() : base()
		{
			_items = new Dictionary<string, JSONItem>();
			_itemsByNativeClass = new Dictionary<string, HashSet<string>>();
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
			if (!_itemsByNativeClass.ContainsKey(item.NativeClass))
			{
				_itemsByNativeClass.Add(item.NativeClass, new HashSet<string>());
			}
			_itemsByNativeClass[item.NativeClass].Add(item.id);
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
			_recipes.Add(recipe.id, recipe);
		}

		public void Add(JSONResourceExtractor extractor)
		{
			_resourceExtractors.Add(extractor.ID, extractor);
		}

		public void Add(JSONGenerator generator)
		{
			_generators.Add(generator.id, generator);
		}

		public Encodings Process()
		{
			Dictionary<string, Item> processedItems = new Dictionary<string, Item>();
			foreach (JSONItem item in _items.Values)
			{
				processedItems.Add(item.id, item.Process());
			}
			processedItems.Add(Constants.MW_ITEM.id, Constants.MW_ITEM);
			ImmutableDictionary<string, Item> finalItems = processedItems.ToImmutableDictionary();
			HashSet<Item> processedResourceItems = new HashSet<Item>();
			foreach (JSONItem item in _resourceItems)
			{
				processedResourceItems.Add(finalItems[item.id]);
			}
			Dictionary<string, Building> processedBuildings = new Dictionary<string, Building>();
			foreach (JSONBuilding building in _buildings.Values)
			{
				processedBuildings.Add(building.ID, building.Process());
			}
			foreach (JSONResourceExtractor extractor in _resourceExtractors.Values)
			{
				processedBuildings.Add(extractor.ID, extractor.Process());
			}
			foreach (JSONGenerator generator in _generators.Values)
			{
				processedBuildings.Add(generator.id, generator.Process());
			}
			ImmutableDictionary<string, Building> finalBuildings = processedBuildings.ToImmutableDictionary();
			Dictionary<string, Recipe> processedRecipes = new Dictionary<string, Recipe>();
			foreach (JSONRecipe recipe in _recipes.Values)
			{
				if (recipe.GetBuilding(finalBuildings) != default)
				{
					processedRecipes.Add(recipe.id, recipe.Process(finalItems, finalBuildings));
				}
			}
			foreach (JSONResourceExtractor extractor in _resourceExtractors.Values)
			{
				foreach (Recipe recipe in extractor.ProcessRecipes(finalItems, _resourceItems, finalBuildings))
				{
					processedRecipes.Add(recipe.id, recipe);
				}
			}
			foreach (JSONGenerator generator in _generators.Values)
			{
				foreach (Recipe recipe in generator.ProcessRecipes(_items, _itemsByNativeClass, finalItems, finalBuildings))
				{
					processedRecipes.Add(recipe.id, recipe);
				}
			}
			return new Encodings(finalItems, processedResourceItems.ToImmutableHashSet(), processedRecipes.ToImmutableDictionary(), finalBuildings);
		}
	}
}
