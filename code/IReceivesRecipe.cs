using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.code
{
	public interface IReceivesRecipe
	{
		void AddRecipe(Recipe recipe, string purpose  = null);
	}
}
