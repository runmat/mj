using GeneralTools.Models;

namespace CarDocu.Models.Settings
{
    public class ScanSettings : ModelBase
    {
        private int _dpi = 150;
        public int DPI 
        { 
            get { return _dpi; }
            set { _dpi = value; SendPropertyChanged("DPI"); }
        }

        private bool _useColor; 
        public bool UseColor 
        { 
            get { return _useColor; }
            set { _useColor = value; SendPropertyChanged("UseColor"); }
        }
    }
}
