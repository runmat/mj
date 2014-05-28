using System.Windows.Controls;

namespace CarDocu.UserControls.AdminGroup
{
    /// <summary>
    /// Interaktionslogik für ucDocTypes.xaml
    /// </summary>
    public partial class ucDomainLocations 
    {
        public ucDomainLocations()
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
