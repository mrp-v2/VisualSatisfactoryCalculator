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
	public partial class CreateRecipePrompt : Form, IReceives<ItemCount>
	{
		private readonly IReceives<Recipe> parentForm;
		private readonly List<ItemCount> products;
		private readonly List<ItemCount> ingredients;
		private readonly string purpose;

		private const string ingredientPurpose = "ingredient";
		private const string productPurpose = "product";

		public CreateRecipePrompt(IReceives<Recipe> parentForm, string purpose)
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
			new CreateItemAndCountPrompt(this, ingredientPurpose).ShowDialog();
		}

		private void AddProductButton_Click(object sender, EventArgs e)
		{
			new CreateItemAndCountPrompt(this, productPurpose).ShowDialog();
		}

		private void RemoveProductButton_Click(object sender, EventArgs e)
		{
			try
			{
				if (ProductsList.SelectedItem is ItemCount)
				{
					products.Remove(ProductsList.SelectedItem as ItemCount);
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
				if (IngredientsList.SelectedItem is ItemCount)
				{
					ingredients.Remove(IngredientsList.SelectedItem as ItemCount);
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
				parentForm.SendObject(rec, purpose);
				Close();
			}
		}

		public void SendObject(ItemCount itemCount, string purpose)
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
				ProductsList.Items.Add(ic);
			}
			ProductsList.EndUpdate();
		}

		protected void UpdateIngredientsListVisual()
		{
			IngredientsList.BeginUpdate();
			IngredientsList.Items.Clear();
			foreach (ItemCount ic in ingredients)
			{
				IngredientsList.Items.Add(ic);
			}
			IngredientsList.EndUpdate();
		}
	}
}
