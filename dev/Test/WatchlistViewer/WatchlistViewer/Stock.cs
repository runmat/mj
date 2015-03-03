using System.Windows.Input;
using WpfTools4.Commands;
// ReSharper disable RedundantUsingDirective
using System.Windows.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneralTools.Models;

namespace WatchlistViewer
{
    public class Stock : ModelBase 
    {
        private string _name;
        private string _wkn;
        private DateTime _dateTime;
        private double _value;
        private double _change;

        private readonly Dictionary<string, string> _nameTranslateDict = new Dictionary<string, string>
        {
            { "Dow Jones", "Dow" },
            { "National Bank", "NGR" },
            { "Goldpreis", "Gold~1326189" },
            { "Siemens", "SIE" },
            { "OSRAM", "Osr" },
            { "Euro / US", "€/US~1390634~0.0000" },
            { "Euro / Schwei", "€/CHF~8362186~0.0000" },
            { "Ölpreis Brent", "Öl~31117610" },
            { "Put", "Put"}
        };

        public string Name
        {
            get { return _name; }
            set { _name = value; SendPropertyChanged("Name"); SendPropertyChanged("ToolTip"); SendPropertyChanged("ShortName"); }
        }

        public string Wkn
        {
            get { return _wkn; }
            set { _wkn = value; SendPropertyChanged("Wkn"); SendPropertyChanged("ToolTip"); SendPropertyChanged("IdNotation"); }
        }

        public DateTime DateTime
        {
            get { return _dateTime; }
            set { _dateTime = value; SendPropertyChanged("DateTime"); SendPropertyChanged("ToolTip"); }
        }

        public double Value
        {
            get { return _value; }
            set { _value = value; SendPropertyChanged("Value"); SendPropertyChanged("ValueFormatted"); SendPropertyChanged("ForeColor"); }
        }

        public double Change
        {
            get { return _change; }
            set { _change = value; SendPropertyChanged("Change"); SendPropertyChanged("ForeColor"); }
        }

        public Brush ForeColor
        {
            get { return (Change < 0 ? Brushes.Red : Brushes.Green); }
        }

        public string ToolTip { get { return string.Format("Zeit {0:HH:mm:ss} vom {0:dd.MM.yyyy}", DateTime); } }

        public string ShortName { get { return GetPartOfValue(0, "{self}"); } }

        public string IdNotation { get { return GetPartOfValue(1, ""); } }

        public string ValueFormatted { get { return Value.ToString(GetPartOfValue(2, "#,##0.00")); } }

        private string GetPartOfValue(int index, string defaultValue)
        {
            var key = _nameTranslateDict.Keys.FirstOrDefault(k => Name.Contains(k));
            if (key == null)
                return GetDefaultValue(Name, defaultValue);

            var val = _nameTranslateDict[key];
            if (!val.Contains("~"))
                return GetDefaultValue(val, defaultValue);

            var valArray = val.Split('~');
            if (index >= valArray.Length)
                return GetDefaultValue(val, defaultValue);

            return valArray[index];
        }

        static string GetDefaultValue(string val, string defaultValue)
        {
            if (defaultValue == "{self}")
                return val;
            
            return defaultValue;
        }

        public MainViewModel Parent { get; set; }

        public ICommand ShowWknAtComdirectCommand { get; private set; }

        public Stock()
        {
            ShowWknAtComdirectCommand = new DelegateCommand(e => Parent.ShowWknAtComdirect(this), e => true);
        }
    }
}
