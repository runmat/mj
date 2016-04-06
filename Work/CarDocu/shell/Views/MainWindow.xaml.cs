using System.Windows;
using CarDocu.Services;
using CarDocu.ViewModels;
using Fluent;

namespace CarDocu
{
    public partial class MainWindow 
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Title = $"    {DomainService.AppName}, Version {DomainService.AppVersion}, Domain: {DomainService.DomainName},  User: {DomainService.Repository.LogonUser.FullName}";

            DataContext = new MainViewModel();

            Ribbon.SelectedTabItem = NewDocuTab;
            TaskService.StartDelayedUiTask(500, () =>
                                                    {
                                                        StoredDocuTab.Visibility = Visibility.Collapsed;
                                                        AdminGroup.Visibility = (DomainService.Repository.LogonUser.IsAdmin ? Visibility.Visible : Visibility.Collapsed);
                                                    });

            SizeChanged += MainWindowSizeChanged;

#if TEST
            this.Width = 1280;
#else
            WindowState = WindowState.Maximized;
#endif
        }

        void MainWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ((MainViewModel)DataContext).MainWindowSizeChanged(e.NewSize);
        }

        private RibbonTabItem _oldSelectedTabItem;
        void RibbonSelectedTabChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
// ReSharper disable PossibleUnintendedReferenceComparison
            if (_oldSelectedTabItem == Ribbon.SelectedTabItem)
// ReSharper restore PossibleUnintendedReferenceComparison
                return;

            _oldSelectedTabItem = Ribbon.SelectedTabItem;

            ((MainViewModel)DataContext).RibbonSelectedTabChanged(NewDocuTab.IsSelected);
        }

        private void RibbonGroupBoxLauncherClick(object sender, RoutedEventArgs e)
        {
            var context = (((FrameworkElement) sender).DataContext as DocuViewModel);
            context?.ShowBigDocuArtSelection();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (DomainService.Threads.IsBusy)
            {
                Tools.Alert("Hinweis:\r\n\r\nAktive Hintergrundprozesse müssen noch abgeschlossen werden!\r\n\r\nBitte versuchen Sie in ein paar Augenblicken noch einmal die Anwendung zu schließen.");
                e.Cancel = true;
                return;
            }

            base.OnClosing(e);
        }
    }
}
