namespace VisualSatisfactoryCalculator.code.Interfaces
{
	public interface IReceives<T>
	{
		void SendObject(T obj, string purpose);
	}
}
