using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using VisualSatisfactoryCalculator.code.Extensions;
using VisualSatisfactoryCalculator.code.Interfaces;
using VisualSatisfactoryCalculator.code.JSONClasses;

namespace VisualSatisfactoryCalculator.code.Utility
{
	public class FileInteractor
	{
		private readonly string _jsonFile;
		private readonly Dictionary<string, JArray> _docGroups;
		private readonly JsonSerializer _jsonSerializer;

		public FileInteractor()
		{
			_jsonFile = File.ReadAllText(".\\data\\Docs.json");
			_jsonSerializer = new JsonSerializer()
			{
				Culture = System.Globalization.CultureInfo.GetCultureInfo(1033),
			};
			_docGroups = new Dictionary<string, JArray>();
			List<JToken> groups = JArray.Parse(_jsonFile).Children().ToList();
			foreach (JToken token in groups)
			{
				string className = token.Value<string>("NativeClass");
				_docGroups.Add(className, token.Value<JArray>("Classes"));
			}
		}

		public Encodings GetEncoders()
		{
			Encodings totalResults = new Encodings();
			CurrentEncodings = totalResults;
			//JSONItems
			List<JSONItem> items = new List<JSONItem>();
			GetSection<JSONItem, JSONItem>("FGItemDescriptor", items);
			GetSection<JSONItem, JSONItem>("FGBuildingDescriptor", items);
			GetSection<JSONItem, JSONItem>("FGItemDescriptorBiomass", items);
			GetSection<JSONItem, JSONItem>("FGEquipmentDescriptor", items);
			GetSection<JSONItem, JSONItem>("FGResourceDescriptor", items);
			GetSection<JSONItem, JSONItem>("FGConsumableDescriptor", items);
			GetSection<JSONItem, JSONItem>("FGItemDescriptorNuclearFuel", items);
			GetSection<JSONItem, JSONItem>("FGAmmoTypeProjectile", items);
			GetSection<JSONItem, JSONItem>("FGAmmoTypeSpreadshot", items);
			GetSection<JSONItem, JSONItem>("FGAmmoTypeInstantHit", items);
			foreach (JSONItem item in items)
			{
				totalResults.Add(item.ID, item);
			}
			//JSONBuildings
			List<JSONBuilding> buildings = new List<JSONBuilding>();
			GetSection<JSONBuilding, JSONBuilding>("FGBuildableManufacturer", buildings);
			GetSection<JSONBuilding, JSONBuilding>("FGBuildableManufacturerVariablePower", buildings);
			foreach (JSONBuilding building in buildings)
			{
				totalResults.Add(building.ID, building);
			}
			//JSONRecipes -- must go after all buildings, uses buildings to decide what produces this recipe
			foreach (JSONRecipe recipe in GetSection<JSONRecipe>("FGRecipe"))
			{
				if (recipe.MachineUID != default)
				{
					totalResults.Add(recipe.ID, recipe);
				}
			}
			//JSONResourceExtractors
			List<JSONResourceExtractor> resourceExtractors = new List<JSONResourceExtractor>();
			GetSection<JSONResourceExtractor, JSONResourceExtractor>("FGBuildableResourceExtractor", resourceExtractors);
			GetSection<JSONResourceExtractor.JSONWaterPump, JSONResourceExtractor>("FGBuildableWaterPump", resourceExtractors);
			GetSection<JSONResourceExtractor, JSONResourceExtractor>("FGBuildableFrackingExtractor", resourceExtractors);
			foreach (JSONResourceExtractor resourceExtractor in resourceExtractors)
			{
				totalResults.Add(resourceExtractor.ID, resourceExtractor);
			}
			//JSONGenerators
			List<JSONGenerator> generators = new List<JSONGenerator>();
			GetSection<JSONGenerator, JSONGenerator>("FGBuildableGeneratorFuel", generators);
			GetSection<JSONGenerator, JSONGenerator>("FGBuildableGeneratorNuclear", generators);
			foreach (JSONGenerator generator in generators)
			{
				if (generator.DisplayName.Equals("Biomass Burner"))
				{
					continue;
				}
				totalResults.Add(generator.ID, generator);
			}
			//Constants
			totalResults.AddRange(Constants.AllConstantEncoders);
			//Generated Recipes -- must go after all items, uses item properties
			//JSONResourceExtractor Recipes
			foreach (JSONResourceExtractor resourceExtractor in resourceExtractors)
			{
				totalResults.AddRange(resourceExtractor.GetRecipes(totalResults));
			}
			//JSONGenerator Recipes
			foreach (JSONGenerator generator in generators)
			{
				if (generator.DisplayName.Equals("Biomass Burner"))
				{
					continue;
				}
				totalResults.AddRange(generator.GetRecipes(totalResults));
			}
			//finished
			Constants.LastResortEncoderList.AddRange(totalResults);
			return totalResults;
		}

		public static Encodings CurrentEncodings { get; private set; }

		public static string ActiveNativeClass { get; private set; }

		private List<V> GetSection<T, V>(string nativeClass, List<V> output) where T : V
		{
			ActiveNativeClass = nativeClass;
			nativeClass = "Class'/Script/FactoryGame." + nativeClass + "'";
			foreach (JToken token in _docGroups[nativeClass].Children().ToList())
			{
				output.Add(token.ToObject<T>(_jsonSerializer));
			}
			ActiveNativeClass = string.Empty;
			return output;
		}

		private List<T> GetSection<T>(string nativeClass)
		{
			return GetSection<T, T>(nativeClass, new List<T>());
		}
	}
}
