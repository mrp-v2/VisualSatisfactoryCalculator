using Newtonsoft.Json;

using VisualSatisfactoryCalculator.satisfactory.DataStorage;

namespace VisualSatisfactoryCalculator.satisfactory.JSONClasses
{
	public class JSONRecipe : JSONRecipeBase
	{
		[JsonConstructor]
		public JSONRecipe(string ClassName, string mDisplayName, string mIngredients, string mProduct, string mManufactoringDuration, string mProducedIn, string mVariablePowerConsumptionConstant, string mVariablePowerConsumptionFactor)
			: base(ClassName, decimal.Parse(mManufactoringDuration), mProducedIn, mIngredients, mProduct, mDisplayName)
		{
		}
	}
}
