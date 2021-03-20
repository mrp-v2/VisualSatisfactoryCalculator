using System;
using System.Collections.Generic;
using System.Linq;

using VisualSatisfactoryCalculator.code.Interfaces;
using VisualSatisfactoryCalculator.code.Utility;
using VisualSatisfactoryCalculator.controls.user;

namespace VisualSatisfactoryCalculator.code.DataStorage
{
	public class Step : IEquatable<Step>
	{
		private decimal _multiplier;
		public List<Connection> Connections { get; }
		private ProductionStepControl _control;

		private readonly IRecipe _recipe;

		public Step(IRecipe recipe, Step relatedStep, string itemUID, bool isProductOfRelated) : this(recipe, 1m)
		{
			Connections = new List<Connection>();
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

		public Step(IRecipe recipe, decimal multiplier)
		{
			Connections = new List<Connection>();
			_recipe = recipe;
			_multiplier = multiplier;
			_control = default;
		}

		public IRecipe GetRecipe()
		{
			return _recipe;
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
			if (_control != default(ProductionStepControl))
			{
				_control.ToggleInput(false);
				_control.MultiplierChanged();
				_control.ToggleInput(true);
			}
		}

		private decimal CalculateDefaultItemRate(string itemUID, bool isItemProduct)
		{
			return 60m / _recipe.CraftTime * _recipe.GetCountFor(itemUID, isItemProduct);
		}

		public decimal CalculateMultiplierForRate(string itemUID, decimal rate, bool isItemProduct)
		{
			return Math.Abs(rate / CalculateDefaultItemRate(itemUID, isItemProduct));
		}

		public decimal GetItemRate(string itemUID, bool isItemProduct)
		{
			return CalculateDefaultItemRate(itemUID, isItemProduct) * _multiplier;
		}

		public void SetControl(ProductionStepControl control)
		{
			_control = control;
		}

		public List<string> GetItemUIDsWithRelatedStep()
		{
			List<string> uids = new List<string>();
			foreach (Connection connection in Connections)
			{
				uids.Add(connection.ItemUID);
			}
			return uids;
		}

		public decimal GetMultiplier()
		{
			return _multiplier;
		}

		public bool Equals(Step obj)
		{
			return _recipe.Equals(obj._recipe);
		}

		public override int GetHashCode()
		{
			return _recipe.GetHashCode();
		}

		public void Delete()
		{
			foreach (Connection connection in Connections)
			{
				connection.Delete();
			}
		}

		public decimal GetPowerDraw(Encodings encodings)
		{
			return (encodings[_recipe.MachineUID] as IBuilding).PowerConsumption * (decimal)Math.Pow((double)(CalculateMachineClockPercentage() / 100m), (double)(encodings[_recipe.MachineUID] as IBuilding).PowerConsumptionExponent) * CalculateMachineCount();
		}

		public bool IsStepRelated(Step test)
		{
			foreach (Connection connection in Connections)
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
