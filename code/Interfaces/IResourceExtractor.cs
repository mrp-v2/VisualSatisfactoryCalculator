using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.code.Interfaces
{
	interface IResourceExtractor : IBuilding
	{
		decimal CycleTime { get; }
		decimal ItemsPerCycle { get; }
		string AllowedResourceForms { get; }
		bool OnlySpecificResources { get; }
		string AllowedResources { get; }
	}
}
