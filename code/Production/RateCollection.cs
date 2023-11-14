using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VisualSatisfactoryCalculator.code.Numbers;
using VisualSatisfactoryCalculator.code.Utility;

namespace VisualSatisfactoryCalculator.code.Production
{
	public class RateCollection
	{
		private readonly Dictionary<string, RationalNumber> BasicRates;
		private double Power;

		public IEnumerable<string> ItemUIDs
		{
			get
			{
				return BasicRates.Keys;
			}
		}

		public RationalNumber this[string itemUID]
		{
			get
			{
				return BasicRates[itemUID];
			}
		}


		public RateCollection() : this(0)
		{

		}

		public RateCollection(double power)
		{
			BasicRates = new Dictionary<string, RationalNumber>();
			Power = power;
		}

		public void Add(string itemUID, RationalNumber rate)
		{
			if (itemUID == Constants.MWItem.ID)
			{
				Power += rate.ToDouble();
			}
			else if (BasicRates.ContainsKey(itemUID))
			{
				BasicRates[itemUID] += rate;
			}
			else
			{
				BasicRates.Add(itemUID, rate);
			}
		}

		private void Subtract(string itemUID, RationalNumber rate)
		{
			if (BasicRates.ContainsKey(itemUID))
			{
				BasicRates[itemUID] -= rate;
			}
			else
			{
				BasicRates.Add(itemUID, -rate);
			}
		}

		public void AdjustPower(double power)
		{
			Power += power;
		}

		public RateCollection Subtract(RateCollection other)
		{
			Power -= other.Power;
			foreach (string itemUID in other.BasicRates.Keys)
			{
				Subtract(itemUID, other.BasicRates[itemUID]);
			}
			return this;
		}
	}
}
