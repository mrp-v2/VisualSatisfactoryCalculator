using System.Collections.Generic;

namespace VisualSatisfactoryCalculator.model.production
{
	public class AbstractPlan<StepType, ItemType, RecipeType> where StepType : AbstractStep<ItemType, StepType, RecipeType> where ItemType : BasicItem
	{
		protected readonly HashSet<StepType> steps;
	}
}
