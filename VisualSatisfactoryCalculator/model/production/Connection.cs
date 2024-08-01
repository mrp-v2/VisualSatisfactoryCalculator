using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VisualSatisfactoryCalculator.code.Numbers;
using VisualSatisfactoryCalculator.code.Utility;

namespace VisualSatisfactoryCalculator.model.production
{
	public class Connection<ItemType> where ItemType : AbstractItem
	{
		public readonly ItemType Item;
		/// <summary>
		/// Steps that produce items flowing into this connection.
		/// </summary>
		private readonly Dictionary<AbstractStep<ItemType>, RationalNumber> producers;
		/// <summary>
		/// Steps that consume items flowing out of this connection.
		/// </summary>
		private readonly Dictionary<AbstractStep<ItemType>, RationalNumber> consumers;

		private readonly CachedValue<IEnumerable<AbstractStep<ItemType>>> steps;

		public IEnumerable<AbstractStep<ItemType>> Steps
		{
			get
			{
				return steps.Get();
			}
		}

		public ConnectionType Type
		{
			get
			{
				if (producers.Count == 1 && consumers.Count == 1)
				{
					return ConnectionType.SINGLE;
				}
				else if (producers.Count == 0 || consumers.Count == 0)
				{
					return ConnectionType.INCOMPLETE;
				}
				else
				{
					return ConnectionType.MULTI;
				}
			}
		}

		public Connection(ItemType item)
		{
			Item = item;
			producers = new Dictionary<AbstractStep<ItemType>, RationalNumber>();
			consumers = new Dictionary<AbstractStep<ItemType>, RationalNumber>();

			steps = new CachedValue<IEnumerable<AbstractStep<ItemType>>>(() =>
			{
				return new HashSet<AbstractStep<ItemType>>(Enumerable.Concat(producers.Keys, consumers.Keys));
			});
		}

		public override bool Equals(object obj)
		{
			return this == obj;
		}

		public override int GetHashCode()
		{
			return Item.GetHashCode() * producers.Count * consumers.Count;
		}

		public void UpdateRatesFrom(ItemRate<ItemType> rate, bool isConsuming)
		{
			throw new NotImplementedException();
		}

		public void UpdateRatesFrom(HashSet<object> visited, HashSet<AbstractStep<ItemType>> toVisit)
		{
			throw new NotImplementedException();
		}

		public void CascadingUpdateRatesFrom(ItemRate<ItemType> rate, bool isConsuming)
		{
			UpdateRatesFrom(rate, isConsuming);
			CascadingUpdateHandler<ItemType>.CascadeUpdates(this);
		}


	}

	public enum ConnectionType
	{
		INCOMPLETE,
		SINGLE,
		MULTI,
	}
}
