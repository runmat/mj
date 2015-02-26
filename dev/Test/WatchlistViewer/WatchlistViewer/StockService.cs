// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneralTools.Models;

namespace WatchlistViewer
{
    public class StockService
    {
        public static void ParseStocks(string stockData)
        {
            var array = stockData.Split('\n');
            foreach (var stock in array)
            {
                var s = stock.Replace("\\n", "~");
                s = s.Replace("\\r", "");
                s = s.Replace("\\n", "");
                s = s.Trim();
                var stockArray = s.Split('~');
                
            }
        }
    }
}
