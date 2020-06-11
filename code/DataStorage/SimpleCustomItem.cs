using VisualSatisfactoryCalculator.code.Interfaces;

namespace VisualSatisfactoryCalculator.code.DataStorage
{
	public class SimpleCustomItem : IItem
	{
		public string UID { get; }
		public string DisplayName { get; }
		public bool IsLiquid { get { return false; } }

		public SimpleCustomItem(string UID, string displayName)
		{
			this.UID = UID;
			DisplayName = displayName;
		}

		public override string ToString()
		{
			return DisplayName;
		}

		public override int GetHashCode()
		{
			return UID.GetHashCode();
		}

		public bool EqualID(string id)
		{
			return UID.Equals(id);
		}

		public bool Equals(IItem other)
		{
			if (!(other is SimpleCustomItem))
			{
				return false;
			}

			return UID.Equals((other as SimpleCustomItem).UID);
		}

		public bool EqualID(IHasUID obj)
		{
			return obj.EqualID(UID);
		}
	}
}
