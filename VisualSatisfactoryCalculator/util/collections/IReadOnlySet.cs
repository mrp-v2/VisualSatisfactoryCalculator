using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.util.collections
{
	public interface IReadOnlySet<T> : IReadOnlyCollection<T>
	{
		public bool Contains(T item);
	}
}
