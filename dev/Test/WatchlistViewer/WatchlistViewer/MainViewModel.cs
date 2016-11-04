// ReSharper disable RedundantUsingDirective
using System.Diagnostics;
using WpfTools4.ViewModels;
using System.Windows.Input;
using WpfTools4.Commands;
using System;
using System.Collections.Generic;

namespace WatchlistViewer
{
    public class MainViewModel : ViewModelBase
    {
        private List<Stock> _stockItems;
        private bool _stockItemsVisible;

        private static bool _browserVisible;
        private static bool _browserRefreshPaused;

        public List<Stock> StockItems
        {
            get { return _stockItems; }
            set { _stockItems = value; SendPropertyChanged("StockItems"); }
        }

        public bool StockItemsVisible
        {
            get { return _stockItemsVisible; }
            set { _stockItemsVisible = value; SendPropertyChanged("StockItemsVisible"); }
        }
        

        public ICommand WatchlistToggleCommand { get; private set; }

        public ICommand QuitCommand { get; private set; }

        private readonly System.Windows.Forms.Timer _initialDelayTimer;
        private System.Windows.Forms.Timer _workTimer;

        public MainViewModel()
        {
            WatchlistToggleCommand = new DelegateCommand(e => WatchlistToggle(), e => true);
            QuitCommand = new DelegateCommand(e => Quit(), e => true);

            FirefoxWebDriver.InvokeWatchlist();
            _initialDelayTimer = new System.Windows.Forms.Timer { Enabled = true, Interval = 100 };
            _initialDelayTimer.Tick += InitialDelayTimerTick;
        }

        void InitialDelayTimerTick(object sender, EventArgs e)
        {
            _initialDelayTimer.Stop();
            _initialDelayTimer.Dispose();

            _workTimer = new System.Windows.Forms.Timer { Enabled = true, Interval = 60000 };
            _workTimer.Tick += WorkTimerTick;
        }

        void WorkTimerTick(object sender, EventArgs e)
        {
            TaskService.StartLongRunningTask(() => WatchlistRefresh());
        }

        private static void WatchlistToggle()
        {
            _browserVisible = !_browserVisible;

            if (_browserVisible)
                WatchlistShow();
            else
                WatchlistHide();
        }

        private static void WatchlistShow()
        {
            _browserRefreshPaused = true;
            WatchlistRefresh(true);
            FirefoxWebDriver.ShowBrowser();
        }

        private static void WatchlistHide()
        {
            FirefoxWebDriver.HideBrowser();
        }

        private static void WatchlistRefresh(bool forceRefresh = false)
        {
            if (_browserRefreshPaused && !forceRefresh)
            {
                _browserRefreshPaused = false;
                return;
            }

            if (FirefoxWebDriver.IsBrowserVisible || forceRefresh)
                FirefoxWebDriver.RefreshBrowser();
        }

        private static void Quit()
        {
            ProcessHelper.KillAllProcessesOf("FireFox");
            Process.GetCurrentProcess().Kill();
        }
    }
}
