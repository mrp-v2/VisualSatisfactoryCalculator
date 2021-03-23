using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.code.Utility
{
	public class CachedValue<T>
	{
		private bool valid;
		private readonly Func<T> valueProvider;
		private T value;

		public CachedValue(Func<T> valueProvider)
		{
			this.valueProvider = valueProvider;
			valid = false;
		}

		public T Get()
		{
			if (!valid)
			{
				value = valueProvider();
			}
			return value;
		}

		public void Invalidate()
		{
			valid = false;
		}

		public void InvalidateIf(T invalidValue)
		{
			if (value.Equals(invalidValue))
			{
				Invalidate();
			}
		}
	}
}
