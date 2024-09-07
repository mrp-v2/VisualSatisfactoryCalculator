using VisualSatisfactoryCalculator.satisfactory.Numbers;

namespace VisualSatisfactoryCalculator.satisfactory.Interfaces
{
	public interface IBuilding : IEncoder
	{
		RationalNumber PowerConsumption { get; }
		RationalNumber PowerConsumptionExponent { get; }
	}
}
