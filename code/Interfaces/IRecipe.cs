using System;
using System.Collections.Generic;

using VisualSatisfactoryCalculator.code.DataStorage;

namespace VisualSatisfactoryCalculator.code.Interfaces
{
	public interface IRecipe : IEquatable<IRecipe>, IEncoder
	{
		string MachineUID { get; }
		decimal CraftTime { get; }
		string ToString(Dictionary<string, IEncoder> encodings);
		Dictionary<string, ItemCount> Ingredients { get; }
		Dictionary<string, ItemCount> Products { get; }
		decimal GetCountFor(string itemUID, bool isProduct);
	}
}
