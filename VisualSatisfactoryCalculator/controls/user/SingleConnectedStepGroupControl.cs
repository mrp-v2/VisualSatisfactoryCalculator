using System.Windows.Forms;

using VisualSatisfactoryCalculator.model.production;

namespace VisualSatisfactoryCalculator.controls.user
{
	public partial class SingleConnectedStepGroupControl<ItemType> : UserControl where ItemType : BasicItem
	{
		/*private readonly Dictionary<AbstractStep<ItemType>, RationalNumberControl> producersStepControlMap = new Dictionary<AbstractStep<ItemType>, RationalNumberControl>();
		private readonly Dictionary<AbstractStep<ItemType>, RationalNumberControl> consumersStepControlMap = new Dictionary<AbstractStep<ItemType>, RationalNumberControl>();
		private readonly Dictionary<RationalNumberControl, AbstractStep<ItemType>> controlMap = new Dictionary<RationalNumberControl, AbstractStep<ItemType>>();
		private readonly Connection<ItemType> connection;

		public SingleConnectedStepGroupControl(Connection<ItemType> connection, HashSet<AbstractStep<ItemType>> singleConnectedStepGroup, bool locked)
		{
			this.connection = connection;
			InitializeComponent();
			decimal netRate = 0;
			foreach (AbstractStep<ItemType> step in singleConnectedStepGroup)
			{
				if (connection.IsStepConsumer(step))
				{
					RationalNumberControl control = new RationalNumberControl(!locked);
					control.SetNumber(step.GetRate(connection.Item, false).Rate);
					netRate -= control.GetNumber();
					control.AddNumberChangedListener((oldValue, newValue) =>
					{
						NumberChanged(control, oldValue, newValue, false);
					});
					ConsumersPanel.Controls.Add(control);
					consumersStepControlMap.Add(step, control);
					controlMap.Add(control, step);
				}
				if (connection.IsStepProducer(step))
				{
					RationalNumberControl control = new RationalNumberControl(!locked);
					control.SetNumber(step.GetRate(connection.Item, true).Rate);
					netRate += control.GetNumber();
					control.AddNumberChangedListener((oldValue, newValue) =>
					{
						NumberChanged(control, oldValue, newValue, true);
					});
					ProducersPanel.Controls.Add(control);
					producersStepControlMap.Add(step, control);
					controlMap.Add(control, step);
				}
				NetGroupRate.SetNumber(netRate);
				NetGroupRate.AddNumberChangedListener(NetGroupRateChanged);
			}
		}

		private void NetGroupRateChanged(decimal oldValue, decimal newValue)
		{
			// TODO
			throw new NotImplementedException();
		}

		private void NumberChanged(RationalNumberControl control, decimal oldValue, decimal newValue, bool isProducer)
		{
			// TODO
			controlMap[control].CascadingUpdateRatesFrom(new ItemRate<ItemType>(connection.Item, newValue), isProducer);
			throw new NotImplementedException();
		}*/
	}
}
