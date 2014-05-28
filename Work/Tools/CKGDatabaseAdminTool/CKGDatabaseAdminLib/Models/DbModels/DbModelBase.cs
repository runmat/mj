using System.ComponentModel;

namespace CKGDatabaseAdminLib.Models.DbModels
{
    public abstract class DbModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
 
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
