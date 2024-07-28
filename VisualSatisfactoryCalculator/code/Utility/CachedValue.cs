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
		private EventHandler valueCalculatedCallback;

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
				if (valueCalculatedCallback != null)
				{
					valueCalculatedCallback(this, EventArgs.Empty);
				}
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

		public void AddValueCalculatedCallback(EventHandler e)
		{
			if (valueCalculatedCallback == null)
			{
				valueCalculatedCallback = e;
			}
			else
			{
				valueCalculatedCallback += e;
			}
		}
	}
}
