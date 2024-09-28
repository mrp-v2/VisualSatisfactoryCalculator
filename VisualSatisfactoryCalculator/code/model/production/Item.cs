using VisualSatisfactoryCalculator.model.production;

namespace VisualSatisfactoryCalculator.satisfactory.model.production
{
	public class Item : BasicItem
	{
		public readonly decimal countDisplayFactor;

		public Item(string id, string displayName, decimal countDisplayFactor) : base(id, displayName)
		{
			this.countDisplayFactor = countDisplayFactor;
		}

		public string ToString(decimal count)
		{
			return (count / countDisplayFactor) + " " + ToString();
		}
	}
}
