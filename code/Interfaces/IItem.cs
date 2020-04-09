using System;

namespace VisualSatisfactoryCalculator.code.Interfaces
{
	public interface IItem : IEquatable<IItem>, IEncoder
	{
		bool IsLiquid();
	}
}
