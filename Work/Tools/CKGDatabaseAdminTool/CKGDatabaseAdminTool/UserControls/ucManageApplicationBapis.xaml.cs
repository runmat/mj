using System.Windows.Controls;
using CKGDatabaseAdminLib.ViewModels;

namespace CKGDatabaseAdminTool.UserControls
{
    /// <summary>
    /// Interaktionslogik für ucManageApplicationBapis.xaml
    /// </summary>
    public partial class ucManageApplicationBapis : UserControl
    {
        public ucManageApplicationBapis()
        {
            InitializeComponent();
        }

        private void dgApplications_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var applicationBapiViewModel = DataContext as ApplicationBapiViewModel;
            if (applicationBapiViewModel != null)
            {
                applicationBapiViewModel.dgApplications_OnSelectionChanged(e);
            } 
        }
    }
}
