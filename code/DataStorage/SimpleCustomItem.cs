using VisualSatisfactoryCalculator.code.Extensions;
using VisualSatisfactoryCalculator.code.Interfaces;

namespace VisualSatisfactoryCalculator.code.DataStorage
{
	public class SimpleCustomItem : IItem
	{
		public string ID { get; }
		public string DisplayName { get; }
		public bool IsFluid { get { return false; } }

		public SimpleCustomItem(string ID, string displayName)
		{
			this.ID = ID;
			DisplayName = displayName;
		}

		public override string ToString()
		{
			return DisplayName;
		}

		public override int GetHashCode()
		{
			return ID.GetHashCode();
		}

		public bool EqualID(string id)
		{
			return ID.Equals(id);
		}

		public bool Equals(IItem other)
		{
			if (!(other is SimpleCustomItem))
			{
				return false;
			}

			return ID.Equals((other as SimpleCustomItem).ID);
		}

		public bool EqualID(IHasID obj)
		{
			return obj.EqualID(ID);
		}

		public string ToString(decimal rate)
		{
			return rate.ToPrettyString();
		}
	}
}
