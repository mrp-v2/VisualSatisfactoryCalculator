using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using SatisfactorySaveEditor.Model;
using SatisfactorySaveEditor.Util;
using SatisfactorySaveEditor.ViewModel.Property;

using SatisfactorySaveParser;

using VisualSatisfactoryCalculator.code.Extensions;
using VisualSatisfactoryCalculator.code.Interfaces;
using VisualSatisfactoryCalculator.code.JSONClasses;

namespace VisualSatisfactoryCalculator.code.Utility
{
	public class SaveFileInteractor
	{
		private readonly string _jsonFile;
		private readonly Dictionary<string, JArray> _docGroups;
		private readonly JsonSerializer _jsonSerializer;
		private readonly ResearchToRecipeMapping _rTRMapping;

		public Dictionary<string, IRecipe> GetUnlockedRecipesFromSave(string saveFile, Dictionary<string, IEncoder> encoders)
		{
			SatisfactorySave save = new SatisfactorySave(saveFile);
			SaveObjectModel root = new SaveRootModel(save.Header);
			EditorTreeNode tree = new EditorTreeNode("Root");
			foreach (SaveObject obj in save.Entries)
			{
				tree.AddChild(obj.TypePath.TrimStart('/').Split('/'), obj);
			}
			BuildNode(root.Items, tree);
			root.IsExpanded = true;
			foreach (SaveObjectModel obj in root.Items)
			{
				obj.IsExpanded = true;
			}
			SaveObjectModel schematics = root.FindChild("Persistent_Level:PersistentLevel.schematicManager", false);
			Dictionary<string, IRecipe> allRecipes = new Dictionary<string, IRecipe>();
			foreach (IEncoder encoder in encoders.Values)
			{
				if (encoder is IRecipe)
				{
					allRecipes.Add((encoder as IRecipe).UID, encoder as IRecipe);
				}
			}
			Dictionary<string, IRecipe> unlockedRecipes = new Dictionary<string, IRecipe>();
#if DEBUG
			List<string> unlockedIDs = new List<string>();
#endif
			foreach (SerializedPropertyViewModel field in schematics.Fields)
			{
				if (field.PropertyName == "mPurchasedSchematics")
				{
					if (!(field is ArrayPropertyViewModel array))
					{
						return default;
					}
					foreach (SerializedPropertyViewModel element in array.Elements)
					{
						if (element is ObjectPropertyViewModel opvm)
						{
							string id = opvm.Str2.Substring(opvm.Str2.IndexOf(".") + 1);
#if DEBUG
							unlockedIDs.Add(id);
#endif
							unlockedRecipes.AddRange(_rTRMapping.GetRecipesForResearch(id, allRecipes));
						}
					}
				}
			}
#if DEBUG
			unlockedIDs.Sort();
			string fileToMake = $".\\data\\{saveFile.Substring(saveFile.LastIndexOf("\\") + 1)}-unlocks.txt";
			if (File.Exists(fileToMake))
			{
				File.Delete(fileToMake);
			}
			StreamWriter writer = File.CreateText(fileToMake);
			for (int i = 0; i < unlockedIDs.Count; i++)
			{
				if (i < unlockedIDs.Count - 1)
				{
					writer.WriteLine(unlockedIDs[i]);
				}
				else
				{
					writer.Write(unlockedIDs[i]);
				}
			}
			writer.Close();
			writer.Dispose();
#endif
			return unlockedRecipes;
		}

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
			return _rTRMapping.GetAllRelevantRecipes(allRecipes);
		}

		public SaveFileInteractor()
		{
			_jsonFile = File.ReadAllText(".\\data\\Docs.json");
			_jsonSerializer = new JsonSerializer()
			{
				Culture = System.Globalization.CultureInfo.GetCultureInfo(1033),
			};
			_docGroups = new Dictionary<string, JArray>();
			_rTRMapping = new ResearchToRecipeMapping();
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
			//JSONItems
			List<JToken> results = new List<JToken>();
			results.AddRange(GetSection("FGItemDescriptor"));
			results.AddRange(GetSection("FGBuildingDescriptor"));
			results.AddRange(GetSection("FGItemDescriptorBiomass"));
			results.AddRange(GetSection("FGEquipmentDescriptor"));
			results.AddRange(GetSection("FGResourceDescriptor"));
			results.AddRange(GetSection("FGConsumableDescriptor"));
			results.AddRange(GetSection("FGItemDescriptorNuclearFuel"));
			Dictionary<string, IEncoder> totalResults = new Dictionary<string, IEncoder>();
			foreach (JToken token in results)
			{
				JSONItem item = token.ToObject<JSONItem>(_jsonSerializer);
				totalResults.Add(item.UID, item);
			}
			//JSONBuildings
			results.Clear();
			results.AddRange(GetSection("FGBuildableManufacturer"));
			foreach (JToken token in results)
			{
				JSONBuilding building = token.ToObject<JSONBuilding>(_jsonSerializer);
				totalResults.Add(building.UID, building);
			}
			//Constants
			totalResults.AddRange(Constants.AllConstantEncoders);
			//JSONRecipes
			results.Clear();
			results.AddRange(GetSection("FGRecipe"));
			foreach (JToken token in results)
			{
				JSONRecipe recipe = token.ToObject<JSONRecipe>(_jsonSerializer);
				totalResults.Add(recipe.UID, recipe);
			}
			//JSONGenerators -- must go near end, uses encodings to get energy value of fuel
			results.Clear();
			results.AddRange(GetSection("FGBuildableGeneratorFuel"));
			results.AddRange(GetSection("FGBuildableGeneratorNuclear"));
			List<JSONGenerator> generatorResults = new List<JSONGenerator>();
			foreach (JToken token in results)
			{
				generatorResults.Add(token.ToObject<JSONGenerator>(_jsonSerializer));
			}
			foreach (JSONGenerator generator in generatorResults)
			{
				if (generator.DisplayName.Equals("Biomass Burner"))
				{
					continue;
				}
				//generators recipes
				totalResults.AddRange(generator.GetRecipes(totalResults));
				//generator itself
				totalResults.Add(generator.UID, generator);
			}
			//finished
			Constants.LastResortEncoderList.AddRange(totalResults);
			return totalResults;
		}

		private List<JToken> GetSection(string nativeClass)
		{
			nativeClass = "Class'/Script/FactoryGame." + nativeClass + "'";
			return _docGroups[nativeClass].Children().ToList();
		}

		private void BuildNode(ObservableCollection<SaveObjectModel> items, EditorTreeNode node)
		{
			foreach (KeyValuePair<string, EditorTreeNode> child in node.Children)
			{
				SaveObjectModel childItem = new SaveObjectModel(child.Value.Name);
				BuildNode(childItem.Items, child.Value);
				items.Add(childItem);
			}

			foreach (SaveObject entry in node.Content)
			{
				switch (entry)
				{
					case SaveEntity se:
						items.Add(new SaveEntityModel(se));
						break;
					case SaveComponent sc:
						items.Add(new SaveComponentModel(sc));
						break;
				}
			}
		}

	}
}
