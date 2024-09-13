using System;
using System.Drawing;
using System.Windows.Forms;

using VisualSatisfactoryCalculator.satisfactory.Numbers;
using VisualSatisfactoryCalculator.satisfactory.Utility;
using VisualSatisfactoryCalculator.forms;
using VisualSatisfactoryCalculator.satisfactory.JSONClasses;

namespace VisualSatisfactoryCalculator.controls.user
{
	public partial class ItemRateControl : UserControl
	{
		public delegate void RateChanged(JSONItem itemUID, RationalNumber oldRate, RationalNumber newRate, bool isProduct);
		public delegate void ItemClicked(JSONItem itemUID, bool isProduct);

		public JSONItem Item { get; }
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

		public ItemRateControl(MainForm mainForm, JSONItem item, RationalNumber rate, bool isProduct, int panelDepth, RateChanged rateChanged, ItemClicked itemClicked)
		{
			_initialized = false;
			InitializeComponent();
			_mainForm = mainForm;
			_rateChanged = rateChanged;
			_itemClicked = itemClicked;
			_panelDepth = panelDepth;
			Item = item;
			IsProduct = isProduct;
			ItemButton.Text = item.displayName;
			UpdateRateValue(rate);
			NumberControl.AddNumberChangedListener(NumberChanged);
		}

		private void NumberChanged(RationalNumber oldValue, RationalNumber newValue)
		{
			if (Enabled && _initialized)
			{
				_mainForm.SuspendDrawing();
				_rateChanged(Item, oldValue, newValue, IsProduct);
				_mainForm.UpdateTotalView();
				_mainForm.ResumeDrawing();
			}
		}

		private void ItemButton_Click(object sender, EventArgs e)
		{
			_itemClicked(Item, IsProduct);
		}

		public void UpdateRateValue(RationalNumber newRate)
		{
			NumberControl.SetNumber(newRate);
		}

		public void ToggleInput(bool on)
		{
			if (!NumberControl.GetNumber().isNonZero && Enabled)
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
