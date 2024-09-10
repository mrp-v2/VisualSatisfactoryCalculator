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
using Connection = VisualSatisfactoryCalculator.model.production.Connection<VisualSatisfactoryCalculator.satisfactory.JSONClasses.JSONItem, VisualSatisfactoryCalculator.satisfactory.DataStorage.BasicRecipe>;
using ItemCount = VisualSatisfactoryCalculator.model.production.ItemCount<VisualSatisfactoryCalculator.satisfactory.JSONClasses.JSONItem>;

namespace VisualSatisfactoryCalculator.satisfactory.Production
{
	public class Step : AbstractStep<JSONItem, BasicRecipe>
	{
		public readonly CachedValue<bool> hasNormalProductConnections;
		public readonly CachedValue<IImmutableSet<Connection>> normalIngredientConnections;
		public readonly CachedValue<IEnumerable<ItemCount>> productionRates;
		public readonly CachedValue<IEnumerable<ItemCount>> consumptionRates;
		public uint MachineCount { get; private set; }
		public ushort ClockSpeedThousandths { get; private set; }
		private StepControl _control;

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
					rates.Add(new ItemRate(connection.item, GetItemRate(connection.item, true)));
				}
				return rates;
			});
			consumptionRates = new CachedValue<IEnumerable<ItemCount>>(() =>
			{
				HashSet<ItemCount> rates = new HashSet<ItemCount>();
				foreach (Connection connection in ingredients.Connections)
				{
					rates.Add(new ItemRate(connection.item, GetItemRate(connection.item, false)));
				}
				return rates;
			});
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
		/// Always positive.
		/// Calculates the multiplier required to produce/consume the specified rate.
		/// </summary>
		public RationalNumber CalculateMultiplierForRate(JSONItem item, RationalNumber rate, bool isItemProduct)
		{
			return (rate / CalculateDefaultItemRate(item, isItemProduct)).AbsoluteValue().Ceiling(3);
		}

		/// <summary>
		/// Always positive
		/// </summary>
		public RationalNumber GetItemRate(JSONItem item, bool isItemProduct)
		{
			if (isItemProduct)
			{
				return products.GetRate(item).rate;
			}
			else
			{
				return ingredients.GetRate(item).rate;
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
			}
			foreach (Connection connection in products.Connections)
			{
				connection.RemoveProducer(this);
			}
			plan.steps.Remove(this);
			plan.processedPlan.Invalidate();
		}

		public double GetPowerDraw(Encodings encodings)
		{
			IBuilding building = encodings[recipe.MachineUID] as IBuilding;
			return building.PowerConsumption.ToDouble() * Math.Pow(ClockSpeedThousandths / 1000d, building.PowerConsumptionExponent.ToDouble()) * MachineCount;
		}

		protected override void UpdateRatesFrom(ItemCount rate, bool isProduct)
		{
			throw new NotImplementedException();
		}

		protected override void UpdateRatesFrom(Dictionary<ItemCount, bool> rates)
		{
			throw new NotImplementedException();
		}
	}
}
