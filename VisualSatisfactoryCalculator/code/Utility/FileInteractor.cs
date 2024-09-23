using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using VisualSatisfactoryCalculator.satisfactory.DataStorage;
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
			GetSection<JSONItem>("AmmoTypeProjectile", jsonEncodings.Add);
			GetSection<JSONItem>("AmmoTypeSpreadshot", jsonEncodings.Add);
			GetSection<JSONItem>("AmmoTypeInstantHit", jsonEncodings.Add);
			GetSection<JSONItem>("BuildingDescriptor", jsonEncodings.Add);
			GetSection<JSONItem>("ConsumableDescriptor", jsonEncodings.Add);
			GetSection<JSONItem>("EquipmentDescriptor", jsonEncodings.Add);
			GetSection<JSONItem>("ItemDescriptor", jsonEncodings.Add);
			GetSection<JSONItem>("ItemDescriptorBiomass", jsonEncodings.Add);
			GetSection<JSONItem>("ItemDescriptorPowerBoosterFuel", jsonEncodings.Add);
			GetSection<JSONItem>("ItemDescriptorNuclearFuel", jsonEncodings.Add);
			GetSection<JSONItem>("PowerShardDescriptor", jsonEncodings.Add);
			GetSection<JSONItem>("ResourceDescriptor", jsonEncodings.Add);
			//JSONBuildings
			GetSection<JSONBuilding>("BuildableManufacturer", jsonEncodings.Add);
			GetSection<JSONBuilding>("BuildableManufacturerVariablePower", jsonEncodings.Add);
			//JSONRecipes
			foreach (JSONRecipe recipe in GetSection<JSONRecipe>("Recipe"))
			{
				jsonEncodings.Add(recipe);
			}
			//JSONResourceExtractors
			GetSection<JSONResourceExtractor>("BuildableFrackingExtractor", jsonEncodings.Add);
			GetSection<JSONResourceExtractor>("BuildableResourceExtractor", jsonEncodings.Add);
			GetSection<JSONResourceExtractor.JSONWaterPump>("BuildableWaterPump", jsonEncodings.Add);
			//JSONGenerators
			List<JSONGenerator> generators = new List<JSONGenerator>();
			GetSection<JSONGenerator>("BuildableGeneratorFuel", jsonEncodings.Add);
			GetSection<JSONGenerator>("BuildableGeneratorNuclear", jsonEncodings.Add);
			//finished
			return jsonEncodings;
		}

		public static string ActiveNativeClass { get; private set; }

		private void GetSection<V>(string nativeClass, Action<V> action)
		{
			ActiveNativeClass = nativeClass;
			nativeClass = "/Script/CoreUObject.Class'/Script/FactoryGame.FG" + nativeClass + "'";
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
			nativeClass = "/Script/CoreUObject.Class'/Script/FactoryGame.FG" + nativeClass + "'";
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
