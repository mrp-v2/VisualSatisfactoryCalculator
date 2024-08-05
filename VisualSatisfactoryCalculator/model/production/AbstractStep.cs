using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VisualSatisfactoryCalculator.code.Utility;

namespace VisualSatisfactoryCalculator.model.production
{
	public abstract class AbstractStep<ItemType> where ItemType : AbstractItem
	{
		protected readonly Recipe<ItemType> recipe;
		protected readonly ItemRateAndConnectionCollection<ItemType> products;
		protected readonly ItemRateAndConnectionCollection<ItemType> ingredients;

		private readonly CachedValue<IEnumerable<Connection<ItemType>>> connections;

		public IEnumerable<Connection<ItemType>> Connections
		{
			get
			{
				return connections.Get();
			}
		}

		protected AbstractStep(Recipe<ItemType> recipe)
		{
			this.recipe = recipe;
			products = new ItemRateAndConnectionCollection<ItemType>();
			ingredients = new ItemRateAndConnectionCollection<ItemType>();

			connections = new CachedValue<IEnumerable<Connection<ItemType>>>(() =>
			{
				return new HashSet<Connection<ItemType>>(Enumerable.Concat(products.Connections, ingredients.Connections));
			});
		}

		public ItemRate<ItemType> GetRate(ItemType item, bool isProduct)
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

		protected abstract void UpdateRatesFrom(ItemRate<ItemType> rate, bool isProduct);

		/// <summary>
		/// Updates the rates using the given rates.
		/// Should throw an error if the given rates have a conflict.
		/// </summary>
		/// <param name="rates">The rates to consider, mapped to if they are a product</param>
		protected abstract void UpdateRatesFrom(Dictionary<ItemRate<ItemType>, bool> rates);

		public void UpdateRatesFrom(HashSet<object> visited)
		{
			/// <summary>
			/// Tracks relevant rates, and if they are a product
			/// </summary>
			Dictionary<ItemRate<ItemType>, bool> relevantRates = new Dictionary<ItemRate<ItemType>, bool>();
			foreach (Connection<ItemType> connection in products.Connections)
			{
				if (visited.Contains(connection))
				{
					relevantRates.Add(connection.GetRate(this, false), true);
				}
			}
			foreach (Connection<ItemType> connection in ingredients.Connections)
			{
				if (visited.Contains(connection))
				{
					relevantRates.Add(connection.GetRate(this, true), false);
				}
			}
			UpdateRatesFrom(relevantRates);
		}

		public void CascadingUpdateRatesFrom(ItemRate<ItemType> rate, bool isProduct)
		{
			UpdateRatesFrom(rate, isProduct);
			CascadingUpdateHandler<ItemType>.CascadeUpdates(this);
		}
	}
}
