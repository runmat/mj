using System.Windows;
using CKGDatabaseAdminLib.ViewModels;
using GeneralTools.Models;

namespace CKGDatabaseAdminTool
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App
    {
        bool _useDefaultDbServer;
        bool _useDefaultStartupView;
        string _developer = "";
    
        private void ApplicationStart(object sender, StartupEventArgs e)
        {
            ProcessStartupArgs(e.Args);

            var mainWindow = new MainWindow(new MainViewModel(_useDefaultDbServer, _developer, _useDefaultStartupView));
            if (mainWindow.IsDbSelected)
            {
                mainWindow.Show();
            }
            else
            {
                Shutdown(0);
            }
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            var splashScreen = new SplashScreen("splashscreen.png");
            splashScreen.Show(true);

            base.OnStartup(e);
        }

        private void ProcessStartupArgs(string[] args)
        {
            if (args.Length < 1)
                return;

            for (var i = 0; i < args.Length; i++)
            {
                var arg = args[i].ToLower().Split('=');
                if (arg.Length != 2)
                    continue;

                switch (arg[0])
                {
                    case "usedefaultdbserver":
                        _useDefaultDbServer = arg[1].NotNullOrEmpty() == "true";
                        break;
                    case "usedefaultstartupview":
                        _useDefaultStartupView = arg[1].NotNullOrEmpty() == "true";
                        break;
                    case "developer":
                        _developer = arg[1];
                        break;
                }
            }
        }
    }
}
