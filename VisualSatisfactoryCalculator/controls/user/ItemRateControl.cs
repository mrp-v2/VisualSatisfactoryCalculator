using System;
using System.Drawing;
using System.Windows.Forms;

using VisualSatisfactoryCalculator.forms;
using VisualSatisfactoryCalculator.satisfactory.model.production;
using VisualSatisfactoryCalculator.satisfactory.Utility;

namespace VisualSatisfactoryCalculator.controls.user
{
	public partial class ItemRateControl : UserControl
	{
		public delegate void RateChanged(Item itemUID, decimal newRate, bool isProduct);
		public delegate void ItemClicked(Item itemUID, bool isProduct);

		public Item Item { get; }
		private bool _initialized;
		public bool IsProduct { get; }
		private readonly MainForm _mainForm;
		private readonly RateChanged _rateChanged;
		private readonly ItemClicked _itemClicked;
		private readonly int _panelDepth;

		public Point GetTotalLocation()
		{
			return PlanLayoutMaker.AddParentPoints(this, _panelDepth);
		}

		public ItemRateControl(MainForm mainForm, Item item, decimal rate, bool isProduct, int panelDepth, RateChanged rateChanged, ItemClicked itemClicked)
		{
			_initialized = false;
			InitializeComponent();
			NumberControl.Maximum = decimal.MaxValue;
			_mainForm = mainForm;
			_rateChanged = rateChanged;
			_itemClicked = itemClicked;
			_panelDepth = panelDepth;
			Item = item;
			IsProduct = isProduct;
			ItemButton.Text = item.displayName;
			UpdateRateValue(rate);
			NumberControl.ValueChanged += NumberChanged;
		}

		private void NumberChanged(object sender, EventArgs args)
		{
			if (Enabled && _initialized)
			{
				_mainForm.SuspendDrawing();
				_rateChanged(Item, NumberControl.Value * Item.countDisplayFactor, IsProduct);
				_mainForm.UpdateTotalView();
				_mainForm.ResumeDrawing();
			}
		}

		private void ItemButton_Click(object sender, EventArgs e)
		{
			_itemClicked(Item, IsProduct);
		}

		public void UpdateRateValue(decimal newRate)
		{
			NumberControl.Value = newRate / Item.countDisplayFactor;
		}

		public void ToggleInput(bool on)
		{
			if (NumberControl.Value != 0 && Enabled)
			{
				Enabled = false;
				return;
			}
			Enabled = on;
		}

		public void FinishInitialization()
		{
			_initialized = true;
		}
	}
}
