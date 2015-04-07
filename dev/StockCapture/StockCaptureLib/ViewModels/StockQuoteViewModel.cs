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
                    var latestStocks = repository.GetLatestStockQuotes(25);
                    return latestStocks;
                }
            }
        }
    }
}
