using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VsSolutionPersister
{
    public partial class MainWindow
    {
        MainViewModel ViewModel { get { return (MainViewModel) DataContext; } }

        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel();
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
            if (ViewModel.IsClosable)
                Application.Current.Shutdown();
        }
    }
}
