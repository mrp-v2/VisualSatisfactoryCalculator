using System;
using System.Collections.Generic;
using VisualSatisfactoryCalculator.code.DataStorage;

namespace VisualSatisfactoryCalculator.code.Interfaces
{
	public interface IRecipe : IMyCloneable<IRecipe>, IEquatable<IRecipe>, IHasUID
	{
		string GetMachine();
		List<ItemCount> GetItemCounts();
		decimal GetCraftTime();
		string GetUID();
	}
}
