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
                var wknCandidate = sArray[index + 1];
                var hasWkn = !(wknCandidate.Contains(":") || wknCandidate.Contains("."));
                var itemLength = hasWkn ? 9 : 8;

                subIndex++;
                if (subIndex >= itemLength)
                {
                    var i = index;
                    var si = hasWkn ? 1 : 0;

                    index += subIndex;
                    subIndex = 0;

                    var name = sArray[i + 0];
                    var wkn = sArray[i + 1];
                    var val = sArray[i + si + 3];
                    var date = sArray[i + si + 1];
                    var stockItem = new Stock
                    {
                        Name = name.Replace("_", " ").SubstringTry(0, 16),
                        Wkn = (hasWkn ? wkn : ""),
                        Value = double.Parse(val),
                        DateTime = DateTime.Now
                    };
                    try
                    {
                        stockItem.DateTime = DateTime.ParseExact(DateTime.Today.ToString("dd.MM.yyyy") + " " + date, "dd.MM.yyyy HH:mm:ss", CultureInfo.CurrentCulture);
                    }
                    catch
                    {
                        DateTime dt;
                        if (DateTime.TryParseExact(date, "dd.MM.yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out dt))
                            stockItem.DateTime = dt;
                    }

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
