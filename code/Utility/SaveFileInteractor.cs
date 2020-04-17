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
		public static readonly string jsonFile = File.ReadAllText(".\\data\\Docs.json");

		public static List<IRecipe> GetUnlockedRecipesFromSave(string saveFile, List<IEncoder> encoders)
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

		public static List<IEncoder> GetEncoders()
		{
			//JSONItems
			List<JToken> results = new List<JToken>();
			results.AddRange(GetSection("FactoryGame.FGPoleDescriptor", "FactoryGame.FGItemDescriptor"));
			results.AddRange(GetSection("FactoryGame.FGItemDescriptor", "FactoryGame.FGRecipe"));
			results.AddRange(GetSection("FactoryGame.FGBuildingDescriptor", "FactoryGame.FGBuildableWire"));
			results.AddRange(GetSection("FactoryGame.FGItemDescriptorBiomass", "FactoryGame.FGChainsaw"));
			results.AddRange(GetSection("FactoryGame.FGEquipmentDescriptor", "FactoryGame.FGBuildableGeneratorFuel"));
			results.AddRange(GetSection("FactoryGame.FGResourceDescriptor", "FactoryGame.FGGolfCartDispenser"));
			results.AddRange(GetSection("FactoryGame.FGVehicleDescriptor", "FactoryGame.FGConsumableDescriptor"));
			results.AddRange(GetSection("FactoryGame.FGConsumableDescriptor", "FactoryGame.FGBuildablePipelineSupport"));
			results.AddRange(GetSection("FactoryGame.FGItemDescriptorNuclearFuel", "FactoryGame.FGBuildableFoundation"));
			List<IEncoder> totalResults = new List<IEncoder>();
			foreach (JToken token in results)
			{
				totalResults.Add(token.ToObject<JSONItem>());
			}
			//JSONBuildings
			results.Clear();
			results.AddRange(GetSection("FactoryGame.FGBuildableManufacturer", "FactoryGame.FGPortableMinerDispenser"));
			foreach (JToken token in results)
			{
				totalResults.Add(token.ToObject<JSONBuilding>());
			}
			//Constants
			totalResults.AddRange(Constants.AllConstantEncoders);
			//JSONRecipes
			results.Clear();
			results.AddRange(GetSection("FactoryGame.FGRecipe", "FactoryGame.FGBuildableConveyorBelt"));
			foreach (JToken token in results)
			{
				totalResults.Add(token.ToObject<JSONRecipe>());
			}
			//JSONGenerators -- must go near end, uses encodings to get energy value of fuel
			results.Clear();
			results.AddRange(GetSection("FactoryGame.FGBuildableGeneratorFuel", "FactoryGame.FGBuildableTradingPost"));
			results.AddRange(GetSection("FactoryGame.FGBuildableGeneratorNuclear", "FactoryGame.FGItemDescriptorNuclearFuel"));
			List<JSONGenerator> generatorResults = new List<JSONGenerator>();
			foreach (JToken token in results)
			{
				generatorResults.Add(token.ToObject<JSONGenerator>());
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

		private static List<JToken> GetSection(string sectionLabel, string nextSectionLabel)
		{
			int startIndex = jsonFile.IndexOf(sectionLabel);
			startIndex = jsonFile.LastIndexOf("{", startIndex);
			int endIndex = jsonFile.IndexOf(nextSectionLabel);
			endIndex = jsonFile.LastIndexOf("}", endIndex);
			JObject search = JObject.Parse(jsonFile.Substring(startIndex, endIndex - startIndex + 1));
			return search["Classes"].Children().ToList();
		}

		private static void BuildNode(ObservableCollection<SaveObjectModel> items, EditorTreeNode node)
		{
			foreach (var child in node.Children)
			{
				var childItem = new SaveObjectModel(child.Value.Name);
				BuildNode(childItem.Items, child.Value);
				items.Add(childItem);
			}

			foreach (var entry in node.Content)
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
