using System.Windows;
using GeneralTools.Models;

namespace CkgNetworkSwitch
{
    public class MainViewModel : ModelBase 
    {
        private bool _isWlanEnabled = NetworkService.IsWlanEnabled();

        public bool IsWlanEnabled
        {
            get { return _isWlanEnabled; }
            set
            {
                var changed = _isWlanEnabled != value;

                _isWlanEnabled = value;
                SendPropertyChanged("IsWlanEnabled");

                if (changed)
                    ;//NetworkService.EnableWlan(_isWlanEnabled);
            }
        }
    }
}