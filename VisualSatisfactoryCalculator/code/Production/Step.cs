using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using VisualSatisfactoryCalculator.satisfactory.Extensions;
using VisualSatisfactoryCalculator.satisfactory.Interfaces;
using VisualSatisfactoryCalculator.satisfactory.Numbers;
using VisualSatisfactoryCalculator.satisfactory.Utility;
using VisualSatisfactoryCalculator.controls.user;
using VisualSatisfactoryCalculator.model.production;
using VisualSatisfactoryCalculator.satisfactory.JSONClasses;
using VisualSatisfactoryCalculator.satisfactory.DataStorage;
using Connection = VisualSatisfactoryCalculator.model.production.Connection<VisualSatisfactoryCalculator.satisfactory.JSONClasses.JSONItem, VisualSatisfactoryCalculator.satisfactory.Production.Step, VisualSatisfactoryCalculator.satisfactory.DataStorage.BasicRecipe>;
using ItemCount = VisualSatisfactoryCalculator.model.production.ItemCount<VisualSatisfactoryCalculator.satisfactory.JSONClasses.JSONItem>;

namespace VisualSatisfactoryCalculator.satisfactory.Production
{
	public class Step : AbstractStep<JSONItem, Step, BasicRecipe>
	{
		public readonly CachedValue<bool> hasNormalProductConnections;
		public readonly CachedValue<IImmutableSet<Connection>> normalIngredientConnections;
		public readonly CachedValue<IEnumerable<ItemCount>> productionRates;
		public readonly CachedValue<IEnumerable<ItemCount>> consumptionRates;
		private uint _machineCount;
		private ushort _clockSpeedThousandths;
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
		public ushort ClockSpeedThousandths
		{
			get
			{
				return _clockSpeedThousandths;
			}
			private set
			{
				_clockSpeedThousandths = value;
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

		public bool HasProductConnectionFor(JSONItem item)
		{
			return products.HasConnection(item);
		}

		public bool HasIngredientConnectionFor(JSONItem item)
		{
			return ingredients.HasConnection(item);
		}

		public Connection GetProductConnection(JSONItem item)
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

		public Connection GetIngredientConnection(JSONItem item)
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

		public Step(BasicRecipe recipe, Step relatedStep, JSONItem item, bool isProductOfRelated) : this(recipe)
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

		public Step(BasicRecipe recipe) : base(recipe)
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

		public Step(BasicRecipe recipe, uint machineCount, ushort clockSpeedThousandths) : this(recipe)
		{
			MachineCount = machineCount;
			ClockSpeedThousandths = clockSpeedThousandths;
		}

		// return (Multiplier * RationalNumber.Pow(Constants.CLOCK_DECIMALS + 2) / CalculateMachineCount()).Ceiling() / RationalNumber.Pow(Constants.CLOCK_DECIMALS);

		public void AddRelatedStep(Step related, JSONItem item, bool isProductOfRelated)
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
		private RationalNumber CalculateDefaultItemRate(JSONItem item, bool isItemProduct)
		{
			return 60 / recipe.time * recipe.GetCountFor(item, isItemProduct);
		}

		/// <summary>
		/// Always positive
		/// </summary>
		public override RationalNumber GetRate(JSONItem item, bool isItemProduct)
		{
			return CalculateDefaultItemRate(item, isItemProduct) * (ClockSpeedThousandths / (RationalNumber)1000) * MachineCount;
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

		public double GetPowerDraw(JsonEncodings encodings)
		{
			IBuilding building = encodings[recipe.MachineUID] as IBuilding;
			return building.PowerConsumption.ToDouble() * Math.Pow(ClockSpeedThousandths / 1000d, building.PowerConsumptionExponent.ToDouble()) * MachineCount;
		}

		protected override void UpdateRatesFrom(Dictionary<ItemCount, bool> rates)
		{
			uint newMachineCount = 0;
			foreach (KeyValuePair<ItemCount<JSONItem>, bool> entry in rates)
			{
				uint potentialMachineCount = (uint)Math.Ceiling((entry.Key.rate / CalculateDefaultItemRate(entry.Key.item, entry.Value)).ToDecimalT());
				newMachineCount = Math.Max(newMachineCount, potentialMachineCount);
			}
			MachineCount = newMachineCount;
			ClockSpeedThousandths = 1000;
			ushort newClockSpeedThousandths = 0;
			foreach (KeyValuePair<ItemCount<JSONItem>, bool> entry in rates)
			{
				ushort potentialClockSpeedThousandths = (ushort)Math.Ceiling((entry.Key.rate / GetRate(entry.Key.item, entry.Value) * 1000).ToDecimalT());
				newClockSpeedThousandths = Math.Max(newClockSpeedThousandths, potentialClockSpeedThousandths);
			}
			ClockSpeedThousandths = newClockSpeedThousandths;
		}

		public void SetMachineCount(uint machineCount)
		{
			MachineCount = machineCount;
		}

		public void SetClockSpeedThousandths(ushort clockSpeedThousandths)
		{
			ClockSpeedThousandths = clockSpeedThousandths;
		}
	}
}
