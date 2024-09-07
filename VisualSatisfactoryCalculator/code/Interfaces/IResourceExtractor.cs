using VisualSatisfactoryCalculator.satisfactory.Numbers;

namespace VisualSatisfactoryCalculator.satisfactory.Interfaces
{
	interface IResourceExtractor : IBuilding
	{
		RationalNumber CycleTime { get; }
		RationalNumber ItemsPerCycle { get; }
		string AllowedResourceForms { get; }
		bool OnlySpecificResources { get; }
		string[] AllowedResources { get; }
	}
}
