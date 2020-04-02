namespace VisualSatisfactoryCalculator.code
{
	public interface IReceives<T>
	{
		void SendObject(T obj, string purpose);
	}
}
