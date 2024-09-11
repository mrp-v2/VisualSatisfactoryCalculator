using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VisualSatisfactoryCalculator.satisfactory.Utility;

namespace VisualSatisfactoryCalculator.model.production
{
	public abstract class AbstractStep<ItemType, StepType, RecipeType> where ItemType : BasicItem where StepType : AbstractStep<ItemType, StepType, RecipeType>
	{
		public readonly RecipeType recipe;
		protected readonly ItemRateAndConnectionCollection<ItemType, StepType, RecipeType> products;
		protected readonly ItemRateAndConnectionCollection<ItemType, StepType, RecipeType> ingredients;

		private readonly CachedValue<IEnumerable<Connection<ItemType, StepType, RecipeType>>> _connections;

		public IEnumerable<Connection<ItemType, StepType, RecipeType>> Connections
		{
			get
			{
				return _connections.Get();
			}
		}

		protected AbstractStep(RecipeType recipe)
		{
			this.recipe = recipe;
			products = new ItemRateAndConnectionCollection<ItemType, StepType, RecipeType>();
			ingredients = new ItemRateAndConnectionCollection<ItemType, StepType, RecipeType>();

			_connections = new CachedValue<IEnumerable<Connection<ItemType, StepType, RecipeType>>>(() =>
			{
				return new HashSet<Connection<ItemType, StepType, RecipeType>>(Enumerable.Concat(products.Connections, ingredients.Connections));
			});

			products.SetConnectionsChangedListener(_connections.Invalidate);
			ingredients.SetConnectionsChangedListener(_connections.Invalidate);
		}

		public ItemCount<ItemType> GetRate(ItemType item, bool isProduct)
		{
			if (isProduct)
			{
				return products.GetRate(item);
			}
			else
			{
				return ingredients.GetRate(item);
			}
		}

		protected virtual void UpdateRatesFrom(ItemCount<ItemType> rate, bool isProduct)
		{
			UpdateRatesFrom(new Dictionary<ItemCount<ItemType>, bool> { { rate, isProduct } });
		}

		/// <summary>
		/// Updates the rates using the given rates.
		/// Should throw an error if the given rates have a conflict.
		/// Used during cascading updates. See <see cref="BreadthFirstSearchHandler{ItemType, RecipeType}"/>.
		/// </summary>
		/// <param name="rates">The rates to consider, mapped to if they are a product</param>
		protected abstract void UpdateRatesFrom(Dictionary<ItemCount<ItemType>, bool> rates);

		/// <summary>
		/// Used during cascading updates. See <see cref="BreadthFirstSearchHandler{ItemType, RecipeType}"/>.
		/// </summary>
		/// <param name="visited"></param>
		public void UpdateRatesFrom(HashSet<object> visited)
		{
			/// <summary>
			/// Tracks relevant rates, and if they are a product
			/// </summary>
			Dictionary<ItemCount<ItemType>, bool> relevantRates = new Dictionary<ItemCount<ItemType>, bool>();
			foreach (Connection<ItemType, StepType, RecipeType> connection in products.Connections)
			{
				if (visited.Contains(connection))
				{
					relevantRates.Add(connection.GetRate(This, false), true);
				}
			}
			foreach (Connection<ItemType, StepType, RecipeType> connection in ingredients.Connections)
			{
				if (visited.Contains(connection))
				{
					relevantRates.Add(connection.GetRate(This, true), false);
				}
			}
			UpdateRatesFrom(relevantRates);
		}

		/// <summary>
		/// Update this steps rates from a given rate, then cascade updates to connected steps.
		/// </summary>
		/// <param name="rate"></param>
		/// <param name="isProduct"></param>
		public void CascadingUpdateRatesFrom(ItemCount<ItemType> rate, bool isProduct)
		{
			UpdateRatesFrom(rate, isProduct);
			BreadthFirstSearchHandler<ItemType, StepType, RecipeType>.CascadeUpdates(This);
		}

		protected abstract StepType This { get; }
	}
}
