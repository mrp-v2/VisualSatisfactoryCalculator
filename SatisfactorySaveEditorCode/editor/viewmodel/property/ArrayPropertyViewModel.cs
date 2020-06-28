using System.Collections.ObjectModel;
using System.Linq;
using SatisfactorySaveEditor.Util;
using SatisfactorySaveParser.PropertyTypes;

namespace SatisfactorySaveEditor.ViewModel.Property
{
    public class ArrayPropertyViewModel : SerializedPropertyViewModel
    {
        private readonly ArrayProperty model;

        private bool isExpanded;

        public ObservableCollection<SerializedPropertyViewModel> Elements { get; }

        public string Type => model.Type;

        public bool IsExpanded
        {
            get => isExpanded;
            set { Set(() => IsExpanded, ref isExpanded, value); }
        }

        public ArrayPropertyViewModel(ArrayProperty arrayProperty) : base(arrayProperty)
        {
            model = arrayProperty;

            Elements = new ObservableCollection<SerializedPropertyViewModel>(arrayProperty.Elements.Select(PropertyViewModelMapper.Convert));
            for (var i = 0; i < Elements.Count; i++) Elements[i].Index = i.ToString();

            IsExpanded = Elements.Count <= 3;
        }
    }
}
