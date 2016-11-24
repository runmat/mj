// ReSharper disable RedundantUsingDirective
using System.Diagnostics;
using WpfTools4.ViewModels;
using System.Windows.Input;
using WpfTools4.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GeneralTools.Models;
using Microsoft.Win32;

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
        public ICommand ProxyOnCommand { get; private set; }
        public ICommand ProxyOffCommand { get; private set; }


        public ICommand QuitCommand { get; private set; }

        private readonly System.Windows.Forms.Timer _initialDelayTimer;
        private System.Windows.Forms.Timer _workTimer;

        public MainViewModel()
        {
            ProxyOnCommand = new DelegateCommand(e => ProxyActivate(true), e => true);
            ProxyOffCommand = new DelegateCommand(e => ProxyActivate(false), e => true);
            WatchlistToggleCommand = new DelegateCommand(e => WatchlistToggle(), e => true);
            QuitCommand = new DelegateCommand(e => Quit(), e => true);

            //FirefoxWebDriver.GetStockDataTest();

            FirefoxWebDriver.InvokeWatchlist();
            _initialDelayTimer = new System.Windows.Forms.Timer { Enabled = true, Interval = 100 };
            _initialDelayTimer.Tick += InitialDelayTimerTick;
        }

        void InitialDelayTimerTick(object sender, EventArgs e)
        {
            _initialDelayTimer.Stop();
            _initialDelayTimer.Dispose();

            _workTimer = new System.Windows.Forms.Timer { Enabled = true, Interval = 10000 };
            _workTimer.Tick += WorkTimerTick;
        }

        void WorkTimerTick(object sender, EventArgs e)
        {
            TaskService.StartLongRunningTask(GetStockData);
        }

        private void GetStockData()
        {
            WatchlistRefresh(true);
            Thread.Sleep(3000);

            var items = FirefoxWebDriver.GetStockData();
            if (items == null)
                return;

            if (StockItems == null || StockItems.None())
            {
                items.ForEach(item => item.Parent = this);
                StockItems = items;
            }
            items.ForEach(item =>
            {
                var stockItem = StockItems.FirstOrDefault(si => si.ShortName == item.ShortName);
                if (stockItem == null)
                    return;

                ModelMapping.Copy(item, stockItem);
                stockItem.Parent = this;
            });

            StockItemsVisible = true;
        }

        private static void WatchlistToggle()
        {
            _browserVisible = !_browserVisible;

            if (_browserVisible)
                WatchlistShow();
            else
                WatchlistHide();
        }

        private static void ProxyActivate(bool activate)
        {
            SetIeProxyRegKey(activate ? 1 : 0);
        }

        private static void SetIeProxyRegKey(int value)
        {
            var regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Internet Settings", true);
            if (regKey == null)
                return;

            regKey.SetValue("ProxyEnable", value);
            regKey.Close();
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
