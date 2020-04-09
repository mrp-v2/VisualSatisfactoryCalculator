using System;
using System.Collections.Generic;
using VisualSatisfactoryCalculator.code.DataStorage;

namespace VisualSatisfactoryCalculator.code.Interfaces
{
	public interface IRecipe : IEquatable<IRecipe>, IEncoder
	{
		string GetMachineUID();
		List<ItemCount> GetItemCounts();
		decimal GetCraftTime();
		string ToString(List<IEncoder> encodings);
	}
}
