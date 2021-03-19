using System;

namespace VisualSatisfactoryCalculator.code.Interfaces
{
	public interface IItem : IEquatable<IItem>, IEncoder
	{
		bool IsLiquid { get; }
		string ToString(decimal rate);
	}
}
