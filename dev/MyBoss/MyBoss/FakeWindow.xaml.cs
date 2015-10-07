using System.Windows;
using System.Windows.Input;

namespace MyBoss
{
    /// <summary>
    /// Interaktionslogik für FakeWindow.xaml
    /// </summary>
    public partial class FakeWindow 
    {
        public FakeWindow()
        {
            InitializeComponent();
        }

        private void FakeWindowOnLoaded(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }

        private void FakeWindowOnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
    }
}
