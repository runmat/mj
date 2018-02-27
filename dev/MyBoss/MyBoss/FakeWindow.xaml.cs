using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace MyBoss
{
    /// <summary>
    /// Interaktionslogik für FakeWindow.xaml
    /// </summary>
    public partial class FakeWindow
    {
        private readonly string _imageName;

        public FakeWindow(string imageName)
        {
            _imageName = imageName;

            InitializeComponent();

            Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), _imageName)));
        }

        private void FakeWindowOnLoaded(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }

        private void FakeWindowOnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_imageName == null || !_imageName.ToLower().Contains("lockscreen"))
            {
                LowLevelKeyboardListener.Disabled = false;
                Close();
                return;
            }

            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.LeftShift))
            {
                LowLevelKeyboardListener.Disabled = false;
                Close();
                MainWindow.StartOutlook();
            }
        }
    }
}
