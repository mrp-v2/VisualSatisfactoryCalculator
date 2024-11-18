using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Windows.Forms;

using VisualSatisfactoryCalculator.model.util;

namespace VisualSatisfactoryCalculator.satisfactory.Utility
{
	public abstract class CachedValue<T>
	{
		private readonly Func<T> _valueProvider;
		private T _value;

		private CachedValue(Func<T> valueProvider)
		{
			_valueProvider = valueProvider;
			_value = default;
		}

		private void RecalculateValue()
		{
			_value = _valueProvider();
		}

		protected abstract void EnsureValid();

		public T Get()
		{
			EnsureValid();
			return _value;
		}

		public class Managed : CachedValue<T>
		{
			private bool _valid;
			private EventHandler _invalidationCallback;

			public Managed(Func<T> valueProvider) : base(valueProvider)
			{
				_valid = false;
				_invalidationCallback = default;
			}

			protected override void EnsureValid()
			{
				if (!_valid)
				{
					RecalculateValue();
					_valid = true;
				}
			}

			public void Invalidate()
			{
				if (_valid)
				{
					_valid = false;
					if (_invalidationCallback != null)
					{
						_invalidationCallback(this, EventArgs.Empty);
					}
				}
			}

			public void InvalidateIf(T invalidValue)
			{
				if (_valid)
				{
					if (_value.Equals(invalidValue))
					{
						Invalidate();
					}
				}
			}

			public void InvalidateIf(ICollection<T> invalidValues)
			{
				if (_valid)
				{
					if (invalidValues.Contains(_value))
					{
						Invalidate();
					}
				}
			}

			public void AddInvalidationCallback(EventHandler e)
			{
				if (_invalidationCallback == null)
				{
					_invalidationCallback = e;
				}
				else
				{
					_invalidationCallback += e;
				}
			}
		}

		public class Versioned : CachedValue<T>
		{
			private int _version;
			private readonly Mutable<int> _reference_version;

			public Versioned(Mutable<int> reference, Func<T> valueProvider) : base(valueProvider)
			{
				_version = -1;
				_reference_version = reference;
			}

			protected override void EnsureValid()
			{
				if (_version != _reference_version.value)
				{
					RecalculateValue();
					_version = _reference_version.value;
				}
			}
		}
	}
}
