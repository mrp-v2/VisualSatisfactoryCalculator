using System.Collections.Generic;
using System.Linq;

using VisualSatisfactoryCalculator.model.util;
using VisualSatisfactoryCalculator.satisfactory.Utility;
using VisualSatisfactoryCalculator.util.collections;

namespace VisualSatisfactoryCalculator.model.production
{
	/// <summary>
	/// The base type for steps.
	/// Implementations should keep track of their item rates somehow.
	/// </summary>
	public abstract class AbstractStep<ItemType, StepType, RecipeType> where ItemType : BasicItem where StepType : AbstractStep<ItemType, StepType, RecipeType>
	{
		public readonly RecipeType recipe;
		protected readonly ConnectionCollection<ItemType, StepType, RecipeType> products;
		protected readonly ConnectionCollection<ItemType, StepType, RecipeType> ingredients;

		private readonly CachedValue<IEnumerable<Connection<ItemType, StepType, RecipeType>>>.Managed _connections;

		private readonly CachedValue<bool>.Versioned _hasSingleConnectionProduct;
		private readonly CachedValue<IReadOnlySet<Connection<ItemType, StepType, RecipeType>>>.Versioned _singleConnectionIngredients;

		public IEnumerable<Connection<ItemType, StepType, RecipeType>> Connections
		{
			get
			{
				return _connections.Get();
			}
		}

		public bool HasSingleConnectionProduct
		{
			get
			{
				return _hasSingleConnectionProduct.Get();
			}
		}

		public IReadOnlySet<Connection<ItemType, StepType, RecipeType>> SingleConnectionIngredients
		{
			get
			{
				return _singleConnectionIngredients.Get();
			}
		}

		protected AbstractStep(Mutable<int> versionSource, RecipeType recipe)
		{
			this.recipe = recipe;
			products = new ConnectionCollection<ItemType, StepType, RecipeType>();
			ingredients = new ConnectionCollection<ItemType, StepType, RecipeType>();

			_connections = new CachedValue<IEnumerable<Connection<ItemType, StepType, RecipeType>>>.Managed(() =>
			{
				return new HashSet<Connection<ItemType, StepType, RecipeType>>(Enumerable.Concat(products.Connections, ingredients.Connections));
			});

			_hasSingleConnectionProduct = new CachedValue<bool>.Versioned(versionSource, () =>
			{
				foreach (Connection<ItemType, StepType, RecipeType> connection in products.Connections)
				{
					if (connection.Type == ConnectionType.SINGLE)
					{
						return true;
					}
				}
				return false;
			});
			_singleConnectionIngredients = new CachedValue<IReadOnlySet<Connection<ItemType, StepType, RecipeType>>>.Versioned(versionSource, () =>
			{
				ViewableSet<Connection<ItemType, StepType, RecipeType>> singleConnections = new ViewableSet<Connection<ItemType, StepType, RecipeType>>();
				foreach (Connection<ItemType, StepType, RecipeType> connection in ingredients.Connections)
				{
					if (connection.Type == ConnectionType.SINGLE)
					{
						singleConnections.Add(connection);
					}
				}
				return singleConnections.ReadOnly;
			});

			products.SetConnectionsChangedListener(_connections.Invalidate);
			ingredients.SetConnectionsChangedListener(_connections.Invalidate);
		}

		public abstract decimal GetRate(ItemType item, bool isProduct);

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
					relevantRates.Add(connection.GetRate(This, false).ToCount(connection.item), true);
				}
			}
			foreach (Connection<ItemType, StepType, RecipeType> connection in ingredients.Connections)
			{
				if (visited.Contains(connection))
				{
					relevantRates.Add(connection.GetRate(This, true).ToCount(connection.item), false);
				}
			}
			UpdateRatesFrom(relevantRates);
		}

		/// <summary>
		/// Update this steps rates from a given rate, then cascade updates to connected steps.
		/// </summary>
		/// <param name="rate"></param>
		/// <param name="isProduct"></param>
		public void CascadingUpdateRatesFrom(ItemCount<ItemType> rate, bool isProduct, ProcessedPlan<StepType, ItemType, RecipeType> processedPlan)
		{
			UpdateRatesFrom(rate, isProduct);
			BreadthFirstSearchHandler<ItemType, StepType, RecipeType>.CascadeUpdates(This, processedPlan);
		}

		protected abstract StepType This { get; }
	}
}
