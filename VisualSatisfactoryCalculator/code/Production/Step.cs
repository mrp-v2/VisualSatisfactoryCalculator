using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using VisualSatisfactoryCalculator.satisfactory.Numbers;
using VisualSatisfactoryCalculator.satisfactory.Utility;
using VisualSatisfactoryCalculator.controls.user;
using VisualSatisfactoryCalculator.model.production;
using Connection = VisualSatisfactoryCalculator.model.production.Connection<VisualSatisfactoryCalculator.satisfactory.model.production.Item, VisualSatisfactoryCalculator.satisfactory.Production.Step, VisualSatisfactoryCalculator.satisfactory.model.production.Recipe>;
using ItemCount = VisualSatisfactoryCalculator.model.production.ItemCount<VisualSatisfactoryCalculator.satisfactory.model.production.Item>;
using VisualSatisfactoryCalculator.satisfactory.model.production;

namespace VisualSatisfactoryCalculator.satisfactory.Production
{
	public class Step : AbstractStep<Item, Step, Recipe>
	{
		public readonly CachedValue<bool> hasNormalProductConnections;
		public readonly CachedValue<IImmutableSet<Connection>> normalIngredientConnections;
		public readonly CachedValue<IEnumerable<ItemCount>> productionRates;
		public readonly CachedValue<IEnumerable<ItemCount>> consumptionRates;
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
			normalIngredientConnections.Invalidate();
			consumptionRates.Invalidate();
		}

		public void AddProductConnection(Connection connection)
		{
			products.AddConnection(connection);
			hasNormalProductConnections.InvalidateIf(false);
			productionRates.Invalidate();
		}

		public void RemoveIngredientConnection(Connection connection)
		{
			ingredients.RemoveConnection(connection);
			normalIngredientConnections.Invalidate();
			consumptionRates.Invalidate();
		}

		public void RemoveProductConnection(Connection connection)
		{
			products.RemoveConnection(connection);
			hasNormalProductConnections.InvalidateIf(true);
			productionRates.Invalidate();
		}

		public Step(Recipe recipe, Step relatedStep, Item item, bool isProductOfRelated) : this(recipe)
		{
			if (isProductOfRelated)
			{
				if (relatedStep.HasProductConnectionFor(item))
				{
					relatedStep.GetProductConnection(item).AddConsumer(this);
				}
				else
				{
					new Connection(item).AddProducer(relatedStep).AddConsumer(this);
				}
			}
			else
			{
				if (relatedStep.HasIngredientConnectionFor(item))
				{
					relatedStep.GetIngredientConnection(item).AddProducer(this);
				}
				else
				{
					new Connection(item).AddConsumer(relatedStep).AddProducer(this);
				}
			}
		}

		public Step(Recipe recipe) : base(recipe)
		{
			_control = default;
			hasNormalProductConnections = new CachedValue<bool>(() =>
			{
				foreach (Connection connection in products.Connections)
				{
					if (connection.Type == ConnectionType.SINGLE)
					{
						return true;
					}
				}
				return false;
			});
			normalIngredientConnections = new CachedValue<IImmutableSet<Connection>>(() =>
			{
				HashSet<Connection> normalIngredients = new HashSet<Connection>();
				foreach (Connection connection in ingredients.Connections)
				{
					if (connection.Type == ConnectionType.SINGLE)
					{
						normalIngredients.Add(connection);
					}
				}
				return ImmutableHashSet.CreateRange(normalIngredients);
			});
			productionRates = new CachedValue<IEnumerable<ItemCount>>(() =>
			{
				HashSet<ItemCount> rates = new HashSet<ItemCount>();
				foreach (Connection connection in products.Connections)
				{
					rates.Add(new ItemCount(connection.item, GetRate(connection.item, true)));
				}
				return rates;
			});
			consumptionRates = new CachedValue<IEnumerable<ItemCount>>(() =>
			{
				HashSet<ItemCount> rates = new HashSet<ItemCount>();
				foreach (Connection connection in ingredients.Connections)
				{
					rates.Add(new ItemCount(connection.item, GetRate(connection.item, false)));
				}
				return rates;
			});
		}

		public Step(Recipe recipe, uint machineCount, uint clockSpeedThousandths) : this(recipe)
		{
			MachineCount = machineCount;
			ClockSpeedDecimal = clockSpeedThousandths;
		}

		// return (Multiplier * RationalNumber.Pow(Constants.CLOCK_DECIMALS + 2) / CalculateMachineCount()).Ceiling() / RationalNumber.Pow(Constants.CLOCK_DECIMALS);

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
		private RationalNumber CalculateDefaultItemRate(Item item, bool isItemProduct)
		{
			return 60 / recipe.time * recipe.GetCount(item, isItemProduct);
		}

		/// <summary>
		/// Always positive
		/// </summary>
		public override RationalNumber GetRate(Item item, bool isItemProduct)
		{
			return CalculateDefaultItemRate(item, isItemProduct) * (ClockSpeedDecimal / (RationalNumber)Constants.CLOCK_SPEED_DECIMAL_FACTOR) * MachineCount;
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
			}
			foreach (Connection connection in products.Connections)
			{
				connection.RemoveProducer(this);
			}
			plan.steps.Remove(this);
			plan.processedPlan.Invalidate();
		}

		public double GetPowerDraw()
		{
			Building building = recipe.building;
			return building.powerConsumption.ToDouble() * Math.Pow(ClockSpeedDecimal / (double)Constants.CLOCK_SPEED_DECIMAL_FACTOR, building.powerConsumptionExponent.ToDouble()) * MachineCount;
		}

		protected override void UpdateRatesFrom(Dictionary<ItemCount, bool> rates)
		{
			uint newMachineCount = 0;
			foreach (KeyValuePair<ItemCount<Item>, bool> entry in rates)
			{
				uint potentialMachineCount = (uint)Math.Ceiling((entry.Key.rate / CalculateDefaultItemRate(entry.Key.item, entry.Value)).ToDecimalT());
				newMachineCount = Math.Max(newMachineCount, potentialMachineCount);
			}
			MachineCount = newMachineCount;
			ClockSpeedDecimal = Constants.CLOCK_SPEED_DECIMAL_FACTOR;
			uint newClockSpeedThousandths = 0;
			foreach (KeyValuePair<ItemCount<Item>, bool> entry in rates)
			{
				uint potentialClockSpeedThousandths = (uint)Math.Ceiling((entry.Key.rate / GetRate(entry.Key.item, entry.Value) * Constants.CLOCK_SPEED_DECIMAL_FACTOR).ToDecimalT());
				newClockSpeedThousandths = Math.Max(newClockSpeedThousandths, potentialClockSpeedThousandths);
			}
			ClockSpeedDecimal = newClockSpeedThousandths;
		}

		public void SetMachineCount(uint machineCount)
		{
			MachineCount = machineCount;
		}

		public void SetClockSpeedThousandths(ushort clockSpeedThousandths)
		{
			ClockSpeedDecimal = clockSpeedThousandths;
		}
	}
}
