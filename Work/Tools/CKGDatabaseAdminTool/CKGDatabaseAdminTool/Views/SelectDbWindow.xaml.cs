using System.Windows;

namespace CKGDatabaseAdminTool
{
    /// <summary>
    /// Interaktionslogik für SelectDbWindow.xaml
    /// </summary>
    public partial class SelectDbWindow
    {
        public SelectDbWindow()
        {
            InitializeComponent();
        }

        private void btnContinue_OnClick(object sender, RoutedEventArgs e)
        {
            if (cbDatabase.SelectedValue != null)
            {
                this.DialogResult = true;
            }
        }
    }
}
