using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VisualSatisfactoryCalculator.code.Numbers;

namespace VisualSatisfactoryCalculator.code.Interfaces
{
	interface IResourceExtractor : IBuilding
	{
		RationalNumber CycleTime { get; }
		RationalNumber ItemsPerCycle { get; }
		string AllowedResourceForms { get; }
		bool OnlySpecificResources { get; }
		string[] AllowedResources { get; }
	}
}
