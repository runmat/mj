using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GeneralTools.Models;

namespace WatchlistViewer
{
    public partial class MainWindow 
    {
        public static ICommand ContextMenuItemCommand { get; private set; } 
         
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel();
        }

        private void MainWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            //const int margin Right = FirefoxWebDriver.BrowserWidth + FirefoxWebDriver.BrowserMarginRight;

            var desktopWorkingArea = SystemParameters.WorkArea;
            Left = desktopWorkingArea.Width - e.NewSize.Width - 150; //- marginRight + 350;
        }
    }
}
