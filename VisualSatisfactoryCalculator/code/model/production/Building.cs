using VisualSatisfactoryCalculator.satisfactory.Numbers;

namespace VisualSatisfactoryCalculator.satisfactory.model.production
{
	public class Building
	{
		public readonly string id;
		public readonly string displayName;
		public readonly RationalNumber powerConsumption;
		public readonly RationalNumber powerConsumptionExponent;

		public Building(string id, string displayName, RationalNumber powerConsumption, RationalNumber powerConsumptionExponent)
		{
			this.id = id;
			this.displayName = displayName;
			this.powerConsumption = powerConsumption;
			this.powerConsumptionExponent = powerConsumptionExponent;
		}
	}
}
