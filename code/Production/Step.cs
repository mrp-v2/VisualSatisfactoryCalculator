using System;
using System.Collections.Generic;
using System.Linq;

using VisualSatisfactoryCalculator.code.Interfaces;
using VisualSatisfactoryCalculator.code.Utility;
using VisualSatisfactoryCalculator.controls.user;

namespace VisualSatisfactoryCalculator.code.Production
{
	public class Step
	{
		private decimal _multiplier;
		public readonly CachedValue<IEnumerable<Connection>> Connections;
		public readonly CachedValue<bool> HasNormalProductConnections;
		public readonly CachedValue<HashSet<Connection>> NormalIngredientConnections;
		private readonly HashSet<Connection> IngredientConnections;
		private readonly HashSet<Connection> ProductConnections;
		private StepControl _control;

		public void AddIngredientConnection(Connection connection)
		{
			IngredientConnections.Add(connection);
			Connections.Invalidate();
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
				new Connection(itemUID).AddNormalConsumer(this).AddNormalProducer(relatedStep);
			}
			else
			{
				new Connection(itemUID).AddNormalConsumer(relatedStep).AddNormalProducer(this);
			}
			SetMultiplier(CalculateMultiplierForRate(itemUID, relatedStep.GetItemRate(itemUID, isProductOfRelated), !isProductOfRelated));
		}

		public Step(IRecipe recipe) : this()
		{
			Recipe = recipe;
			_multiplier = 1m;
			_control = default;
		}

		private Step()
		{
			IngredientConnections = new HashSet<Connection>();
			ProductConnections = new HashSet<Connection>();
			Connections = new CachedValue<IEnumerable<Connection>>(() =>
			{
				List<Connection> list = new List<Connection>();
				list.AddRange(IngredientConnections);
				list.AddRange(ProductConnections);
				return list;
			});
			HasNormalProductConnections = new CachedValue<bool>(() =>
			{
				foreach (Connection connection in ProductConnections)
				{
					if (connection.Type == Connection.OverallConnectionType.NORMAL)
					{
						return true;
					}
				}
				return false;
			});
			NormalIngredientConnections = new CachedValue<HashSet<Connection>>(() =>
			{
				HashSet<Connection> normalIngredients = new HashSet<Connection>();
				foreach (Connection connection in IngredientConnections)
				{
					if (connection.Type == Connection.OverallConnectionType.NORMAL)
					{
						normalIngredients.Add(connection);
					}
				}
				return normalIngredients;
			});
		}

		public int CalculateMachineCount()
		{
			return (int)Math.Ceiling(_multiplier);
		}

		public decimal CalculateMachineClockPercentage()
		{
			return Math.Ceiling(_multiplier * (int)Math.Pow(10, Constants.CLOCK_DECIMALS + 2) / CalculateMachineCount()) / (int)Math.Pow(10, Constants.CLOCK_DECIMALS);
		}

		public void AddRelatedStep(Step related, string itemUID, bool isProductOfRelated)
		{
			if (isProductOfRelated)
			{
				new Connection(itemUID).AddNormalConsumer(this).AddNormalProducer(related);
			}
			else
			{
				new Connection(itemUID).AddNormalConsumer(related).AddNormalProducer(this);
			}
		}

		public void SetMultiplier(decimal multiplier)
		{
			_multiplier = multiplier;
			if (_control != default(StepControl))
			{
				_control.UpdateNumerics();
			}
		}

		private decimal CalculateDefaultItemRate(string itemUID, bool isItemProduct)
		{
			return 60m / Recipe.CraftTime * Recipe.GetCountFor(itemUID, isItemProduct);
		}

		public decimal CalculateMultiplierForRate(string itemUID, decimal rate, bool isItemProduct)
		{
			return Math.Abs(rate / CalculateDefaultItemRate(itemUID, isItemProduct));
		}

		public decimal GetItemRate(string itemUID, bool isItemProduct)
		{
			return CalculateDefaultItemRate(itemUID, isItemProduct) * _multiplier;
		}

		public void SetControl(StepControl control)
		{
			_control = control;
		}

		public List<string> GetItemUIDsWithRelatedStep()
		{
			List<string> uids = new List<string>();
			foreach (Connection connection in Connections.Get())
			{
				uids.Add(connection.ItemUID);
			}
			return uids;
		}

		public decimal GetMultiplier()
		{
			return _multiplier;
		}

		public void Delete(Plan plan)
		{
			foreach (Connection connection in Connections.Get())
			{
				connection.Delete(); // TODO this won't work for multi-connections
			}
			plan.Steps.Remove(this);
			plan.ProcessedPlan.Invalidate();
		}

		public decimal GetPowerDraw(Encodings encodings)
		{
			return (encodings[Recipe.MachineUID] as IBuilding).PowerConsumption * (decimal)Math.Pow((double)(CalculateMachineClockPercentage() / 100m), (double)(encodings[Recipe.MachineUID] as IBuilding).PowerConsumptionExponent) * CalculateMachineCount();
		}

		public bool IsStepRelated(Step test)
		{
			foreach (Connection connection in Connections.Get())
			{
				if (connection.ContainsStep(test))
				{
					return true;
				}
			}
			return false;
		}
	}
}
