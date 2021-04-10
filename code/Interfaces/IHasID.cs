namespace VisualSatisfactoryCalculator.code.Interfaces
{
	public interface IHasID
	{
		bool EqualID(string id);
		bool EqualID(IHasID obj);
		string ID { get; }
	}
}
