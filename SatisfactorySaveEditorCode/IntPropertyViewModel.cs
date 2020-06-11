using SatisfactorySaveParser.PropertyTypes;

namespace SatisfactorySaveEditor.ViewModel.Property
{
	public class IntPropertyViewModel : SerializedPropertyViewModel
	{
		private readonly IntProperty model;

		private int value;

		public int Value
		{
			get
			{
				return value;
			}

			set { Set(() => Value, ref this.value, value); }
		}

		public IntPropertyViewModel(IntProperty intProperty) : base(intProperty)
		{
			model = intProperty;

			value = model.Value;
		}
	}
}
