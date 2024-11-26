using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Windows.Forms;

using VisualSatisfactoryCalculator.controls.user;
using VisualSatisfactoryCalculator.model.production;
using VisualSatisfactoryCalculator.satisfactory.model.production;
using VisualSatisfactoryCalculator.satisfactory.Utility;

using Connection = VisualSatisfactoryCalculator.model.production.Connection<VisualSatisfactoryCalculator.satisfactory.model.production.Item, VisualSatisfactoryCalculator.satisfactory.Production.Step, VisualSatisfactoryCalculator.satisfactory.model.production.Recipe>;
using ItemCount = VisualSatisfactoryCalculator.model.production.ItemCount<VisualSatisfactoryCalculator.satisfactory.model.production.Item>;
using ProcessedPlan = VisualSatisfactoryCalculator.model.production.ProcessedPlan<VisualSatisfactoryCalculator.satisfactory.Production.Step, VisualSatisfactoryCalculator.satisfactory.model.production.Item, VisualSatisfactoryCalculator.satisfactory.model.production.Recipe>;

namespace VisualSatisfactoryCalculator.satisfactory.Production
{
	public class Step : AbstractStep<Item, Step, Recipe>
	{
		public readonly CachedValue<ImmutableDictionary<Item, decimal>>.Managed productionRates;
		public readonly CachedValue<ImmutableDictionary<Item, decimal>>.Managed consumptionRates;
		private uint _machineCount;
		private uint _clockSpeedDecimal;
		public uint MachineCount
		{
			get
			{
				return _machineCount;
			}
			private set
			{
				_machineCount = value;
				productionRates.Invalidate();
				consumptionRates.Invalidate();
			}
		}
		public uint ClockSpeedDecimal
		{
			get
			{
				return _clockSpeedDecimal;
			}
			private set
			{
				_clockSpeedDecimal = value;
				productionRates.Invalidate();
				consumptionRates.Invalidate();
			}
		}
		private StepControl _control;

		protected override Step This
		{
			get
			{
				return this;
			}
		}

		public IEnumerable<Connection> GetIngredientConnections()
		{
			return ingredients.Connections;
		}

		public IEnumerable<Connection> GetProductConnections()
		{
			return products.Connections;
		}

		public bool HasProductConnectionFor(Item item)
		{
			return products.HasConnection(item);
		}

		public bool HasIngredientConnectionFor(Item item)
		{
			return ingredients.HasConnection(item);
		}

		public Connection GetProductConnection(Item item)
		{
			try
			{
				return products.GetConnection(item);
			}
			catch (KeyNotFoundException e)
			{
				throw new ArgumentException("This step has no product connection for item '" + item + "'", e);
			}
		}

		public Connection GetIngredientConnection(Item item)
		{
			try
			{
				return ingredients.GetConnection(item);
			}
			catch (KeyNotFoundException e)
			{
				throw new ArgumentException("This step has no ingredient connection for item '" + item + "'", e);
			}
		}

		public void AddIngredientConnection(Connection connection)
		{
			ingredients.AddConnection(connection);
			consumptionRates.Invalidate();
		}

		public void AddProductConnection(Connection connection)
		{
			products.AddConnection(connection);
			productionRates.Invalidate();
		}

		public void RemoveIngredientConnection(Connection connection)
		{
			ingredients.RemoveConnection(connection);
			consumptionRates.Invalidate();
		}

		public void RemoveProductConnection(Connection connection)
		{
			products.RemoveConnection(connection);
			productionRates.Invalidate();
		}

		public Step(Recipe recipe, Step relatedStep, Item item, bool isProductOfRelated, out bool updateRequired) : this(recipe)
		{
			if (isProductOfRelated)
			{
				if (relatedStep.HasProductConnectionFor(item))
				{
					Connection connection = relatedStep.GetProductConnection(item).AddConsumer(this);
					AddIngredientConnection(connection);
					updateRequired = true;
				}
				else
				{
					UpdateRatesFrom(relatedStep.GetRate(item, isProductOfRelated).ToCount(item), !isProductOfRelated);
					Connection connection = new Connection(item).AddProducer(relatedStep).AddConsumer(this);
					AddIngredientConnection(connection);
					relatedStep.AddProductConnection(connection);
					updateRequired = false;
				}
			}
			else
			{
				if (relatedStep.HasIngredientConnectionFor(item))
				{
					Connection connection = relatedStep.GetIngredientConnection(item).AddProducer(this);
					AddProductConnection(connection);
					updateRequired = true;
				}
				else
				{
					UpdateRatesFrom(relatedStep.GetRate(item, isProductOfRelated).ToCount(item), !isProductOfRelated);
					Connection connection = new Connection(item).AddConsumer(relatedStep).AddProducer(this);
					AddProductConnection(connection);
					relatedStep.AddIngredientConnection(connection);
					updateRequired = false;
				}
			}
		}

		public Step(Recipe recipe) : base(Plan.VERSION, recipe)
		{
			_control = default;
			_machineCount = 1;
			_clockSpeedDecimal = Constants.CLOCK_SPEED_PERCENT_FACTOR * 100;

			productionRates = new CachedValue<ImmutableDictionary<Item, decimal>>.Managed(() =>
			{
				Dictionary<Item, decimal> rates = new Dictionary<Item, decimal>();
				foreach (KeyValuePair<Item, decimal> pair in recipe.products)
				{
					rates.Add(pair.Key, CalculateCurrentRate(pair.Value));
				}
				return rates.ToImmutableDictionary();
			});
			consumptionRates = new CachedValue<ImmutableDictionary<Item, decimal>>.Managed(() =>
			{
				Dictionary<Item, decimal> rates = new Dictionary<Item, decimal>();
				foreach (KeyValuePair<Item, decimal> pair in recipe.ingredients)
				{
					rates.Add(pair.Key, CalculateCurrentRate(pair.Value));
				}
				return rates.ToImmutableDictionary();
			});
		}

		public Step(Recipe recipe, uint machineCount, uint clockSpeedThousandths) : this(recipe)
		{
			MachineCount = machineCount;
			ClockSpeedDecimal = clockSpeedThousandths;
		}

		public void AddRelatedStep(Step related, Item item, bool isProductOfRelated)
		{
			if (isProductOfRelated)
			{
				new Connection(item).AddConsumer(this).AddProducer(related);
			}
			else
			{
				new Connection(item).AddConsumer(related).AddProducer(this);
			}
		}

		/// <summary>
		/// Always positive
		/// </summary>
		private decimal CalculateDefaultItemRate(Item item, bool isItemProduct)
		{
			return CalculateDefaultItemRate(recipe.GetCount(item, isItemProduct));
		}

		private decimal CalculateDefaultItemRate(decimal recipeCount)
		{
			return 60 / recipe.time * recipeCount;
		}

		private decimal CalculateCurrentRate(decimal recipeCount)
		{
			return CalculateDefaultItemRate(recipeCount) * (ClockSpeedDecimal / (decimal)Constants.CLOCK_SPEED_FACTOR) * MachineCount;
		}

		private void UpdateControl()
		{
			if (_control != default)
			{
				_control.UpdateNumerics();
			}
		}

		/// <summary>
		/// Always positive
		/// </summary>
		public override decimal GetRate(Item item, bool isItemProduct)
		{
			if (isItemProduct)
			{
				return productionRates.Get()[item];
			}
			else
			{
				return consumptionRates.Get()[item];
			}
		}

		public void SetControl(StepControl control)
		{
			_control = control;
		}

		public void Delete(Plan plan)
		{
			foreach (Connection connection in ingredients.Connections)
			{
				connection.RemoveConsumer(this);
				if (connection.Type == ConnectionType.INCOMPLETE)
				{
					DeleteConnection(connection);
				}
			}
			foreach (Connection connection in products.Connections)
			{
				connection.RemoveProducer(this);
				if (connection.Type == ConnectionType.INCOMPLETE)
				{
					DeleteConnection(connection);
				}
			}
			plan.RemoveStep(this);
		}

		private void DeleteConnection(Connection connection)
		{
			foreach (Step step in connection.ConsumerSteps)
			{
				step.RemoveIngredientConnection(connection);
			}
			foreach (Step step in connection.ProducerSteps)
			{
				step.RemoveProductConnection(connection);
			}
		}

		public double GetPowerDraw()
		{
			Building building = recipe.building;
			return (double)building.powerConsumption * Math.Pow(ClockSpeedDecimal / (double)Constants.CLOCK_SPEED_FACTOR, (double)building.powerConsumptionExponent) * MachineCount;
		}

		protected override void UpdateRatesFrom(Dictionary<ItemCount, bool> rates)
		{
			uint newMachineCount = 0;
			foreach (KeyValuePair<ItemCount<Item>, bool> entry in rates)
			{
				uint potentialMachineCount = (uint)Math.Ceiling(entry.Key.rate / CalculateDefaultItemRate(entry.Key.item, entry.Value));
				newMachineCount = Math.Max(newMachineCount, potentialMachineCount);
			}
			MachineCount = newMachineCount;
			ClockSpeedDecimal = Constants.CLOCK_SPEED_PERCENT_FACTOR;
			uint newClockSpeedThousandths = 0;
			foreach (KeyValuePair<ItemCount<Item>, bool> entry in rates)
			{
				uint potentialClockSpeedThousandths = (uint)Math.Ceiling(entry.Key.rate / GetRate(entry.Key.item, entry.Value) * Constants.CLOCK_SPEED_PERCENT_FACTOR);
				newClockSpeedThousandths = Math.Max(newClockSpeedThousandths, potentialClockSpeedThousandths);
			}
			ClockSpeedDecimal = newClockSpeedThousandths;
			UpdateControl();
		}

		public void SetMachineCount(uint machineCount, ProcessedPlan processedPlan)
		{
			MachineCount = machineCount;
			UpdateControl();
			CascadeUpdates(processedPlan);
		}

		public void SetClockSpeedThousandths(uint clockSpeedThousandths, ProcessedPlan processedPlan)
		{
			ClockSpeedDecimal = clockSpeedThousandths;
			UpdateControl();
			CascadeUpdates(processedPlan);
		}

		private void CascadeUpdates(ProcessedPlan processedPlan)
		{
			BreadthFirstSearchHandler<Item, Step, Recipe>.CascadeUpdates(this, processedPlan);
		}
	}
}
