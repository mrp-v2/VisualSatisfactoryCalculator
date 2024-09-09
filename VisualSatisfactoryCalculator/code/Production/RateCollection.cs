using System.Collections.Generic;

using VisualSatisfactoryCalculator.model.production;
using VisualSatisfactoryCalculator.satisfactory.JSONClasses;
using VisualSatisfactoryCalculator.satisfactory.Numbers;
using VisualSatisfactoryCalculator.satisfactory.Utility;

namespace VisualSatisfactoryCalculator.satisfactory.Production
{
	public class RateCollection
	{
		private readonly ItemRateCollection<JSONItem> BasicRates;
		private double Power;

		public IEnumerable<JSONItem> Items
		{
			get
			{
				return BasicRates.Keys;
			}
		}

		public RationalNumber this[JSONItem item]
		{
			get
			{
				return BasicRates[item].rate;
			}
		}


		public RateCollection() : this(0)
		{

		}

		public RateCollection(double power)
		{
			BasicRates = new ItemRateCollection<JSONItem>();
			Power = power;
		}

		public void Add(JSONItem item, RationalNumber rate)
		{
			if (item == Constants.MWItem)
			{
				Power += rate.ToDouble();
			}
			else if (BasicRates.ContainsKey(item))
			{
				BasicRates[item] += rate;
			}
			else
			{
				BasicRates.Add(new ItemRate<JSONItem>(item, rate));
			}
		}

		private void Subtract(JSONItem item, RationalNumber rate)
		{
			if (BasicRates.ContainsKey(item))
			{
				BasicRates[item] -= rate;
			}
			else
			{
				BasicRates.Add(new ItemRate<JSONItem>(item, -rate));
			}
		}

		public void AdjustPower(double power)
		{
			Power += power;
		}

		public RateCollection Subtract(RateCollection other)
		{
			Power -= other.Power;
			foreach (JSONItem item in other.BasicRates.Keys)
			{
				Subtract(item, other.BasicRates[item].rate);
			}
			return this;
		}
	}
}
