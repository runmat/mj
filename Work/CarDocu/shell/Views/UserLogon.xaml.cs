namespace CarDocu
{
    /// <summary>
    /// Interaktionslogik für AppSettingsInitial.xaml
    /// </summary>
    public partial class UserLogon 
    {
        public UserLogon()
        {
            InitializeComponent();

            tbUserLogon.Focus();
        }

        private void OkClick(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
