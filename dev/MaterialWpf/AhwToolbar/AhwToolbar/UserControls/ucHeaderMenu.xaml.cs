using System.Windows;
using System.Windows.Controls;

namespace AhwToolbar.UserControls
{
    /// <summary>
    /// Interaktionslogik für ucHeaderMenu.xaml
    /// </summary>
    public partial class UcHeaderMenu : UserControl
    {
        public UcHeaderMenu()
        {
            InitializeComponent();
        }

        private void OnCloseClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(0);
        }
    }
}
