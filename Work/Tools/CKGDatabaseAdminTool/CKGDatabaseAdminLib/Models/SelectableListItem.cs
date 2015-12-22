using System;
using System.Xml.Serialization;

namespace CKGDatabaseAdminLib.Models
{
    public class SelectableListItem : DbModelBase
    {
        [XmlIgnore]
        public Action<SelectableListItem> OnChange { get; set; }

        private string _key;

        public string Key
        {
            get { return _key; }
            set
            {
                _key = value;
                OnPropertyChanged("Key");
            }
        }

        private bool _isChecked;

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                OnPropertyChanged("IsChecked");

                if (OnChange != null)
                    OnChange(this);
            }
        }
    }
}
