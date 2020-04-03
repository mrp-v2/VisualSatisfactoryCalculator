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
		public ObservableCollection<SerializedPropertyViewModel> Fields { get; }

		public DynamicStructDataViewModel(DynamicStructData dynamicStruct)
		{
			Fields = new ObservableCollection<SerializedPropertyViewModel>(dynamicStruct.Fields.Select(PropertyViewModelMapper.Convert));
		}
	}
}
