namespace VisualSatisfactoryCalculator.model.production
{
	public delegate void OnConnectionChanged();

	internal static class Util
	{
		public static ItemCount<ItemType> ToCount<ItemType>(this decimal count, ItemType item) where ItemType : BasicItem
		{
			return new ItemCount<ItemType>(item, count);
		}
	}
}
