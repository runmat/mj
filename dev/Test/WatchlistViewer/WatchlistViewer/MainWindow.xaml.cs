using System.Windows;
using System.Windows.Input;

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
            //const int marginRight = FirefoxWebDriver.BrowserWidth + FirefoxWebDriver.BrowserMarginRight;

            var desktopWorkingArea = SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Width - e.NewSize.Width - 150; //- marginRight + 350;
            ;
        }
    }
}
