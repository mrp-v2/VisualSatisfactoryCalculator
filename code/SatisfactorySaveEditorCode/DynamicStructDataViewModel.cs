using GalaSoft.MvvmLight;
using SatisfactorySaveEditor.Util;
using SatisfactorySaveEditor.ViewModel.Property;
using SatisfactorySaveParser.PropertyTypes.Structs;
using System.Collections.ObjectModel;
using System.Linq;

namespace SatisfactorySaveEditor.ViewModel.Struct
{
	public class DynamicStructDataViewModel : ViewModelBase
	{
		private readonly DynamicStructData model;
		public ObservableCollection<SerializedPropertyViewModel> Fields { get; }

		public DynamicStructDataViewModel(DynamicStructData dynamicStruct)
		{
			model = dynamicStruct;

			Fields = new ObservableCollection<SerializedPropertyViewModel>(dynamicStruct.Fields.Select(PropertyViewModelMapper.Convert));
		}
	}
}
