using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using Application = System.Windows.Application;
using Button = System.Windows.Controls.Button;
using TextBox = System.Windows.Controls.TextBox;

namespace VsSolutionPersister
{
    public partial class MainWindow
    {
        MainViewModel ViewModel { get { return (MainViewModel) DataContext; } }

        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel();

            var timer = new System.Windows.Forms.Timer { Interval = 200 };
            timer.Tick += (sender, args) =>
            {
                timer.Stop();
                timer.Dispose();

                Topmost = false;
                WindowState = WindowState.Minimized;
                WindowState = WindowState.Normal;
            };
            timer.Start();
        }

        private void UIElement_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!(e.OriginalSource is Image))
            {
                if (!ViewModel.AllowSelectionChange())
                    e.Handled = true;

                return;
            }

            e.Handled = true;
            var image = (e.OriginalSource as Image);
            if (image.Parent is Button)
            {
                var button = (image.Parent as Button);
                button.Command.Execute(button.CommandParameter);
            }
        }

        private void WindowDeactivated(object sender, System.EventArgs e)
        {
            //if (ViewModel.IsClosable)
            //    Application.Current.Shutdown();
        }

        private void UIElement_OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(sender is TextBox))
                return;

            ((TextBox) sender).Focus();
        }
    }
}
