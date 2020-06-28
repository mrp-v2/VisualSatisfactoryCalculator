using SatisfactorySaveParser.PropertyTypes;

namespace SatisfactorySaveEditor.ViewModel.Property
{
    public class StructPropertyViewModel : SerializedPropertyViewModel
    {
        private readonly StructProperty model;

        private object structData; // TODO: Rest of the owl, implement view models for structs

        public string Type => model.Type;

        public object StructData
        {
            get => structData;
            set { Set(() => StructData, ref structData, value); }
        }

        public StructPropertyViewModel(StructProperty structProperty) : base(structProperty)
        {
            model = structProperty;
        }
    }
}
