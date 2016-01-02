using System.Windows.Input;
using WpfTools4.Commands;
using WpfTools4.ViewModels;

namespace WpfComboboxAutocomplete
{
    public class Tag : ViewModelBase
    {
        private string _name;
        private int _sort;

        public ICommand DeleteCommand { get; private set; }

        public string Name
        {
            get { return _name; }
            set { _name = value; SendPropertyChanged("Name"); }
        }

        public int Sort
        {
            get { return _sort; }
            set { _sort = value; SendPropertyChanged("Sort"); }
        }

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
