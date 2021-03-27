using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using VisualSatisfactoryCalculator.code.Production;
using VisualSatisfactoryCalculator.forms;

using static VisualSatisfactoryCalculator.code.Utility.PlanLayoutMaker;

namespace VisualSatisfactoryCalculator.controls.user
{
	public partial class SplitAndMergeControl : UserControl, ILayoutControl
	{
		public readonly Connection BackingConnection;
		public readonly MainForm MainForm;
		public Dictionary<Step, ItemRateControl> OutControls = new Dictionary<Step, ItemRateControl>();
		public Dictionary<Step, ItemRateControl> InControls = new Dictionary<Step, ItemRateControl>();

		public SplitAndMergeControl(Connection backingConnection, MainForm mainForm)
		{
			InitializeComponent();
			BackingConnection = backingConnection;
			MainForm = mainForm;
			backingConnection.SetControl(this);
			foreach (Step step in backingConnection.GetProducerSteps())
			{
				ItemRateControl irc = new ItemRateControl(mainForm, BackingConnection.ItemUID, backingConnection.GetProducerRate(step), true, 3, RateChanged, ItemClicked);
				irc.ItemButton.Enabled = false;
				InPanel.Controls.Add(irc);
				InControls.Add(step, irc);
			}
			foreach (Step step in backingConnection.GetConsumerSteps())
			{
				ItemRateControl irc = new ItemRateControl(mainForm, BackingConnection.ItemUID, BackingConnection.GetConsumerRate(step), false, 3, RateChanged, ItemClicked);
				irc.ItemButton.Enabled = false;
				irc.Anchor = AnchorStyles.Bottom;
				OutPanel.Controls.Add(irc);
				OutControls.Add(step, irc);
			}
			Disposed += OnDisposed;
		}

		private void OnDisposed(object sender, EventArgs e)
		{
			BackingConnection.SetControl(null);
		}

		public void UpdateNumerics()
		{
			ToggleInput(false);
			foreach (Step step in OutControls.Keys)
			{
				ItemRateControl irc = OutControls[step];
				irc.UpdateRateValue(BackingConnection.GetConsumerRate(step));
			}
			foreach (Step step in InControls.Keys)
			{
				ItemRateControl irc = InControls[step];
				if (BackingConnection.ConnectedSteps.Get().Contains(step))
				{
					irc.UpdateRateValue(BackingConnection.GetProducerRate(step));
				}
			}
			ToggleInput(true);
		}

		private void ToggleInput(bool on)
		{
			Enabled = on;
		}

		private void RateChanged(string itemUID, decimal newRate, bool isProduct)
		{

		}

		private void ItemClicked(string itemUID, bool isProduct)
		{

		}

		public StepControl TopControl
		{
			get
			{
				throw new NotImplementedException("Split and merge control does not have a TopControl");
			}
		}

		private bool placed;

		public void Place(int xStart, int yStart)
		{
			if (placed)
			{
				throw new InvalidOperationException("Can't place a step control that has already been placed");
			}
			Location = new Point(xStart, yStart);
			placed = true;
		}
	}
}
