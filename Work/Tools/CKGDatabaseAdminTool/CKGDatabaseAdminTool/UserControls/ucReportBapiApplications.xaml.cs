using System.Windows.Controls;
using CKGDatabaseAdminLib.ViewModels;

namespace CKGDatabaseAdminTool.UserControls
{
    /// <summary>
    /// Interaktionslogik für ucReportBapiApplications.xaml
    /// </summary>
    public partial class ucReportBapiApplications : UserControl
    {
        public ucReportBapiApplications()
        {
            InitializeComponent();
        }

        private void dgBapis_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var bapiApplicationViewModel = DataContext as BapiApplicationViewModel;
            if (bapiApplicationViewModel != null)
            {
                bapiApplicationViewModel.dgBapis_OnSelectionChanged(e);
            } 
        }
    }
}
