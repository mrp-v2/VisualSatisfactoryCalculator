using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.code
{
	public static class GenericExtensions
	{
		public static bool ContainsAny<T>(this IEnumerable<T> me, IEnumerable<T> other)
		{
			foreach (T item in other)
			{
				if (me.Contains(item))
				{
					return true;
				}
			}
			return false;
		}
	}
}
