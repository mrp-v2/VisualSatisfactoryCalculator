using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VisualSatisfactoryCalculator.satisfactory.Utility;

namespace VisualSatisfactoryCalculator.model.production
{
	public abstract class AbstractStep<ItemType, RecipeType> where ItemType : BasicItem
	{
		public readonly RecipeType recipe;
		protected readonly ItemRateAndConnectionCollection<ItemType, RecipeType> products;
		protected readonly ItemRateAndConnectionCollection<ItemType, RecipeType> ingredients;

		private readonly CachedValue<IEnumerable<Connection<ItemType, RecipeType>>> _connections;

		public IEnumerable<Connection<ItemType, RecipeType>> Connections
		{
			get
			{
				return _connections.Get();
			}
		}

		protected AbstractStep(RecipeType recipe)
		{
			this.recipe = recipe;
			products = new ItemRateAndConnectionCollection<ItemType, RecipeType>();
			ingredients = new ItemRateAndConnectionCollection<ItemType, RecipeType>();

			_connections = new CachedValue<IEnumerable<Connection<ItemType, RecipeType>>>(() =>
			{
				return new HashSet<Connection<ItemType, RecipeType>>(Enumerable.Concat(products.Connections, ingredients.Connections));
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

		protected abstract void UpdateRatesFrom(ItemCount<ItemType> rate, bool isProduct);

		/// <summary>
		/// Updates the rates using the given rates.
		/// Should throw an error if the given rates have a conflict.
		/// </summary>
		/// <param name="rates">The rates to consider, mapped to if they are a product</param>
		protected abstract void UpdateRatesFrom(Dictionary<ItemCount<ItemType>, bool> rates);

		public void UpdateRatesFrom(HashSet<object> visited)
		{
			/// <summary>
			/// Tracks relevant rates, and if they are a product
			/// </summary>
			Dictionary<ItemCount<ItemType>, bool> relevantRates = new Dictionary<ItemCount<ItemType>, bool>();
			foreach (Connection<ItemType, RecipeType> connection in products.Connections)
			{
				if (visited.Contains(connection))
				{
					relevantRates.Add(connection.GetRate(this, false), true);
				}
			}
			foreach (Connection<ItemType, RecipeType> connection in ingredients.Connections)
			{
				if (visited.Contains(connection))
				{
					relevantRates.Add(connection.GetRate(this, true), false);
				}
			}
			UpdateRatesFrom(relevantRates);
		}

		public void CascadingUpdateRatesFrom(ItemCount<ItemType> rate, bool isProduct)
		{
			UpdateRatesFrom(rate, isProduct);
			BreadthFirstSearchHandler<ItemType, RecipeType>.CascadeUpdates(this);
		}
	}
}
