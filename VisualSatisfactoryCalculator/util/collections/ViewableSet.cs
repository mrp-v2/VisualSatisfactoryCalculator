using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.util.collections
{
	public class ViewableSet<T> : HashSet<T>
	{
		public readonly IReadOnlySet<T> ReadOnly;

		public ViewableSet()
		{
			ReadOnly = new ReadOnlyView(this);
		}

		public ViewableSet(IEnumerable<T> collection) : base(collection)
		{
			ReadOnly = new ReadOnlyView(this);
		}

		private class ReadOnlyView : IReadOnlySet<T>
		{
			private readonly ViewableSet<T> _set;

			public int Count
			{
				get
				{
					return _set.Count;
				}
			}

			internal ReadOnlyView(ViewableSet<T> set)
			{
				_set = set;
			}

			public bool Contains(T item)
			{
				return _set.Contains(item);
			}

			public IEnumerator<T> GetEnumerator()
			{
				return _set.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return _set.GetEnumerator();
			}
		}
	}
}
