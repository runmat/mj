using System.Windows.Input;
using System.Xml.Serialization;
using CarDocu.Contracts;
using WpfTools4.Commands;
using WpfTools4.ViewModels;

namespace CarDocu.Models
{
    public class Tag : ViewModelBase
    {
        private string _name;

        [XmlIgnore]
        public ICommand DeleteCommand { get; private set; }

        public string Name
        {
            get { return _name; }
            set { _name = value; SendPropertyChanged("Name"); }
        }

        [XmlIgnore]
        public IAutoCompleteTagCloudConsumer Parent { get; set; }

        public bool IsPrivate { get; set; }


        public Tag()
        {
            DeleteCommand = new DelegateCommand(DeleteAction);
        }

        void DeleteAction(object e)
        {
            Parent?.OnDeleteTag(Name, IsPrivate);
        }
    }
}
