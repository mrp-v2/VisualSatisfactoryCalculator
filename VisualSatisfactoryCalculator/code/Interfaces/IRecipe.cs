using System;
using System.Collections.Generic;

using VisualSatisfactoryCalculator.satisfactory.Numbers;
using VisualSatisfactoryCalculator.satisfactory.Utility;
using VisualSatisfactoryCalculator.model.production;
using VisualSatisfactoryCalculator.satisfactory.JSONClasses;

namespace VisualSatisfactoryCalculator.satisfactory.Interfaces
{
	public interface IRecipe : IEquatable<IRecipe>, IEncoder
	{
		string MachineUID { get; }
		string ToString(Encodings encodings);
		/// <summary>
		/// Creates a formatted string representation of this recipe.
		/// </summary>
		/// <param name="encodings">The encodings to use</param>
		/// <param name="format">The format of the string representation. The format is given as a string with {...}s indicating where to insert specific information about the recipe.
		/// <list type="bullet">
		/// <item><term>{name}</term><description>display name</description></item>
		/// <item><term>{conversion}</term><description>the ingredients and the products</description></item>
		/// <item><term>{time}</term><description>the amount of time the recipe takes</description></item>
		/// <item><term>{machine}</term><description>the machine the recipe uses</description></item>
		/// </list>
		/// </param>
		/// <returns></returns>
		string ToString(Encodings encodings, string format);
		RationalNumber GetCountFor(JSONItem item, bool isProduct);
	}
}
