using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
