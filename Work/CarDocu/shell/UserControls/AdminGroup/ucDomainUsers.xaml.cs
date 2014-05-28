using System.Windows.Controls;

namespace CarDocu.UserControls.AdminGroup
{
    /// <summary>
    /// Interaktionslogik für ucDomainUsers.xaml
    /// </summary>
    public partial class ucDomainUsers 
    {
        public ucDomainUsers()
        {
            InitializeComponent();
        }

        private void TextBoxIsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (((TextBox)sender).IsVisible)
                ((TextBox)sender).Focus();
        }
    }
}
