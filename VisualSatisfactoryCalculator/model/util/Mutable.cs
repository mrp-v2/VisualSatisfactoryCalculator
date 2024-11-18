using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.model.util
{
	public class Mutable<T> where T : struct
	{
		public T value;

		public Mutable(T value)
		{
			this.value = value;
		}

		public Mutable() : this(default) { }
	}
}
