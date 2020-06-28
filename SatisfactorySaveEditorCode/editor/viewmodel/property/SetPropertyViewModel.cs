using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight.CommandWpf;
using SatisfactorySaveEditor.Util;
using SatisfactorySaveParser.PropertyTypes;

namespace SatisfactorySaveEditor.ViewModel.Property
{
    public class SetPropertyViewModel : SerializedPropertyViewModel
    {
        private readonly SetProperty model;

        private bool isExpanded;

        public ObservableCollection<SerializedPropertyViewModel> Elements { get; }

        public string Type => model.Type;

        public bool IsExpanded
        {
            get => isExpanded;
            set { Set(() => IsExpanded, ref isExpanded, value); }
        }

        public SetPropertyViewModel(SetProperty setProperty) : base(setProperty)
        {
            model = setProperty;

            Elements = new ObservableCollection<SerializedPropertyViewModel>(setProperty.Elements.Select(PropertyViewModelMapper.Convert));
            for (var i = 0; i < Elements.Count; i++) Elements[i].Index = i.ToString();

            IsExpanded = Elements.Count <= 3;
        }
    }
}
