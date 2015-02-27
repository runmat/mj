using System.Globalization;
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
        public static List<Stock> ParseStocks(string stockData)
        {
            stockData = PrepareStockData(stockData);

            var stockList = new List<Stock>();

            var sArray = stockData.Split('\n').Skip(1).ToArray();
            for (var index = 0; index < sArray.Length; index += 5)
            {
                var itemArray = new string[5];
                for (var j = 0; j < 5; j++)
                    itemArray[j] = sArray[index + j].Trim();

                var subItems = itemArray[1].Split(' ');
                var subItemsIndex = (subItems[0].Contains(":") ? 0 : 1);
                var stockItem = new Stock
                {
                    Name = itemArray[0].Replace("_", " ").SubstringTry(0, 16),
                    Wkn = (subItemsIndex > 0 ? subItems[0] : ""),
                    DateTime = DateTime.ParseExact(DateTime.Today.ToString("dd.MM.yyyy") + " " + subItems[subItemsIndex], "dd.MM.yyyy HH:mm:ss", CultureInfo.CurrentCulture),
                    Value = Double.Parse(subItems[subItemsIndex + 2]),
                    Change = Double.Parse(subItems[subItemsIndex + 4]),
                };

                stockList.Add(stockItem);
            }

            return stockList;
        }

        static string PrepareStockData(string stockData)
        {
            stockData = stockData.Replace("Deutsche Bank", "Deutsche_Bank");
            stockData = stockData.Replace("Dow Jones", "Dow_Jones");

            return stockData;
        }
    }
}
