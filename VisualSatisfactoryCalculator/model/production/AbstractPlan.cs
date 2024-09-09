using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.model.production
{
	public class AbstractPlan<StepType, ItemType, RecipeType> where StepType : AbstractStep<ItemType, RecipeType> where ItemType : AbstractItem
	{
		protected readonly HashSet<StepType> steps;
	}
}
