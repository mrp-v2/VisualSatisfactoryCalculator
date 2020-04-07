using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.code.Interfaces
{
	public interface IItem : IEquatable<IItem>, IHasUniqueID
	{
		string GetUniqueID();
	}
}
