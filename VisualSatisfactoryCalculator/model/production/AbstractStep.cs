using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.model.production
{
	public abstract class AbstractStep
	{
		protected readonly ItemRateCollection<Connection> products;
		protected readonly ItemRateCollection<Connection> ingredients;
	}
}
