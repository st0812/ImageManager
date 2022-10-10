namespace WpfViewModel
{
    public class NewTagNameViewModel:NotificationObject
    {
        private string _value;
        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                RaisePropertyChanged();
            }
        }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Value);
        }

        public string Normalize()
        {
            return Value.Replace(" ", "").Replace("　", "");
        }

        public void Reset()
        {
            Value = string.Empty;
        }
    }
}
