using System.Windows.Input;
using WpfTools4.Commands;
// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchlistViewer
{
    public class MainViewModel
    {
        public ICommand WatchlistShowCommand { get; private set; }
        public ICommand WatchlistHideCommand { get; private set; }

        public MainViewModel()
        {
            WatchlistShowCommand = new DelegateCommand(e => WatchlistShow(), e => true);
            WatchlistHideCommand = new DelegateCommand(e => WatchlistHide(), e => true);

            //FirefoxWebDriver.InvokeWatchlist();
        }

        void WatchlistShow()
        {
            FirefoxWebDriver.ShowBrowser();
        }

        void WatchlistHide()
        {
            FirefoxWebDriver.HideBrowser();
        }
    }
}
