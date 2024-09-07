using VisualSatisfactoryCalculator.model.production;
using VisualSatisfactoryCalculator.satisfactory.Interfaces;

namespace VisualSatisfactoryCalculator.satisfactory.DataStorage
{
	public class SimpleCustomItem : AbstractItem, IItem
	{
		public bool IsFluid { get { return false; } }

		string IHasID.ID
		{
			get
			{
				return ID;
			}
		}
		string IHasDisplayName.DisplayName
		{
			get
			{
				return DisplayName;
			}
		}

		public SimpleCustomItem(string ID, string displayName) : base(ID, displayName) { }

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
	}
}
