using System.Windows.Controls;
using CKGDatabaseAdminLib.ViewModels;

namespace CKGDatabaseAdminTool.UserControls
{
    /// <summary>
    /// Interaktionslogik für ucCopyFieldTranslations.xaml
    /// </summary>
    public partial class ucCopyFieldTranslations : UserControl
    {
        public ucCopyFieldTranslations()
        {
            InitializeComponent();
        }

        private void dgApplications_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var fieldTranslationCopyViewModel = DataContext as FieldTranslationCopyViewModel;
            if (fieldTranslationCopyViewModel != null)
            {
                fieldTranslationCopyViewModel.dgApplications_OnSelectionChanged(e);
            } 
        }

        private void OtherDbConnections_Filter(object sender, System.Windows.Data.FilterEventArgs e)
        {
            e.Accepted = ((e.Item as string) != (DataContext as FieldTranslationCopyViewModel).Parent.ActualDatabase);
        }
    }
}
