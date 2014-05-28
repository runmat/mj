using GeneralTools.Models;

namespace CarDocu.Models
{
    public class UserUIHintEntity : ModelBase 
    {
        private string _key; 
        public string Key 
        { 
            get { return _key; }
            set { _key = value; SendPropertyChanged("Key"); }
        }

        private string _title; 
        public string Title 
        { 
            get { return _title; }
            set { _title = value; SendPropertyChanged("Header"); }
        }

        private bool _isConfirmed; 
        public bool IsConfirmed 
        { 
            get { return _isConfirmed; }
            set { _isConfirmed = value; SendPropertyChanged("IsConfirmed"); }
        }
    }
}
