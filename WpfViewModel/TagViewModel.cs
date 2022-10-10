using ImageApplication.Tags;

namespace WpfViewModel
{
    public class TagViewModel:NotificationObject
    {

        public TagViewModel(TagData model)
        {
            Model = model;
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                RaisePropertyChanged();
            }
        }

        public TagData Model { get; }

        public string ID { get => Model.TagID; }

        public string Name { get => Model.TagName; }
    }
}
