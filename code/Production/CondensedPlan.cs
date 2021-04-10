using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VisualSatisfactoryCalculator.code.Interfaces;
using VisualSatisfactoryCalculator.code.Utility;

namespace VisualSatisfactoryCalculator.code.Production
{
	[Serializable]
	public class CondensedPlan
	{
		private readonly HashSet<CondensedStep> steps = new HashSet<CondensedStep>();
		private readonly HashSet<CondensedConnection> connections = new HashSet<CondensedConnection>();

		public CondensedPlan(Plan plan)
		{
			CondensingContext context = new CondensingContext();
			int id = 0;
			foreach (Step step in plan.Steps)
			{
				context.stepIDs.Add(step, id++);
			}
			id = 0;
			foreach (Connection connection in plan.ProcessedPlan.Get().GetAllConnections())
			{
				context.connectionIDs.Add(connection, id++);
			}
			foreach (Step step in plan.Steps)
			{
				steps.Add(new CondensedStep(step, context));
			}
			foreach (Connection connection in plan.ProcessedPlan.Get().GetAllConnections())
			{
				connections.Add(new CondensedConnection(connection, context));
			}
		}

		public Plan ToPlan(Encodings encodings)
		{
			Plan plan = new Plan();
			ExpandingContext context = new ExpandingContext();
			foreach (CondensedStep condensedStep in steps)
			{
				Step step = new Step((IRecipe)encodings[condensedStep.RecipeID]);
				step.SetMultiplier(condensedStep.multiplier, false);
				context.stepIDs.Add(condensedStep.ID, step);
				plan.Steps.Add(step);
			}
			foreach (CondensedConnection condensedConnection in connections)
			{
				new Connection(condensedConnection, context, steps);
			}
			return plan;
		}

		public class ExpandingContext
		{
			public readonly Dictionary<int, Step> stepIDs = new Dictionary<int, Step>();
		}

		internal class CondensingContext
		{
			internal readonly Dictionary<Connection, int> connectionIDs = new Dictionary<Connection, int>();
			internal readonly Dictionary<Step, int> stepIDs = new Dictionary<Step, int>();
		}

		[Serializable]
		public class CondensedStep
		{
			internal readonly int ID;
			internal readonly string RecipeID;
			internal readonly decimal multiplier;

			internal CondensedStep(Step step, CondensingContext context)
			{
				ID = context.stepIDs[step];
				RecipeID = step.Recipe.ID;
				multiplier = step.Multiplier;
			}
		}

		[Serializable]
		public class CondensedConnection
		{
			internal readonly int ID;
			public readonly Dictionary<int, decimal> Consumers = new Dictionary<int, decimal>();
			public readonly Dictionary<int, decimal> Producers = new Dictionary<int, decimal>();
			internal readonly string ItemID;

			internal CondensedConnection(Connection connection, CondensingContext context)
			{
				ID = context.connectionIDs[connection];
				ItemID = connection.ItemID;
				foreach (Step step in connection.GetConsumerSteps())
				{
					Consumers.Add(context.stepIDs[step], connection.GetConsumerRate(step));
				}
				foreach (Step step in connection.GetProducerSteps())
				{
					Producers.Add(context.stepIDs[step], connection.GetProducerRate(step));
				}
			}
		}
	}
}
