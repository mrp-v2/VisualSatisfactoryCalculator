using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.code
{
	public interface IReceives<T>
	{
		void SendObject(T obj, string purpose = null);
	}
}
