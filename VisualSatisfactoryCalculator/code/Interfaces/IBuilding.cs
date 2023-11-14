using VisualSatisfactoryCalculator.code.Numbers;

namespace VisualSatisfactoryCalculator.code.Interfaces
{
	public interface IBuilding : IEncoder
	{
		RationalNumber PowerConsumption { get; }
		RationalNumber PowerConsumptionExponent { get; }
	}
}
