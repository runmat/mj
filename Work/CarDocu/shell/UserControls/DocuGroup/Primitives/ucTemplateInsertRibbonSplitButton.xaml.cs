using System.Windows;
using Fluent;

namespace CarDocu.UserControls.DocuGroup
{
    /// <summary>
    /// Interaktionslogik für ucPdfRibbonSplitButton.xaml
    /// </summary>
    public partial class ucTemplateInsertRibbonSplitButton 
    {
        public ucTemplateInsertRibbonSplitButton()
        {
            InitializeComponent();
        }

        private void TemplateDropDownClick(object sender, RoutedEventArgs e)
        {
            ((SplitButton) sender).IsDropDownOpen = !((SplitButton) sender).IsDropDownOpen;
        }
    }
}
