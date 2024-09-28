namespace VisualSatisfactoryCalculator.model.production
{
	/// <summary>
	/// The basic implementations of an item. Extend this class to provide additional functionality.
	/// </summary>
	public class BasicItem
	{
		/// <summary>
		/// The unique identifier of this item.
		/// </summary>
		public readonly string id;
		/// <summary>
		/// The display name of this item.
		/// </summary>
		public readonly string displayName;

		protected BasicItem(string id, string displayName)
		{
			this.id = id;
			this.displayName = displayName;
		}

		public override int GetHashCode()
		{
			return id.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (!(obj is BasicItem other))
			{
				return false;
			}
			return id.Equals(other.id);
		}

		public override string ToString()
		{
			return displayName;
		}
	}
}
