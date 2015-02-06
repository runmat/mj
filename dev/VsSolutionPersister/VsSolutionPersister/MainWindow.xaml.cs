using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VsSolutionPersister
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel();
        }

        private void UIElement_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var s = e.Source;
            if (e.OriginalSource is Image)
            {
                e.Handled = true;

                var image = (e.OriginalSource as Image);
                if (image.Parent is Button)
                {
                    var button = (image.Parent as Button);
                    button.Command.Execute(button.CommandParameter);
                }
            }
        }

        private void Window_Deactivated(object sender, System.EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
