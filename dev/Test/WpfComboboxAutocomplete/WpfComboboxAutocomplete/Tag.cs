using WpfTools4.ViewModels;

namespace WpfComboboxAutocomplete
{
    public class Tag : ViewModelBase
    {
        private string _name;
        private int _sort;

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
    }
}
