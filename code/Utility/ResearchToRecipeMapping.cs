using System;
using System.Collections.Generic;
using System.Linq;
using VisualSatisfactoryCalculator.code.Extensions;
using VisualSatisfactoryCalculator.code.Interfaces;

namespace VisualSatisfactoryCalculator.code.Utility
{
	public class ResearchToRecipeMapping
	{
		public static readonly Dictionary<string, string[]> mapping = new Dictionary<string, string[]>()
		{
			/*
			alternate template
			{"Schematic_Alternate__C", new string[] { "Recipe_Alternate__C" } },
			schematic template
			{"Schematic__C", new string[] { } },
			research template
			{"Research__C", new string[] { } },
			alternate recipe template
			"Recipe_Alternate__C"
			*/
			//starting recipes
			{"Schematic_StartingRecipes_C", new string[] { "Recipe_IngotIron_C", "Recipe_IronPlate_C", "Recipe_IronRod_C" } },
			//onboarding recipes
			{"Schematic_Tutorial1_C", new string[] {} },
			{"Schematic_Tutorial1_5_C", new string[] { "Recipe_IngotCopper_C", "Recipe_Wire_C", "Recipe_Cable_C" } },
			{"Schematic_Tutorial2_C", new string[] { "Recipe_Concrete_C", "Recipe_Screw_C", "Recipe_IronPlateReinforced_C" } },
			{"Schematic_Tutorial3_C", new string[] {} },
			{"Schematic_Tutorial4_C", new string[] {} },
			{"Schematic_Tutorial5_C", new string[] { "Recipe_Biomass_Leaves_C", "Recipe_Biomass_Wood_C" } },
			//normal hub unlocks
			{"Schematic_1-1_C", new string[] { } },
			{"Schematic_1-2_C", new string[] { } },
			{"Schematic_1-3_C", new string[] { "Recipe_Beacon_C" } },
			{"Schematic_2-1_C", new string[] { "Recipe_CopperSheet_C", "Recipe_Rotor_C", "Recipe_ModularFrame_C", "Recipe_SpaceElevatorPart_1_C" } },
			{"Schematic_2-2_C", new string[] { "Recipe_Biofuel_C" } },
			{"Schematic_2-3_C", new string[] { } },
			{"Schematic_2-5_C", new string[] { } },
			{"Schematic_3-1_C", new string[] { "Build_GeneratorCoal_CDesc_Coal_C", "Build_GeneratorCoal_CDesc_CompactedCoal_C", "Build_GeneratorCoal_CDesc_PetroleumCoke_C" } },
			{"Schematic_3-2_C", new string[] { } },
			{"Schematic_3-3_C", new string[] { "Recipe_IngotSteel_C", "Recipe_SteelBeam_C", "Recipe_SteelPipe_C", "Recipe_SpaceElevatorPart_2_C" } },
			{"Schematic_3-4_C", new string[] { } },
			{"Schematic_4-1_C", new string[] { "Recipe_EncasedIndustrialBeam_C", "Recipe_Stator_C", "Recipe_Motor_C", "Recipe_SpaceElevatorPart_3_C", "Recipe_ModularFrameHeavy_C" } },
			{"Schematic_4-2_C", new string[] { } },
			{"Schematic_4-4_C", new string[] { } },
			{"Schematic_5-1_C", new string[] { "Recipe_Plastic_C", "Recipe_Rubber_C", "Recipe_LiquidFuel_C", "Recipe_PetroleumCoke_C", "Recipe_CircuitBoard_C", "Recipe_ResidualFuel_C", "Recipe_ResidualPlastic_C", "Recipe_ResidualRubber_C" } },
			{"Schematic_5-1-1_C", new string[] {} },
			{"Schematic_5-2_C", new string[] { "Recipe_Computer_C", "Recipe_SpaceElevatorPart_4_C", "Recipe_SpaceElevatorPart_5_C" } },
			{"Schematic_5-3_C", new string[] { "Recipe_UnpackageBioFuel_C", "Recipe_UnpackageFuel_C", "Recipe_UnpackageOil_C", "Recipe_UnpackageOilResidue_C", "Recipe_UnpackageWater_C", "Recipe_FluidCanister_C", "Recipe_Fuel_C", "Recipe_LiquidBiofuel_C", "Recipe_PackagedBiofuel_C", "Recipe_PackagedCrudeOil_C", "Recipe_PackagedOilResidue_C", "Recipe_PackagedWater_C" } },
			{"Schematic_5-4_C", new string[] { "Recipe_FilterGasMask_C" } },
			{"Schematic_5-4-1_C", new string[] { } },
			{"Schematic_6-1_C", new string[] { "Build_GeneratorFuel_CDesc_LiquidFuel_C", "Build_GeneratorFuel_CDesc_LiquidTurboFuel_C", "Build_GeneratorFuel_CDesc_LiquidBiofuel_C" } },
			{"Schematic_6-2_C", new string[] { } },
			{"Schematic_6-3_C", new string[] { } },
			{"Schematic_6-4_C", new string[] { } },
			{"Schematic_7-1_C", new string[] { "Recipe_AluminumSheet_C", "Recipe_IngotAluminum_C", "Recipe_AluminumScrap_C", "Recipe_AluminaSolution_C" } },
			{"Schematic_7-2_C", new string[] { "Recipe_HeatSink_C", "Recipe_MotorTurbo_C", "Recipe_Battery_C" } },
			{"Schematic_7-3_C", new string[] { "Recipe_FilterHazmat_C" } },
			{"Schematic_7-4_C", new string[] { "Recipe_NuclearFuelRod_C", "Recipe_ElectromagneticControlRod_C", "Recipe_UraniumCell_C", "Recipe_UraniumPellet_C", "Recipe_SulfuricAcid_C", "Build_GeneratorNuclear_CDesc_NuclearFuelRod_C" } },
			//MAM recipes
			{"Research_ACarapace_0_C", new string[] { } },
			{"Research_ACarapace_1_C", new string[] { "Recipe_Biomass_AlienCarapace_C" } },
			{"Research_ACarapace_2_C", new string[] { } },
			{"Research_ACarapace_2_1_C", new string[] { } },
			{"Research_ACarapace_3_C", new string[] { "Recipe_SpikedRebar_C" } },
			{"Research_AOrganisms_1_C", new string[] { } },
			{"Research_AOrganisms_2_C", new string[] { } },
			{"Research_AOrgans_0_C", new string[] { } },
			{"Research_AOrgans_1_C", new string[] { "Recipe_Biomass_AlienOrgans_C" } },
			{"Research_AOrgans_2_C", new string[] { } },
			{"Research_AOrgans_3_C", new string[] { } },
			{"Research_Caterium_0_C", new string[] { } },
			{"Research_Caterium_1_C", new string[] { "Recipe_IngotCaterium_C" } },
			{"Research_Caterium_2_C", new string[] { "Recipe_Quickwire_C" } },
			{"Research_Caterium_3_C", new string[] { } },
			{"Research_Caterium_3_1_C", new string[] { } },
			{"Research_Caterium_4_1_C", new string[] { "Recipe_AILimiter_C" } },
			{"Research_Caterium_4_1_1_C", new string[] { } },
			{"Research_Caterium_4_2_C", new string[] { } },
			{"Research_Caterium_4_3_C", new string[] { } },
			{"Research_Caterium_5_C", new string[] { "Recipe_HighSpeedConnector_C" } },
			{"Research_Caterium_6_1_C", new string[] { "Recipe_ComputerSuper_C" } },
			{"Research_Caterium_6_2_C", new string[] { } },
			{"Research_Caterium_6_3_C", new string[] { } },
			{"Research_Caterium_7_1_C", new string[] { } },
			{"Research_Caterium_7_2_C", new string[] { } },
			{"Research_FlowerPetals_1_C", new string[] { } },
			{"Research_FlowerPetals_2_C", new string[] { } },
			{"Research_FlowerPetals_3_C", new string[] { "Recipe_ColorCartridge_C" } },
			{"Research_Mycelia_1_C", new string[] { } },
			{"Research_Mycelia_2_C", new string[] { } },
			{"Research_Mycelia_3_C", new string[] { } },
			{"Research_Mycelia_4_C", new string[] { } },
			{"Research_Mycelia_5_C", new string[] { } },
			{"Research_Nutrients_0_C", new string[] { } },
			{"Research_Nutrients_1_C", new string[] { } },
			{"Research_Nutrients_2_C", new string[] { } },
			{"Research_Nutrients_3_C", new string[] { } },
			{"Research_Nutrients_4_C", new string[] { } },
			{"Research_PowerSlugs_1_C", new string[] { "Recipe_PowerCrystalShard_1_C" } },
			{"Research_PowerSlugs_2_C", new string[] { } },
			{"Research_PowerSlugs_3_C", new string[] { "Recipe_PowerCrystalShard_2_C" } },
			{"Research_PowerSlugs_4_C", new string[] { } },
			{"Research_PowerSlugs_5_C", new string[] { "Recipe_PowerCrystalShard_3_C" } },
			{"Research_Quartz_0_C", new string[] { } },
			{"Research_Quartz_1_1_C", new string[] { "Recipe_QuartzCrystal_C" } },
			{"Research_Quartz_1_2_C", new string[] { "Recipe_Silica_C" } },
			{"Research_Quartz_2_C", new string[] { "Recipe_CrystalOscillator_C" } },
			{"Research_Quartz_3_C", new string[] { } },
			{"Research_Quartz_3_1_C", new string[] { } },
			{"Research_Quartz_3_2_C", new string[] { } },
			{"Research_Quartz_3_3_C", new string[] { "Recipe_RadioControlUnit_C" } },
			{"Research_Quartz_4_C", new string[] { } },
			{"Research_Quartz_4_1_C", new string[] { } },
			{"Research_Sulfur_0_C", new string[] { } },
			{"Research_Sulfur_1_C", new string[] { "Recipe_Gunpowder_C", "Recipe_Nobelisk_C", "Recipe_Cartridge_C" } },
			{"Research_Sulfur_2_C", new string[] { } },
			{"Research_Sulfur_3_C", new string[] { } },
			{"Research_Sulfur_3_1_C", new string[] { } },
			{"Research_Sulfur_3_2_C", new string[] { } },
			{"Research_Sulfur_3_2_1_C", new string[] { } },
			{"Research_Sulfur_4_C", new string[] { } },
			{"Research_Sulfur_4_1_C", new string[] { } },
			{"Research_Sulfur_4_2_C", new string[] { } },
			{"Research_Sulfur_4_2_1_C", new string[] { } },
			{"Research_Sulfur_5_C", new string[] { } },
			{"Research_Sulfur_6_C", new string[] { } },
			//hard-drive recipes
			{"Schematic_Alternate_AdheredIronPlate_C", new string[] { "Recipe_Alternate_AdheredIronPlate_C" } },
			{"Schematic_Alternate_Beacon1_C", new string[] { "Recipe_Alternate_Beacon_1_C" } },
			{"Schematic_Alternate_BoltedFrame_C", new string[] { "Recipe_Alternate_BoltedFrame_C" } },
			{"Schematic_Alternate_Cable1_C", new string[] { "Recipe_Alternate_Cable_1_C" } },
			{"Schematic_Alternate_Cable2_C", new string[] { "Recipe_Alternate_Cable_2_C" } },
			{"Schematic_Alternate_CircuitBoard1_C", new string[] { "Recipe_Alternate_CircuitBoard_1_C" } },
			{"Schematic_Alternate_CircuitBoard2_C", new string[] { "Recipe_Alternate_CircuitBoard_2_C" } },
			{"Schematic_Alternate_Coal1_C", new string[] { "Recipe_Alternate_Coal_1_C" } },
			{"Schematic_Alternate_Coal2_C", new string[] { "Recipe_Alternate_Coal_2_C" } },
			{"Schematic_Alternate_CoatedCable_C", new string[] { "Recipe_Alternate_CoatedCable_C" } },
			{"Schematic_Alternate_CoatedIronPlate_C", new string[] { "Recipe_Alternate_CoatedIronPlate_C" } },
			{"Schematic_Alternate_CokeSteelIngot_C", new string[] { "Recipe_Alternate_CokeSteelIngot_C" } },
			{"Schematic_Alternate_Computer1_C", new string[] { "Recipe_Alternate_Computer_1_C" } },
			{"Schematic_Alternate_Computer2_C", new string[] { "Recipe_Alternate_Computer_2_C" } },
			{"Schematic_Alternate_Concrete_C", new string[] { "Recipe_Alternate_Concrete_C" } },
			{"Schematic_Alternate_CopperAlloyIngot_C", new string[] { "Recipe_Alternate_CopperAlloyIngot_C" } },
			{"Schematic_Alternate_CopperRotor_C", new string[] { "Recipe_Alternate_CopperRotor_C" } },
			{"Schematic_Alternate_CrystalOscillator_C", new string[] { "Recipe_Alternate_CrystalOscillator_C" } },
			{"Schematic_Alternate_DilutedPackagedFuel_C", new string[] { "Recipe_Alternate_DilutedPackagedFuel_C" } },
			{"Schematic_Alternate_ElectroAluminumScrap_C", new string[] { "Recipe_Alternate_ElectroAluminumScrap_C" } },
			{"Schematic_Alternate_ElectrodeCircuitBoard_C", new string[] { "Recipe_Alternate_ElectrodeCircuitBoard_C" } },
			{"Schematic_Alternate_ElectromagneticControlRod1_C", new string[] { "Recipe_Alternate_ElectromagneticControlRod_1_C" } },
			{"Schematic_Alternate_EnrichedCoal_C", new string[] { "Recipe_Alternate_EnrichedCoal_C" } },
			{"Schematic_Alternate_FlexibleFramework_C", new string[] { "Recipe_Alternate_FlexibleFramework_C" } },
			{"Schematic_Alternate_FusedWire_C", new string[] { "Recipe_Alternate_FusedWire_C" } },
			{"Schematic_Alternate_Gunpowder1_C", new string[] { "Recipe_Alternate_Gunpowder_1_C" } },
			{"Schematic_Alternate_HeatSink1_C", new string[] { "Recipe_Alternate_HeatSink_1_C" } },
			{"Schematic_Alternate_HeavyFlexibleFrame_C", new string[] { "Recipe_Alternate_HeavyFlexibleFrame_C" } },
			{"Schematic_Alternate_HeavyModularFrame_C", new string[] { "Recipe_Alternate_ModularFrameHeavy_C" } },
			{"Schematic_Alternate_HeavyOilResidue_C", new string[] { "Recipe_Alternate_HeavyOilResidue_C" } },
			{"Schematic_Alternate_HighSpeedConnector_C", new string[] { "Recipe_Alternate_HighSpeedConnector_C" } },
			{"Schematic_Alternate_HighSpeedWiring_C", new string[] { "Recipe_Alternate_HighSpeedWiring_C" } },
			{"Schematic_Alternate_IngotIron_C", new string[] { "Recipe_Alternate_IngotIron_C" } },
			{"Schematic_Alternate_IngotSteel1_C", new string[] { "Recipe_Alternate_IngotSteel_1_C" } },
			{"Schematic_Alternate_IngotSteel2_C", new string[] { "Recipe_Alternate_IngotSteel_2_C" } },
			{"Schematic_Alternate_InventorySlots1_C", new string[] { } },
			{"Schematic_Alternate_InventorySlots2_C", new string[] { } },
			{"Schematic_Alternate_ModularFrame_C", new string[] { "Recipe_Alternate_ModularFrame_C" } },
			{"Schematic_Alternate_Motor1_C", new string[] { "Recipe_Alternate_Motor_1_C" } },
			{"Schematic_Alternate_Nobelisk1_C", new string[] { "Recipe_Alternate_Nobelisk_1_C" } },
			{"Schematic_Alternate_NuclearFuelRod1_C", new string[] { "Recipe_Alternate_NuclearFuelRod_1_C" } },
			{"Schematic_Alternate_Plastic1_C", new string[] { "Recipe_Alternate_Plastic_1_C" } },
			{"Schematic_Alternate_PlasticSmartPlating_C", new string[] { "Recipe_Alternate_PlasticSmartPlating_C" } },
			{"Schematic_Alternate_PolyesterFabric_C", new string[] { "Recipe_Alternate_PolyesterFabric_C" } },
			{"Schematic_Alternate_PolymerResin_C", new string[] { "Recipe_Alternate_PolymerResin_C" } },
			{"Schematic_Alternate_PureAluminumIngot_C", new string[] { "Recipe_PureAluminumIngot_C" } },
			{"Schematic_Alternate_PureCateriumIngot_C", new string[] { "Recipe_Alternate_PureCateriumIngot_C" } },
			{"Schematic_Alternate_PureCopperIngot_C", new string[] { "Recipe_Alternate_PureCopperIngot_C" } },
			{"Schematic_Alternate_PureIronIngot_C", new string[] { "Recipe_Alternate_PureIronIngot_C" } },
			{"Schematic_Alternate_PureQuartzCrystal_C", new string[] { "Recipe_Alternate_PureQuartzCrystal_C" } },
			{"Schematic_Alternate_Quickwire_C", new string[] { "Recipe_Alternate_Quickwire_C" } },
			{"Schematic_Alternate_RadioControlUnit1_C", new string[] { "Recipe_Alternate_RadioControlUnit_1_C" } },
			{"Schematic_Alternate_RecycledRubber_C", new string[] { "Recipe_Alternate_RecycledRubber_C" } },
			{"Schematic_Alternate_ReinforcedIronPlate1_C", new string[] { "Recipe_Alternate_ReinforcedIronPlate_1_C" } },
			{"Schematic_Alternate_ReinforcedIronPlate2_C", new string[] { "Recipe_Alternate_ReinforcedIronPlate_2_C" } },
			{"Schematic_Alternate_ReinforcedSteelPlate_C", new string[] { "Recipe_Alternate_EncasedIndustrialBeam_C" } },
			{"Schematic_Alternate_Rotor_C", new string[] { "Recipe_Alternate_Rotor_C" } },
			{"Schematic_Alternate_RubberConcrete_C", new string[] { "Recipe_Alternate_RubberConcrete_C" } },
			{"Schematic_Alternate_Screw2_C", new string[] { "Recipe_Alternate_Screw_2_C" } },
			{"Schematic_Alternate_Screw_C", new string[] { "Recipe_Alternate_Screw_C" } },
			{"Schematic_Alternate_Silica_C", new string[] { "Recipe_Alternate_Silica_C" } },
			{"Schematic_Alternate_Stator_C", new string[] { "Recipe_Alternate_Stator_C" } },
			{"Schematic_Alternate_SteamedCopperSheet_C", new string[] { "Recipe_Alternate_SteamedCopperSheet_C" } },
			{"Schematic_Alternate_SteelCoatedPlate_C", new string[] { "Recipe_Alternate_SteelCoatedPlate_C" } },
			{"Schematic_Alternate_SteelRod_C", new string[] { "Recipe_Alternate_SteelRod_C" } },
			{"Schematic_Alternate_TurboFuel_C", new string[] { "Recipe_Alternate_Turbofuel_C" } },
			{"Schematic_Alternate_TurboHeavyFuel_C", new string[] { "Recipe_Alternate_TurboHeavyFuel_C" } },
			{"Schematic_Alternate_TurboMotor1_C", new string[] { "Recipe_Alternate_TurboMotor_1_C" } },
			{"Schematic_Alternate_UraniumCell1_C", new string[] { "Recipe_Alternate_UraniumCell_1_C" } },
			{"Schematic_Alternate_WetConcrete_C", new string[] { "Recipe_Alternate_WetConcrete_C" } },
			{"Schematic_Alternate_Wire1_C", new string[] { "Recipe_Alternate_Wire_1_C" } },
			{"Schematic_Alternate_Wire2_C", new string[] { "Recipe_Alternate_Wire_2_C" } },
		};

		public static Dictionary<string, IRecipe> GetRecipesForResearch(string research, Dictionary<string, IRecipe> recipes)
		{
			if (mapping.ContainsKey(research))
			{
				Dictionary<string, IRecipe> unlockedRecipes = new Dictionary<string, IRecipe>();
				foreach (string str in mapping[research])
				{
					unlockedRecipes.Add(recipes[str].UID, recipes[str]);
				}
				return unlockedRecipes;
			}
			if (research.Contains("ResourceSink") || research.Contains("HardDrive") || research.Contains("Inventory"))
			{
				return new Dictionary<string, IRecipe>();
			}
			if (research.StartsWith("Schematic_Alternate"))
			{
				string temp = research.Remove(0, "Schematic".Length);
				temp = temp.Insert(0, "Recipe");
				temp = SeperateNumbers(temp);
				return new Dictionary<string, IRecipe>() { { recipes[temp].UID, recipes[temp] } };
			}
			Console.WriteLine("Research " + research + " does not have a mapping!");
			return new Dictionary<string, IRecipe>();
		}

		public static Dictionary<string, IRecipe> GetAllRelevantRecipes(Dictionary<string, IRecipe> recipes)
		{
			Dictionary<string, IRecipe> allRecipes = new Dictionary<string, IRecipe>();
			foreach (string key in mapping.Keys)
			{
				foreach (string str in mapping[key])
				{
					allRecipes.Add(str, recipes[str]);
				}
			}
			return allRecipes;
		}

		public static readonly char[] numbers = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
		public static readonly char[] letters = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

		private static string SeperateNumbers(string str)
		{
			string lowerStr = str.ToLower();
			for (int i = 0; i < lowerStr.Length - 1; i++)
			{
				if (numbers.Contains(lowerStr[i]) && letters.Contains(lowerStr[i + 1]) || numbers.Contains(lowerStr[i + 1]) && letters.Contains(lowerStr[i]))
				{
					str = str.Insert(i + 1, "_");
					lowerStr = str.ToLower();
				}
			}
			return str;
		}
	}
}
