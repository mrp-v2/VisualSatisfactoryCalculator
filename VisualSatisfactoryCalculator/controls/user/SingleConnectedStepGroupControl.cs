using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MrpV2.GenericLibrary.code.dictionary.bidirectional.classes;

using VisualSatisfactoryCalculator.model.production;

namespace VisualSatisfactoryCalculator.controls.user
{
	public partial class SingleConnectedStepGroupControl<ItemType> : UserControl where ItemType : AbstractItem
	{
		private readonly BidirectionalDictionary<AbstractStep<ItemType>, RationalNumberControl> producersStepControlMap = new BidirectionalDictionary<AbstractStep<ItemType>, RationalNumberControl>();
		private readonly BidirectionalDictionary<AbstractStep<ItemType>, RationalNumberControl> consumersStepControlMap = new BidirectionalDictionary<AbstractStep<ItemType>, RationalNumberControl>();

		public SingleConnectedStepGroupControl(Connection<ItemType> connection, HashSet<AbstractStep<ItemType>> singleConnectedStepGroup, bool locked)
		{
			InitializeComponent();
			foreach (AbstractStep<ItemType> step in singleConnectedStepGroup)
			{
				if (connection.IsStepConsumer(step))
				{
					RationalNumberControl control = new RationalNumberControl(!locked);
					control.SetNumber(step.GetRate(connection.Item, false).Rate);
					control.AddNumberChangedListener(() =>
					{
						NumberChanged(control);
					});
					ConsumersPanel.Controls.Add(control);
					consumersStepControlMap.Add(step, control);
				}
				if (connection.IsStepProducer(step))
				{
					RationalNumberControl control = new RationalNumberControl(!locked);
					control.SetNumber(step.GetRate(connection.Item, true).Rate);
					control.AddNumberChangedListener(() =>
					{
						NumberChanged(control);
					});
					ProducersPanel.Controls.Add(control);
					producersStepControlMap.Add(step, control);
				}
				// TODO set net group rate number
			}
		}

		private void NetGroupRateChanged()
		{
			// TODO
			throw new NotImplementedException();
		}

		private void NumberChanged(RationalNumberControl control)
		{
			// TODO
			throw new NotImplementedException();
		}
	}
}
