using System.Collections.Generic;

using VisualSatisfactoryCalculator.model.production;
using VisualSatisfactoryCalculator.satisfactory.model.production;
using VisualSatisfactoryCalculator.satisfactory.Utility;

namespace VisualSatisfactoryCalculator.satisfactory.Production
{
	public class RateCollection
	{
		private readonly ItemCountCollection<Item> _basicRates;
		private double _power;

		public IEnumerable<Item> Items
		{
			get
			{
				return _basicRates.Keys;
			}
		}

		public decimal this[Item item]
		{
			get
			{
				return _basicRates[item].rate;
			}
		}


		public RateCollection() : this(0)
		{

		}

		public RateCollection(double power)
		{
			_basicRates = new ItemCountCollection<Item>();
			_power = power;
		}

		public void Add(Item item, decimal rate)
		{
			if (item == Constants.MW_ITEM)
			{
				_power += (double)rate;
			}
			else if (_basicRates.ContainsKey(item))
			{
				_basicRates[item] += rate;
			}
			else
			{
				_basicRates.Add(new ItemCount<Item>(item, rate));
			}
		}

		private void Subtract(Item item, decimal rate)
		{
			if (_basicRates.ContainsKey(item))
			{
				_basicRates[item] -= rate;
			}
			else
			{
				_basicRates.Add(new ItemCount<Item>(item, -rate));
			}
		}

		public void AdjustPower(double power)
		{
			_power += power;
		}

		public RateCollection Subtract(RateCollection other)
		{
			_power -= other._power;
			foreach (Item item in other._basicRates.Keys)
			{
				Subtract(item, other._basicRates[item].rate);
			}
			return this;
		}
	}
}
