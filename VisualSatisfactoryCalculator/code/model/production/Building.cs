namespace VisualSatisfactoryCalculator.satisfactory.model.production
{
	public class Building
	{
		public readonly string id;
		public readonly string displayName;
		public readonly decimal powerConsumption;
		public readonly decimal powerConsumptionExponent;

		public Building(string id, string displayName, decimal powerConsumption, decimal powerConsumptionExponent)
		{
			this.id = id;
			this.displayName = displayName;
			this.powerConsumption = powerConsumption;
			this.powerConsumptionExponent = powerConsumptionExponent;
		}
	}
}
