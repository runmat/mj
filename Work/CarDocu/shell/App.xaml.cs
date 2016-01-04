using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using CarDocu.Services;
using CarDocu.ViewModels;
using GeneralTools.Services;
using WpfTools4.Common;

namespace CarDocu
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App
    {
        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        }

        private void ApplicationStart(object sender, StartupEventArgs e)
        {
            if (AssemblyService.ApplicationCloneOfMeIsAlreadyRunning(Tools.Alert, DomainService.AppName))
            {
                Current.Shutdown();
                return;
            }

            TaskService.InitUiSynchronizationContext();

            //Disable shutdown when the dialog closes
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            if (!DomainService.ValidDomainSettingsAvailable())
            {
                ShowInitialAppSettingsDialog();
                return;
            }

            DomainService.LoadGlobalSettings();

            if (!DomainService.ValidArchivesAvailable())
            {
                ShowInitialAppSettingsDialog("Geben Sie bitte jetzt noch die Dateipfade zu den Archiven an:");
                return;
            }

            if (!DomainService.UserLogon(GetUserLoginNameFromDialog, DomainService.DebugIsAdminEnvironment))
                Current.Shutdown();
            else
            {
                Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                new MainWindow().ShowDialog();
            }

            ClearTempFolders();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            //EventManager.RegisterClassHandler(typeof(UIElement), UIElement.PreviewKeyDownEvent, new RoutedEventHandler(KeyDownEventHandler), true);

            var splashScreen = new SplashScreen("logo_cardocu-border.jpg");
            splashScreen.Show(true, true);

            base.OnStartup(e);
        }

        //internal static void KeyDownEventHandler(object sender, RoutedEventArgs e)
        //{
        //    if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.A))
        //    {
        //        var newDocuViewModel = MainViewModel.Instance.NewDocuViewModel;
        //        if (newDocuViewModel != null && newDocuViewModel.ScanDocumentSaveCommand.CanExecute(null))
        //            newDocuViewModel.ScanDocumentSaveCommand.Execute(null);

        //        e.Handled = true;
        //    }
        //}

        public static void ClearTempFolders()
        {
            try
            {
                var dirInfo = new DirectoryInfo(DomainService.Repository.GlobalSettings.TempPath);
                foreach (var file in dirInfo.GetFiles())
                {
                    try
                    {
                        file.Delete();
                    }
                    catch (Exception)
                    {
                    }


                }
            }
            catch (Exception)
            {

            }

        }

        private static void ShowInitialAppSettingsDialog(string userHintForArchives = null)
        {
            var firstRunStartUpDialog = new AppSettingsInitial();
            var context = new AppSettingsInitialViewModel(firstRunStartUpDialog, userHintForArchives);
            firstRunStartUpDialog.DataContext = context;
            firstRunStartUpDialog.ShowDialog();
            if (context.Success)
                Restart();
            else
                Current.Shutdown();
        }

        static string GetUserLoginNameFromDialog()
        {
            var firstRunStartUpDialog = new UserLogon();
            var context = new UserLogonViewModel();
            firstRunStartUpDialog.DataContext = context;
            firstRunStartUpDialog.ShowDialog();
            return context.LoginData;
        }

        public static void Restart()
        {
            var exeFileName = Assembly.GetEntryAssembly().Location;
            Process.Start(exeFileName);
            Current.Shutdown();
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MainWindow.IsEnabled = true;

            Actions.OnErrorMessageBox((Exception)e.ExceptionObject);
        }

        static public string GetFolderFromDialog(string selectedPath, string caption)
        {
            var dialog = new FolderBrowserDialog
            {
                SelectedPath = selectedPath,
                ShowNewFolderButton = true,
                Description = caption
            };

            var result = dialog.ShowDialog();
            if (result == DialogResult.Cancel)
                return null;

            return dialog.SelectedPath;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            DomainService.Threads.Dispose();

            base.OnExit(e);
        }
    }
}
