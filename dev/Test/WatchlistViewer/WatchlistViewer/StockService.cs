using System.Globalization;
// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Linq;
using GeneralTools.Models;

namespace WatchlistViewer
{
    public class StockService
    {
        public static List<Stock> ParseStocks(string stockData)
        {
            stockData = PrepareStockData(stockData);

            var stockList = new List<Stock>();

            stockData = stockData.Replace("\r", "");
            stockData = stockData.Replace("Name Kurs Aktuell 52W", "");
            var sArray = stockData.Split('\n').Skip(1).ToArray();

            var index = 0;
            var subIndex = 0;
            while (index < sArray.Length)
            {
                var hasWkn = !(sArray[index + 1].Contains(":"));
                var itemLength = hasWkn ? 10 : 9;

                subIndex++;
                if (subIndex >= itemLength)
                {
                    var i = index;
                    var si = hasWkn ? 1 : 0;

                    index += subIndex;
                    subIndex = 0;

                    var stockItem = new Stock
                    {

                        Name = sArray[i + 0].Replace("_", " ").SubstringTry(0, 16),
                        Wkn = (hasWkn ? sArray[i + 1] : ""),
                        DateTime = DateTime.ParseExact(DateTime.Today.ToString("dd.MM.yyyy") + " " + sArray[i + si + 1], "dd.MM.yyyy HH:mm:ss", CultureInfo.CurrentCulture),
                        Value = double.Parse(sArray[i + si + 3]),
                    };

                    stockItem.Name = stockItem.Name.Replace("db ", "");
                    stockItem.Name = stockItem.Name.Replace(" 500", "");
                    stockItem.Name = stockItem.Name.Replace(" Jones", "");
                    stockItem.Name = stockItem.Name.Replace("Volkswagen Vz.", "VW");

                    var changeValue = double.Parse(sArray[i + si + 6]);
                    stockItem.OpenValue = stockItem.Value - changeValue;
                    //stockItem.TopValue = double.Parse(itemArray[3].Replace("-", "").Trim());
                    //stockItem.BottomValue = double.Parse(itemArray[4].Replace("-", "").Trim());

                    stockList.Add(stockItem);
                }
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
