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

		public abstract void UpdateRatesFrom(ItemRate<ItemType> rate, bool isProduct);

		public abstract void UpdateRatesFrom(HashSet<object> visited);

		public void CascadingUpdateRatesFrom(ItemRate<ItemType> rate, bool isProduct)
		{
			UpdateRatesFrom(rate, isProduct);
			CascadingUpdateHandler<ItemType>.CascadeUpdates(this);
		}
	}
}
