namespace VisualSatisfactoryCalculator.satisfactory.Interfaces
{
	public interface IBuilding : IEncoder
	{
		decimal PowerConsumption { get; }
		decimal PowerConsumptionExponent { get; }
	}
}
