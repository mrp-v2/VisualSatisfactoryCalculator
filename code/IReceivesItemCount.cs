using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.code
{
	public interface IReceivesItemCount
	{
		void AddItemCount(ItemCount itemCount, string purpose = null);
	}
}
