using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VisualSatisfactoryCalculator.satisfactory.DataStorage;

namespace VisualSatisfactoryCalculator.code.Utility
{
	public class Encodings
	{
		public readonly ImmutableHashSet<BasicRecipe> recipes;

		public Encodings(ImmutableHashSet<BasicRecipe> recipes)
		{
			this.recipes = recipes;
		}
	}
}
