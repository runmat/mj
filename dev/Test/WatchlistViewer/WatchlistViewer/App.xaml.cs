using System.Windows;

namespace WatchlistViewer
{
    public partial class App 
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ProcessHelper.KillAllProcessesOf("WatchlistViewer");

            base.OnStartup(e);
        }

        static void Test()
        {
            FirefoxWebDriver.InvokeWatchlist();

            Current.Shutdown();
        }
    }
}
