using System.Windows.Input;
using System.Xml.Serialization;
using WpfTools4.Commands;
using WpfTools4.ViewModels;

namespace WpfComboboxAutocomplete
{
    public class Tag : ViewModelBase
    {
        private string _name;
        private int _sort;

        [XmlIgnore]
        public ICommand DeleteCommand { get; private set; }

        public string Name
        {
            get { return _name; }
            set { _name = value; SendPropertyChanged("Name"); }
        }

        [XmlIgnore]
        public int Sort
        {
            get { return _sort; }
            set { _sort = value; SendPropertyChanged("Sort"); }
        }

        [XmlIgnore]
        public IAutoCompleteTagCloudConsumer Parent { get; set; }


        public Tag()
        {
            DeleteCommand = new DelegateCommand(DeleteAction);
        }

        void DeleteAction(object e)
        {
            Parent?.OnDeleteTag(Name);
        }
    }
}
