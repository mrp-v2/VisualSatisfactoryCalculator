using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SatisfactorySaveEditor.Model;
using SatisfactorySaveEditor.Util;
using SatisfactorySaveEditor.ViewModel.Property;
using SatisfactorySaveParser;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using VisualSatisfactoryCalculator.code.Interfaces;
using VisualSatisfactoryCalculator.code.JSONClasses;

namespace VisualSatisfactoryCalculator.code.Utility
{
	public class SaveFileInteractor
	{
		private readonly string jsonFile;
		private readonly Dictionary<string, JArray> docGroups;
		private readonly JsonSerializer jsonSerializer;

		public List<IRecipe> GetUnlockedRecipesFromSave(string saveFile, List<IEncoder> encoders)
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
			List<IRecipe> allRecipes = new List<IRecipe>();
			foreach (IEncoder encoder in encoders)
			{
				if (encoder is IRecipe) allRecipes.Add(encoder as IRecipe);
			}
			List<IRecipe> unlockedRecipes = new List<IRecipe>();
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
							unlockedRecipes.AddRange(ResearchToRecipeMapping.GetRecipesForResearch(id, allRecipes));
						}
					}
				}
			}
			return unlockedRecipes;
		}

		public SaveFileInteractor()
		{
			jsonFile = File.ReadAllText(".\\data\\Docs.json");
			jsonSerializer = new JsonSerializer()
			{
				Culture = System.Globalization.CultureInfo.GetCultureInfo(1033),
			};
			docGroups = new Dictionary<string, JArray>();
			//
			List<JToken> groups = JArray.Parse(jsonFile).Children().ToList();
			foreach (JToken token in groups)
			{
				string className = token.Value<string>("NativeClass");
				docGroups.Add(className, token.Value<JArray>("Classes"));
			}
		}

		public List<IEncoder> GetEncoders()
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
			List<IEncoder> totalResults = new List<IEncoder>();
			foreach (JToken token in results)
			{
				totalResults.Add(token.ToObject<JSONItem>(jsonSerializer));
			}
			//JSONBuildings
			results.Clear();
			results.AddRange(GetSection("FGBuildableManufacturer"));
			foreach (JToken token in results)
			{
				totalResults.Add(token.ToObject<JSONBuilding>(jsonSerializer));
			}
			//Constants
			totalResults.AddRange(Constants.AllConstantEncoders);
			//JSONRecipes
			results.Clear();
			results.AddRange(GetSection("FGRecipe"));
			foreach (JToken token in results)
			{
				totalResults.Add(token.ToObject<JSONRecipe>(jsonSerializer));
			}
			//JSONGenerators -- must go near end, uses encodings to get energy value of fuel
			results.Clear();
			results.AddRange(GetSection("FGBuildableGeneratorFuel"));
			results.AddRange(GetSection("FGBuildableGeneratorNuclear"));
			List<JSONGenerator> generatorResults = new List<JSONGenerator>();
			foreach (JToken token in results)
			{
				generatorResults.Add(token.ToObject<JSONGenerator>(jsonSerializer));
			}
			foreach (JSONGenerator generator in generatorResults)
			{
				if (generator.GetDisplayName().Equals("Biomass Burner")) continue;
				totalResults.AddRange(generator.GetRecipes(totalResults));
				totalResults.Add(generator);
			}
			//finished
			Constants.LastResortEncoderList.AddRange(totalResults);
			return totalResults;
		}

		private List<JToken> GetSection(string nativeClass)
		{
			nativeClass = "Class'/Script/FactoryGame." + nativeClass + "'";
			return docGroups[nativeClass].Children().ToList();
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
