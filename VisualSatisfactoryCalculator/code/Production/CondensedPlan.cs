using System;
using System.Collections.Generic;

using VisualSatisfactoryCalculator.satisfactory.DataStorage;
using VisualSatisfactoryCalculator.satisfactory.Interfaces;
using VisualSatisfactoryCalculator.satisfactory.Numbers;
using VisualSatisfactoryCalculator.satisfactory.Utility;

using Connection = VisualSatisfactoryCalculator.model.production.Connection<VisualSatisfactoryCalculator.satisfactory.JSONClasses.JSONItem,
	VisualSatisfactoryCalculator.satisfactory.Production.Step, VisualSatisfactoryCalculator.satisfactory.DataStorage.BasicRecipe>;

namespace VisualSatisfactoryCalculator.satisfactory.Production
{
	[Serializable]
	public class CondensedPlan
	{
		private readonly HashSet<CondensedStep> _steps = new HashSet<CondensedStep>();
		private readonly HashSet<CondensedConnection> _connections = new HashSet<CondensedConnection>();

		public CondensedPlan(Plan plan)
		{
			CondensingContext context = new CondensingContext();
			int id = 0;
			foreach (Step step in plan.steps)
			{
				context._stepIDs.Add(step, id++);
			}
			id = 0;
			foreach (Connection connection in plan.processedPlan.Get().GetAllConnections())
			{
				context._connectionIDs.Add(connection, id++);
			}
			foreach (Step step in plan.steps)
			{
				_steps.Add(new CondensedStep(step, context));
			}
			foreach (Connection connection in plan.processedPlan.Get().GetAllConnections())
			{
				_connections.Add(new CondensedConnection(connection, context));
			}
		}

		public Plan ToPlan(JsonEncodings encodings)
		{
			Plan plan = new Plan();
			ExpandingContext context = new ExpandingContext();
			foreach (CondensedStep condensedStep in _steps)
			{
				Step step = new Step((BasicRecipe)encodings[condensedStep._recipeID], condensedStep._machineCount, condensedStep._clockSpeedThousandths);
				context.stepIDs.Add(condensedStep._id, step);
				plan.steps.Add(step);
			}
			foreach (CondensedConnection condensedConnection in _connections)
			{
				throw new NotImplementedException();
				//new Connection(condensedConnection, context, _steps);
			}
			return plan;
		}

		public class ExpandingContext
		{
			public readonly Dictionary<int, Step> stepIDs = new Dictionary<int, Step>();
		}

		internal class CondensingContext
		{
			internal readonly Dictionary<Connection, int> _connectionIDs = new Dictionary<Connection, int>();
			internal readonly Dictionary<Step, int> _stepIDs = new Dictionary<Step, int>();
		}

		[Serializable]
		public class CondensedStep
		{
			internal readonly int _id;
			internal readonly string _recipeID;
			internal readonly uint _machineCount;
			internal readonly ushort _clockSpeedThousandths;

			internal CondensedStep(Step step, CondensingContext context)
			{
				_id = context._stepIDs[step];
				_recipeID = step.recipe.ID;
				_machineCount = step.MachineCount;
				_clockSpeedThousandths = step.ClockSpeedThousandths;
			}
		}

		[Serializable]
		public class CondensedConnection
		{
			internal readonly int _id;
			public readonly Dictionary<int, RationalNumber> consumers = new Dictionary<int, RationalNumber>();
			public readonly Dictionary<int, RationalNumber> producers = new Dictionary<int, RationalNumber>();
			internal readonly string _itemID;

			internal CondensedConnection(Connection connection, CondensingContext context)
			{
				_id = context._connectionIDs[connection];
				_itemID = connection.item.id;
				foreach (Step step in connection.ConsumerSteps)
				{
					consumers.Add(context._stepIDs[step], connection.GetRate(step, true));
				}
				foreach (Step step in connection.ProducerSteps)
				{
					producers.Add(context._stepIDs[step], connection.GetRate(step, false));
				}
			}
		}
	}
}
