// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StockCapture.Models;

namespace StockCapture.ViewModels
{
    public class StockQuoteViewModel
    {
        public List<StockQuote> StockQuotes
        {
            get
            {
                using (var repository = new Repository())
                {
                    var latestStocks = repository.GetLatestStockQuotes(60*24)
                        .Where(sq => sq.Date.GetValueOrDefault() > DateTime.Parse("07.04.2015 12:20"))
                            .ToList();
                    return latestStocks;
                }
            }
        }

        public List<StockQuote> LatestStockQuotes
        {
            get
            {
                using (var repository = new Repository())
                {
                    var latestStocks = repository.GetLatestStockQuotes(100);
                    return latestStocks;
                }
            }
        }
    }
}
