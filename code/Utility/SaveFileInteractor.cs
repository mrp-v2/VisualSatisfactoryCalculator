using Newtonsoft.Json.Linq;
using SatisfactorySaveEditor.Model;
using SatisfactorySaveEditor.Util;
using SatisfactorySaveEditor.ViewModel.Property;
using SatisfactorySaveParser;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using VisualSatisfactoryCalculator.code.JSONClasses;

namespace VisualSatisfactoryCalculator.code.Utility
{
	public class SaveFileInteractor
	{
		public static List<JSONRecipe> GetRecipesFromSave(string saveFile)
		{
			//get information on unlocked recipes
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
			//get information on items
			string jsonFile = File.ReadAllText("C:\\Program Files\\Epic Games\\SatisfactoryEarlyAccess\\CommunityResources\\Docs\\Docs.json");
			List<JToken> results = new List<JToken>();
			results.AddRange(GetSection(jsonFile, "FactoryGame.FGPoleDescriptor", "FactoryGame.FGItemDescriptor"));
			results.AddRange(GetSection(jsonFile, "FactoryGame.FGItemDescriptor", "FactoryGame.FGRecipe"));
			results.AddRange(GetSection(jsonFile, "FactoryGame.FGBuildingDescriptor", "FactoryGame.FGBuildableWire"));
			results.AddRange(GetSection(jsonFile, "FactoryGame.FGItemDescriptorBiomass", "FactoryGame.FGChainsaw"));
			results.AddRange(GetSection(jsonFile, "FactoryGame.FGEquipmentDescriptor", "FactoryGame.FGBuildableGeneratorFuel"));
			results.AddRange(GetSection(jsonFile, "FactoryGame.FGResourceDescriptor", "FactoryGame.FGGolfCartDispenser"));
			results.AddRange(GetSection(jsonFile, "FactoryGame.FGVehicleDescriptor", "FactoryGame.FGConsumableDescriptor"));
			results.AddRange(GetSection(jsonFile, "FactoryGame.FGConsumableDescriptor", "FactoryGame.FGBuildablePipelineSupport"));
			results.AddRange(GetSection(jsonFile, "FactoryGame.FGItemDescriptorNuclearFuel", "FactoryGame.FGBuildableFoundation"));
			List<JSONItem> itemResults = new List<JSONItem>();
			foreach (JToken token in results)
			{
				itemResults.Add(token.ToObject<JSONItem>());
			}
			//get information on recipes
			results = GetSection(jsonFile, "FactoryGame.FGRecipe", "FactoryGame.FGBuildableConveyorBelt");
			List<JSONRecipe> recipeResults = new List<JSONRecipe>();
			foreach (JToken token in results)
			{
				recipeResults.Add(token.ToObject<JSONRecipe>());
			}
			foreach (JSONRecipe recipe in recipeResults)
			{
				recipe.Initialize(itemResults);
			}
			//use the information
			List<JSONRecipe> unlockedRecipes = new List<JSONRecipe>();
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
							unlockedRecipes.AddRange(ResearchToRecipeMapping.GetRecipesForResearch(id, recipeResults));
						}
					}
				}
			}
			return unlockedRecipes;
		}

		private static List<JToken> GetSection(string file, string sectionLabel, string nextSectionLabel)
		{
			int startIndex = file.IndexOf(sectionLabel);
			startIndex = file.LastIndexOf("{", startIndex);
			int endIndex = file.IndexOf(nextSectionLabel);
			endIndex = file.LastIndexOf("}", endIndex);
			JObject search = JObject.Parse(file.Substring(startIndex, endIndex - startIndex + 1));
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
