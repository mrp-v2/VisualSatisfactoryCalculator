namespace VisualSatisfactoryCalculator.model.production
{
	internal static class Util
	{
		public static ItemCount<ItemType> ToCount<ItemType>(this decimal count, ItemType item) where ItemType : BasicItem
		{
			return new ItemCount<ItemType>(item, count);
		}
	}
}
