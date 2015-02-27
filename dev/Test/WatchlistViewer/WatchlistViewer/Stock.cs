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
            { "Goldpreis", "Gold" },
            { "Siemens", "SIE" },
            { "OSRAM", "Osr" },
            { "Euro / US", "€/US" },
            { "Euro / Schwei", "€/CHF" },
            { "Ölpreis Brent", "Öl" },
        };

        public string Name
        {
            get { return _name; }
            set { _name = value; SendPropertyChanged("Name"); SendPropertyChanged("ToolTip"); SendPropertyChanged("ShortName"); }
        }

        public string Wkn
        {
            get { return _wkn; }
            set { _wkn = value; SendPropertyChanged("Wkn"); SendPropertyChanged("ToolTip"); }
        }

        public DateTime DateTime
        {
            get { return _dateTime; }
            set { _dateTime = value; SendPropertyChanged("DateTime"); SendPropertyChanged("ToolTip"); }
        }

        public double Value
        {
            get { return _value; }
            set { _value = value; SendPropertyChanged("Value"); SendPropertyChanged("ForeColor"); }
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

        public string ShortName
        {
            get
            {
                var key = _nameTranslateDict.Keys.FirstOrDefault(k => Name.Contains(k));
                if (key == null)
                    return Name;

                return _nameTranslateDict[key];
            }
        }
    }
}
