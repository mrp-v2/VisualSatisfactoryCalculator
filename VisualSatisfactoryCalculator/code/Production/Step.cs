﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using VisualSatisfactoryCalculator.code.Extensions;
using VisualSatisfactoryCalculator.code.Interfaces;
using VisualSatisfactoryCalculator.code.Numbers;
using VisualSatisfactoryCalculator.code.Utility;
using VisualSatisfactoryCalculator.controls.user;

namespace VisualSatisfactoryCalculator.code.Production
{
	public class Step
	{
		public RationalNumber Multiplier { get; private set; }
		public readonly CachedValue<IImmutableSet<Connection>> Connections;
		public readonly CachedValue<bool> HasNormalProductConnections;
		public readonly CachedValue<IImmutableSet<Connection>> NormalIngredientConnections;
		private readonly HashSet<Connection> IngredientConnections;
		private readonly HashSet<Connection> ProductConnections;
		private StepControl _control;

		public IEnumerable<Connection> GetIngredientConnections()
		{
			return IngredientConnections;
		}

		public IEnumerable<Connection> GetProductConnections()
		{
			return ProductConnections;
		}

		public bool HasProductConnectionFor(string itemUID)
		{
			foreach (Connection connection in ProductConnections)
			{
				if (connection.ItemID == itemUID)
				{
					return true;
				}
			}
			return false;
		}

		public bool HasIngredientConnectionFor(string itemUID)
		{
			foreach (Connection connection in IngredientConnections)
			{
				if (connection.ItemID == itemUID)
				{
					return true;
				}
			}
			return false;
		}

		public Connection GetProductConnection(string itemUID)
		{
			foreach (Connection connection in ProductConnections)
			{
				if (connection.ItemID == itemUID)
				{
					return connection;
				}
			}
			throw new ArgumentException("This step has no product connection for item '" + itemUID + "'");
		}

		public Connection GetIngredientConnection(string itemUID)
		{
			foreach (Connection connection in IngredientConnections)
			{
				if (connection.ItemID == itemUID)
				{
					return connection;
				}
			}
			throw new ArgumentException("This step has no ingredient connection for item '" + itemUID + "'");
		}

		public HashSet<Step> GetAllConnectedSteps(Connection exclude)
		{
			return GetAllConnectedStepsRecursive(new HashSet<Step>(), exclude);
		}

		private HashSet<Step> GetAllConnectedStepsRecursive(HashSet<Step> connected, Connection exclude)
		{
			connected.Add(this);
			foreach (Connection connection in Connections.Get())
			{
				if (connection == exclude)
				{
					continue;
				}
				foreach (Step step in connection.ConnectedSteps.Get())
				{
					if (!connected.Contains(step))
					{
						step.GetAllConnectedStepsRecursive(connected, exclude);
					}
				}
			}
			return connected;
		}

		public HashSet<Step> GetAllNormallyConnectedSteps(Connection exclude)
		{
			return GetAllNormallyConnectedStepsRecursive(new HashSet<Step>(), exclude);
		}

		private HashSet<Step> GetAllNormallyConnectedStepsRecursive(HashSet<Step> connected, Connection exclude)
		{
			connected.Add(this);
			foreach (Connection connection in Connections.Get())
			{
				if (connection == exclude)
				{
					continue;
				}
				if (connection.Type.Get() == Connection.ConnectionType.NORMAL)
				{
					foreach (Step step in connection.ConnectedSteps.Get())
					{
						if (!connected.Contains(step))
						{
							step.GetAllNormallyConnectedStepsRecursive(connected, exclude);
						}
					}
				}
			}
			return connected;
		}

		public void AddIngredientConnection(Connection connection)
		{
			IngredientConnections.Add(connection);
			Connections.Invalidate();
			NormalIngredientConnections.Invalidate();
		}

		public void AddProductConnection(Connection connection)
		{
			ProductConnections.Add(connection);
			Connections.Invalidate();
			HasNormalProductConnections.InvalidateIf(false);
		}

		public void RemoveIngredientConnection(Connection connection)
		{
			IngredientConnections.Remove(connection);
			Connections.Invalidate();
			NormalIngredientConnections.Invalidate();
		}

		public void RemoveProductConnection(Connection connection)
		{
			ProductConnections.Remove(connection);
			Connections.Invalidate();
			HasNormalProductConnections.InvalidateIf(true);
		}

		public readonly IRecipe Recipe;

		public Step(IRecipe recipe, Step relatedStep, string itemUID, bool isProductOfRelated) : this(recipe)
		{
			if (isProductOfRelated)
			{
				if (relatedStep.HasProductConnectionFor(itemUID))
				{
					relatedStep.GetProductConnection(itemUID).AddConsumer(this);
				}
				else
				{
					Multiplier = CalculateMultiplierForRate(itemUID, relatedStep.GetItemRate(itemUID, isProductOfRelated), !isProductOfRelated);
					new Connection(itemUID).AddProducer(relatedStep).AddConsumer(this);
				}
			}
			else
			{
				if (relatedStep.HasIngredientConnectionFor(itemUID))
				{
					relatedStep.GetIngredientConnection(itemUID).AddProducer(this);
				}
				else
				{
					Multiplier = CalculateMultiplierForRate(itemUID, relatedStep.GetItemRate(itemUID, isProductOfRelated), !isProductOfRelated);
					new Connection(itemUID).AddConsumer(relatedStep).AddProducer(this);
				}
			}
		}

		public Step(IRecipe recipe) : this()
		{
			Recipe = recipe;
			Multiplier = 1;
			_control = default;
		}

		private Step()
		{
			IngredientConnections = new HashSet<Connection>();
			ProductConnections = new HashSet<Connection>();
			Connections = new CachedValue<IImmutableSet<Connection>>(() =>
			{
				HashSet<Connection> connections = new HashSet<Connection>();
				connections.AddRange(IngredientConnections);
				connections.AddRange(ProductConnections);
				return ImmutableHashSet.CreateRange(connections);
			});
			HasNormalProductConnections = new CachedValue<bool>(() =>
			{
				foreach (Connection connection in ProductConnections)
				{
					if (connection.Type.Get() == Connection.ConnectionType.NORMAL)
					{
						return true;
					}
				}
				return false;
			});
			NormalIngredientConnections = new CachedValue<IImmutableSet<Connection>>(() =>
			{
				HashSet<Connection> normalIngredients = new HashSet<Connection>();
				foreach (Connection connection in IngredientConnections)
				{
					if (connection.Type.Get() == Connection.ConnectionType.NORMAL)
					{
						normalIngredients.Add(connection);
					}
				}
				return ImmutableHashSet.CreateRange(normalIngredients);
			});
		}

		public int CalculateMachineCount()
		{
			return Multiplier.Ceiling();
		}

		public RationalNumber CalculateMachineClockPercentage()
		{
			return (Multiplier * RationalNumber.Pow(Constants.CLOCK_DECIMALS + 2) / CalculateMachineCount()).Ceiling() / RationalNumber.Pow(Constants.CLOCK_DECIMALS);
		}

		public void AddRelatedStep(Step related, string itemUID, bool isProductOfRelated)
		{
			if (isProductOfRelated)
			{
				new Connection(itemUID).AddConsumer(this).AddProducer(related);
			}
			else
			{
				new Connection(itemUID).AddConsumer(related).AddProducer(this);
			}
		}

		public void SetMultiplier(RationalNumber multiplier, HashSet<Connection> excludedConnections, HashSet<Step> updated)
		{
			SetMultiplier(multiplier, false);
			updated.Add(this);
			HashSet<Connection> nextRound = new HashSet<Connection>();
			foreach (Connection connection in Connections.Get())
			{
				if (!excludedConnections.Contains(connection))
				{
					nextRound.Add(connection);
				}
			}
			foreach (Connection connection in nextRound)
			{
				connection.UpdateMultipliers(updated, this, excludedConnections);
			}
		}

		public void SetMultiplier(RationalNumber multiplier, HashSet<Connection> excludedConnections)
		{
			SetMultiplier(multiplier, excludedConnections, new HashSet<Step>());
		}

		public void SetMultiplier(RationalNumber multiplier)
		{
			SetMultiplier(multiplier, new HashSet<Connection>());
		}

		public void SetMultiplier(RationalNumber multiplier, bool causeCascadingUpdates)
		{
			if (causeCascadingUpdates)
			{
				SetMultiplier(multiplier);
			}
			else
			{
				Multiplier = multiplier;
				if (_control != default(StepControl))
				{
					_control.UpdateNumerics();
				}
			}
		}

		/// <summary>
		/// Always positive
		/// </summary>
		private RationalNumber CalculateDefaultItemRate(string itemUID, bool isItemProduct)
		{
			return 60 / Recipe.CraftTime * Recipe.GetCountFor(itemUID, isItemProduct);
		}

		/// <summary>
		/// Always positive
		/// </summary>
		public RationalNumber CalculateMultiplierForRate(string itemUID, RationalNumber rate, bool isItemProduct)
		{
			return (rate / CalculateDefaultItemRate(itemUID, isItemProduct)).AbsoluteValue();
		}

		/// <summary>
		/// Always positive
		/// </summary>
		public RationalNumber GetItemRate(string itemUID, bool isItemProduct)
		{
			return CalculateDefaultItemRate(itemUID, isItemProduct) * Multiplier;
		}

		public void SetControl(StepControl control)
		{
			_control = control;
		}

		public void Delete(Plan plan)
		{
			foreach (Connection connection in IngredientConnections)
			{
				connection.DeleteConsumer(this);
			}
			foreach (Connection connection in ProductConnections)
			{
				connection.DeleteProducer(this);
			}
			plan.Steps.Remove(this);
			plan.ProcessedPlan.Invalidate();
		}

		public double GetPowerDraw(Encodings encodings)
		{
			IBuilding building = encodings[Recipe.MachineUID] as IBuilding;
			return building.PowerConsumption.ToDouble() * Math.Pow(CalculateMachineClockPercentage().ToDouble() / 100, building.PowerConsumptionExponent.ToDouble()) * CalculateMachineCount();
		}
	}
}
