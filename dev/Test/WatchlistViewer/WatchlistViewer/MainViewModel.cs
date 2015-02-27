using WpfTools4.ViewModels;
// ReSharper disable RedundantUsingDirective
using System.Windows.Input;
using WpfTools4.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchlistViewer
{
    public class MainViewModel : ViewModelBase
    {
        private List<Stock> _stockItems;

        public List<Stock> StockItems
        {
            get { return _stockItems; }
            set { _stockItems = value; SendPropertyChanged("StockItems"); }
        }

        public ICommand WatchlistShowCommand { get; private set; }
        public ICommand WatchlistHideCommand { get; private set; }
        public ICommand GetStockDataCommand { get; private set; }

        public MainViewModel()
        {
            WatchlistShowCommand = new DelegateCommand(e => WatchlistShow(), e => true);
            WatchlistHideCommand = new DelegateCommand(e => WatchlistHide(), e => true);
            GetStockDataCommand = new DelegateCommand(e => GetStockData(), e => true);

            //FirefoxWebDriver.InvokeWatchlist();
        }

        private void WatchlistShow()
        {
            FirefoxWebDriver.ShowBrowser();
        }

        private void WatchlistHide()
        {
            FirefoxWebDriver.HideBrowser();
        }

        private void GetStockData()
        {
            StockItems = FirefoxWebDriver.GetStockData();
        }
    }
}
