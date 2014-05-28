using System.Windows.Controls;
using CKGDatabaseAdminLib.ViewModels;

namespace CKGDatabaseAdminTool.UserControls
{
    /// <summary>
    /// Interaktionslogik für ucCopyApplications.xaml
    /// </summary>
    public partial class ucCopyApplications : UserControl
    {
        public ucCopyApplications()
        {
            InitializeComponent();
        }

        private void dgApplications_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var applicationCopyViewModel = DataContext as ApplicationCopyViewModel;
            if (applicationCopyViewModel != null)
            {
                applicationCopyViewModel.dgApplications_OnSelectionChanged(e);
            }
        }

        private void OtherDbConnections_Filter(object sender, System.Windows.Data.FilterEventArgs e)
        {
            e.Accepted = ((e.Item as string) != (DataContext as ApplicationCopyViewModel).Parent.ActualDatabase);
        }
    }
}
