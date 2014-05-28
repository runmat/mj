using System.Windows;
using System.Windows.Threading;
using CKGDatabaseAdminLib.ViewModels;

namespace CKGDatabaseAdminTool
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        private void ApplicationStart(object sender, StartupEventArgs e)
        {
            var mainWindow = new MainWindow(new MainViewModel());
            if (mainWindow.IsDbSelected)
            {
                mainWindow.Show();
            }
            else
            {
                Shutdown(0);
            }
        }
    }
}
