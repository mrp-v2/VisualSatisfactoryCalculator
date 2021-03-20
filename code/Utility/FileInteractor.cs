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

		public Dictionary<string, IRecipe> GetAllRecipes(Dictionary<string, IEncoder> encoders)
		{
			Dictionary<string, IRecipe> allRecipes = new Dictionary<string, IRecipe>();
			foreach (IEncoder encoder in encoders.Values)
			{
				if (encoder is IRecipe)
				{
					allRecipes.Add((encoder as IRecipe).UID, encoder as IRecipe);
				}
			}
			return allRecipes;
		}

		public FileInteractor()
		{
			_jsonFile = File.ReadAllText(".\\data\\Docs.json");
			_jsonSerializer = new JsonSerializer()
			{
				Culture = System.Globalization.CultureInfo.GetCultureInfo(1033),
			};
			_docGroups = new Dictionary<string, JArray>();
			//
			List<JToken> groups = JArray.Parse(_jsonFile).Children().ToList();
			foreach (JToken token in groups)
			{
				string className = token.Value<string>("NativeClass");
				_docGroups.Add(className, token.Value<JArray>("Classes"));
			}
		}

		public Dictionary<string, IEncoder> GetEncoders()
		{
			Dictionary<string, IEncoder> totalResults = new Dictionary<string, IEncoder>();
			//JSONItems
			List<JSONItem> items = new List<JSONItem>();
			GetSection("FGItemDescriptor", items);
			GetSection("FGBuildingDescriptor", items);
			GetSection("FGItemDescriptorBiomass", items);
			GetSection("FGEquipmentDescriptor", items);
			GetSection("FGResourceDescriptor", items);
			GetSection("FGConsumableDescriptor", items);
			GetSection("FGItemDescriptorNuclearFuel", items);
			foreach (JSONItem item in items)
			{
				totalResults.Add(item.UID, item);
			}
			//JSONBuildings
			foreach (JSONBuilding building in GetSection<JSONBuilding>("FGBuildableManufacturer"))
			{
				totalResults.Add(building.UID, building);
			}
			//Constants
			totalResults.AddRangeIfNew(Constants.AllConstantEncoders);
			//JSONRecipes
			foreach (JSONRecipe recipe in GetSection<JSONRecipe>("FGRecipe"))
			{
				totalResults.Add(recipe.UID, recipe);
			}
			//JSONGenerators -- must go near end, uses encodings to get energy value of fuel
			List<JSONGenerator> generators = new List<JSONGenerator>();
			GetSection("FGBuildableGeneratorFuel", generators);
			GetSection("FGBuildableGeneratorNuclear", generators);
			foreach (JSONGenerator generator in generators)
			{
				if (generator.DisplayName.Equals("Biomass Burner"))
				{
					continue;
				}
				//generators recipes
				totalResults.AddRangeIfNew(generator.GetRecipes(totalResults));
				//generator itself
				totalResults.Add(generator.UID, generator);
			}
			//finished
			Constants.LastResortEncoderList.AddRangeIfNew(totalResults);
			return totalResults;
		}

		public static string ActiveNativeClass { get; private set; }

		private List<T> GetSection<T>(string nativeClass, List<T> output)
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
			return GetSection<T>(nativeClass, new List<T>());
		}
	}
}
