// ReSharper disable RedundantUsingDirective
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

        public string Name
        {
            get { return _name; }
            set { _name = value; SendPropertyChanged("Name"); }
        }

        public string Wkn
        {
            get { return _wkn; }
            set { _wkn = value; SendPropertyChanged("Wkn"); }
        }

        public DateTime DateTime
        {
            get { return _dateTime; }
            set { _dateTime = value; SendPropertyChanged("DateTime"); }
        }

        public double Value
        {
            get { return _value; }
            set { _value = value; SendPropertyChanged("Value"); }
        }

        public double Change
        {
            get { return _change; }
            set { _change = value; SendPropertyChanged("Change"); }
        }
    }
}
