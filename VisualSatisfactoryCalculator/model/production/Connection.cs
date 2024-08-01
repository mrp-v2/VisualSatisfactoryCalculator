using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VisualSatisfactoryCalculator.code.Numbers;

namespace VisualSatisfactoryCalculator.model.production
{
	public class Connection
	{
		public readonly string ItemUID;
		/// <summary>
		/// Steps that produce items flowing into this connection.
		/// </summary>
		private readonly Dictionary<AbstractStep, RationalNumber> producers;
		/// <summary>
		/// Steps that consume items flowing out of this connection.
		/// </summary>
		private readonly Dictionary<AbstractStep, RationalNumber> consumers;

		public Connection(string itemUID)
		{
			ItemUID = itemUID;
			producers = new Dictionary<AbstractStep, RationalNumber>();
			consumers = new Dictionary<AbstractStep, RationalNumber>();
		}
	}
}
