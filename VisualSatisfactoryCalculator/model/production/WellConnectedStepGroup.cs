using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace VisualSatisfactoryCalculator.model.production
{
	public class WellConnectedStepGroup
	{
		public readonly bool editable;

		public WellConnectedStepGroup(bool editable)
		{
			this.editable = editable;
		}
	}
}
