using SatisfactorySaveParser.PropertyTypes;

namespace SatisfactorySaveEditor.ViewModel.Property
{
	public class StrPropertyViewModel : SerializedPropertyViewModel
	{
		private readonly StrProperty model;

		private string value;

		public string Value
		{
			get
			{
				return value;
			}

			set { Set(() => Value, ref this.value, value); }
		}

		public StrPropertyViewModel(StrProperty strProperty) : base(strProperty)
		{
			model = strProperty;

			value = model.Value;
		}
	}
}
