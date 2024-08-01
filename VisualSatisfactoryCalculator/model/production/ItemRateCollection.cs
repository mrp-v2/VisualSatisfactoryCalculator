using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.model.production
{
	public class ItemRateCollection
	{
		private readonly Dictionary<string, ItemRate> rates;
		private readonly Dictionary<string, Connection> connections;

		public ItemRateCollection()
		{
			rates = new Dictionary<string, ItemRate>();
			connections = new Dictionary<string, Connection>();
		}
	}
}
