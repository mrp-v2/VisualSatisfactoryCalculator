using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.util.collections
{
	public class ViewableDictionary<TKey, TValue> : Dictionary<TKey, TValue>
	{
		public readonly IReadOnlyDictionary<TKey, TValue> ReadOnly;

		public ViewableDictionary()
		{
			ReadOnly = new ReadOnlyView(this);
		}

		public ViewableDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary)
		{
			ReadOnly = new ReadOnlyView(this);
		}

		private class ReadOnlyView : IReadOnlyDictionary<TKey, TValue>
		{
			private readonly ViewableDictionary<TKey, TValue> _dictionary;

			public int Count
			{
				get
				{
					return _dictionary.Count;
				}
			}

			public TValue this[TKey key]
			{
				get
				{
					return _dictionary[key];
				}
			}

			public IEnumerable<TKey> Keys
			{
				get
				{
					return _dictionary.Keys;
				}
			}

			public IEnumerable<TValue> Values
			{
				get
				{
					return _dictionary.Values;
				}
			}

			internal ReadOnlyView(ViewableDictionary<TKey, TValue> dictionary)
			{
				_dictionary = dictionary;
			}

			public bool ContainsKey(TKey key)
			{
				return _dictionary.ContainsKey(key);
			}

			public bool TryGetValue(TKey key, out TValue value)
			{
				return _dictionary.TryGetValue(key, out value);
			}

			public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
			{
				return _dictionary.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return _dictionary.GetEnumerator();
			}
		}
	}
}
