// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneralTools.Models;

namespace WatchlistViewer
{
    public class Stock
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public string Timestamp { get; set; }

        public string ChangePercent { get; set; }
    }
}
