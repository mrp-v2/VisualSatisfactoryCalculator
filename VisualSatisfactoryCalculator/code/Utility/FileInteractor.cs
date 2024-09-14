using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using VisualSatisfactoryCalculator.satisfactory.Extensions;
using VisualSatisfactoryCalculator.satisfactory.JSONClasses;

namespace VisualSatisfactoryCalculator.satisfactory.Utility
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

		public JsonEncodings GetEncoders()
		{
			JsonEncodings jsonEncodings = new JsonEncodings();
			//JSONItems
			GetSection<JSONItem>("FGItemDescriptor", jsonEncodings.Add);
			GetSection<JSONItem>("FGBuildingDescriptor", jsonEncodings.Add);
			GetSection<JSONItem>("FGItemDescriptorBiomass", jsonEncodings.Add);
			GetSection<JSONItem>("FGEquipmentDescriptor", jsonEncodings.Add);
			GetSection<JSONItem>("FGResourceDescriptor", jsonEncodings.Add);
			GetSection<JSONItem>("FGConsumableDescriptor", jsonEncodings.Add);
			GetSection<JSONItem>("FGItemDescriptorNuclearFuel", jsonEncodings.Add);
			GetSection<JSONItem>("FGAmmoTypeProjectile", jsonEncodings.Add);
			GetSection<JSONItem>("FGAmmoTypeSpreadshot", jsonEncodings.Add);
			GetSection<JSONItem>("FGAmmoTypeInstantHit", jsonEncodings.Add);
			jsonEncodings.Add(Constants.MW_ITEM);
			//JSONBuildings
			GetSection<JSONBuilding>("FGBuildableManufacturer", jsonEncodings.Add);
			GetSection<JSONBuilding>("FGBuildableManufacturerVariablePower", jsonEncodings.Add);
			//JSONRecipes -- must go after all buildings, uses buildings to decide what produces this recipe
			foreach (JSONRecipe recipe in GetSection<JSONRecipe>("FGRecipe"))
			{
				if (recipe.MachineUID != default)
				{
					jsonEncodings.Add(recipe);
				}
			}
			//JSONResourceExtractors
			GetSection<JSONResourceExtractor>("FGBuildableResourceExtractor", jsonEncodings.Add);
			GetSection<JSONResourceExtractor.JSONWaterPump>("FGBuildableWaterPump", jsonEncodings.Add);
			GetSection<JSONResourceExtractor>("FGBuildableFrackingExtractor", jsonEncodings.Add);
			//JSONGenerators
			List<JSONGenerator> generators = new List<JSONGenerator>();
			GetSection<JSONGenerator>("FGBuildableGeneratorFuel", jsonEncodings.Add);
			GetSection<JSONGenerator>("FGBuildableGeneratorNuclear", jsonEncodings.Add);
			//Generated Recipes -- must go after all items, uses item properties
			//JSONResourceExtractor Recipes
			// TODO move to process encodings
			foreach (JSONResourceExtractor resourceExtractor in resourceExtractors)
			{
				jsonEncodings.AddRange(resourceExtractor.GetRecipes(jsonEncodings));
			}
			//JSONGenerator Recipes
			foreach (JSONGenerator generator in generators)
			{
				if (generator.DisplayName.Equals("Biomass Burner"))
				{
					continue;
				}
				jsonEncodings.AddRange(generator.GetRecipes(jsonEncodings));
			}
			//finished
			Constants.FALLBACK_ENCODINGS.AddRange(jsonEncodings);
			return jsonEncodings;
		}

		public static string ActiveNativeClass { get; private set; }

		private void GetSection<V>(string nativeClass, Action<V> action)
		{
			ActiveNativeClass = nativeClass;
			nativeClass = "Class'/Script/FactoryGame." + nativeClass + "'";
			foreach (JToken token in _docGroups[nativeClass].Children().ToList())
			{
				V result = token.ToObject<V>(_jsonSerializer);
				action(result);
			}
			ActiveNativeClass = string.Empty;
		}

		private IEnumerable<T> GetSection<T>(string nativeClass)
		{
			ActiveNativeClass = nativeClass;
			nativeClass = "Class'/Script/FactoryGame." + nativeClass + "'";
			List<T> output = new List<T>();
			foreach (JToken token in _docGroups[nativeClass].Children().ToList())
			{
				output.Add(token.ToObject<T>(_jsonSerializer));
			}
			ActiveNativeClass = string.Empty;
			return output;
		}
	}
}
