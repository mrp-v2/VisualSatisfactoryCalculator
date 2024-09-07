using System;

namespace VisualSatisfactoryCalculator.satisfactory.Interfaces
{
	public interface IItem : IEquatable<IItem>, IEncoder
	{
		bool IsFluid { get; }
	}
}
