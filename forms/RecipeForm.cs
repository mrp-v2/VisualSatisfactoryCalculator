using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisualSatisfactoryCalculator.code;

namespace VisualSatisfactoryCalculator.forms
{
	public partial class RecipeForm : Form, IReceivesItemCount
	{
		private readonly IReceivesRecipe parentForm;
		private readonly List<ItemCount> products;
		private readonly List<ItemCount> ingredients;
		private readonly string purpose;

		private const string ingredientPurpose = "ingredient";
		private const string productPurpose = "product";

		public RecipeForm(IReceivesRecipe parentForm, string purpose = null)
		{
			InitializeComponent();
			this.parentForm = parentForm;
			this.purpose = purpose;
			products = new List<ItemCount>();
			ingredients = new List<ItemCount>();
			MachineNameCombo.BeginUpdate();
			foreach (string str in SuggestionsController.SC.GetMachines())
			{
				MachineNameCombo.Items.Add(str);
			}
			MachineNameCombo.EndUpdate();
		}

		private void AddIngredientButton_Click(object sender, EventArgs e)
		{
			new ItemAndCountForm(this, ingredientPurpose).ShowDialog();
		}

		private void AddProductButton_Click(object sender, EventArgs e)
		{
			new ItemAndCountForm(this, productPurpose).ShowDialog();
		}

		private void RemoveProductButton_Click(object sender, EventArgs e)
		{
			try
			{
				if (ProductsList.SelectedItem is string)
				{
					products.Remove(ItemCount.FromString(ProductsList.SelectedItem as string));
					UpdateProductsListVisual();
				}
			}
			catch
			{

			}
		}

		private void RemoveIngredientButton_Click(object sender, EventArgs e)
		{
			try
			{
				if (IngredientsList.SelectedItem is string)
				{
					ingredients.Remove(ItemCount.FromString(IngredientsList.SelectedItem as string));
					UpdateIngredientsListVisual();
				}
			}
			catch
			{

			}
		}

		private void NoButton_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void YesButton_Click(object sender, EventArgs e)
		{
			Recipe rec = GetRecipe();
			if (rec != null)
			{
				parentForm.AddRecipe(rec, purpose);
				SuggestionsController.SC.AddMachine(MachineNameCombo.Text);
				Close();
			}
		}

		public void AddItemCount(ItemCount itemCount, string purpose)
		{
			switch (purpose)
			{
				case productPurpose:
					products.Add(itemCount);
					UpdateProductsListVisual();
					break;
				case ingredientPurpose:
					ingredients.Add(itemCount);
					UpdateIngredientsListVisual();
					break;
			}
		}

		protected List<ItemCount> GetFullCount()
		{
			return products.Merge(ingredients.Inverse());
		}

		protected Recipe GetRecipe()
		{
			return new Recipe(GetFullCount(), (int)CraftTimeNumeric.Value, MachineNameCombo.Text);
		}

		protected void UpdateProductsListVisual()
		{
			ProductsList.BeginUpdate();
			ProductsList.Items.Clear();
			foreach (ItemCount ic in products)
			{
				ProductsList.Items.Add(ic.ToString());
			}
			ProductsList.EndUpdate();
		}

		protected void UpdateIngredientsListVisual()
		{
			IngredientsList.BeginUpdate();
			IngredientsList.Items.Clear();
			foreach (ItemCount ic in ingredients)
			{
				IngredientsList.Items.Add(ic.ToString());
			}
			IngredientsList.EndUpdate();
		}
	}
}
