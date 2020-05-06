namespace VisualSatisfactoryCalculator.code.Interfaces
{
	public interface IHasUID
	{
		bool EqualID(string id);
		bool EqualID(IHasUID obj);
		string UID { get; }
	}
}
