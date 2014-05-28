using System.Windows;
using System.Windows.Controls;
using Fluent;

namespace CarDocu.UserControls.DocuGroup
{
    /// <summary>
    /// Interaktionslogik für ucPdfRibbonSplitButton.xaml
    /// </summary>
    public partial class ucPdfRibbonSplitButton : UserControl
    {
        public ucPdfRibbonSplitButton()
        {
            InitializeComponent();
        }

        private void PdfDropDownClick(object sender, RoutedEventArgs e)
        {
            ((SplitButton) sender).IsDropDownOpen = !((SplitButton) sender).IsDropDownOpen;
        }
    }
}
