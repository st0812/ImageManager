using System;
using System.Collections.Generic;
using WpfViewModel;

namespace ImageViewer.ViewModels
{
    public class SearchWordViewModel: NotificationObject
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
        public SearchWordViewModel()
        {
            Value = string.Empty;
        }
        public bool IsExist()
        {
            return !string.IsNullOrWhiteSpace(Value);
        }

        public IEnumerable<string> ToKeywordss()
        {
            return Value.Split(new string[] { " ", "　" }, StringSplitOptions.RemoveEmptyEntries);
        }

        public void AddWord(string value)
        {
            Value =Value+ " " + value;
        }
    }
}
