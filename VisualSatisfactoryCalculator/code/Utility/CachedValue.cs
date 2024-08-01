using System;
using System.Collections.Generic;

namespace VisualSatisfactoryCalculator.code.Utility
{
	public class CachedValue<T>
	{
		private bool valid;
		private readonly Func<T> valueProvider;
		private T value;
		private EventHandler invalidationCallback;

		public CachedValue(Func<T> valueProvider)
		{
			this.valueProvider = valueProvider;
			valid = false;
			invalidationCallback = default;
		}

		public T Get()
		{
			if (!valid)
			{
				value = valueProvider();
				valid = true;
			}
			return value;
		}

		public void Invalidate()
		{
			if (valid)
			{
				valid = false;
				if (invalidationCallback != null)
				{
					invalidationCallback(this, EventArgs.Empty);
				}
			}
		}

		public void InvalidateIf(T invalidValue)
		{
			if (valid)
			{
				if (value.Equals(invalidValue))
				{
					Invalidate();
				}
			}
		}

		public void InvalidateIf(ICollection<T> invalidValues)
		{
			if (valid)
			{
				if (invalidValues.Contains(value))
				{
					Invalidate();
				}
			}
		}

		public void AddInvalidationCallback(EventHandler e)
		{
			if (invalidationCallback == null)
			{
				invalidationCallback = e;
			}
			else
			{
				invalidationCallback += e;
			}
		}
	}
}
