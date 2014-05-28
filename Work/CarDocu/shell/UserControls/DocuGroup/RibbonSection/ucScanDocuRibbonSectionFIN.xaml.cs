using System.Windows;
using System.Windows.Controls;

namespace CarDocu.UserControls.DocuGroup
{
    /// <summary>
    /// Interaktionslogik für ucScanDocuRibbonSection.xaml
    /// </summary>
    public partial class ucScanDocuRibbonSectionFIN 
    {
        public ucScanDocuRibbonSectionFIN()
        {
            InitializeComponent();
        }

        private void TextBoxIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (((TextBox)sender).IsEnabled)
                ((TextBox)sender).Focus();        
        }

        //private void FirstFocusIsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        //{
        //    if (((TextBox)sender).Visibility == Visibility.Visible)
        //        ((TextBox)sender).Focus();
        //}
    }
}
