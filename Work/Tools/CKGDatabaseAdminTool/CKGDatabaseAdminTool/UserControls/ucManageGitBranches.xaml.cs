using System;
using System.Windows;
using System.Windows.Controls;
using CKGDatabaseAdminLib.ViewModels;
using Microsoft.Win32;

namespace CKGDatabaseAdminTool.UserControls
{
    /// <summary>
    /// Interaktionslogik für ucManageGitBranches.xaml
    /// </summary>
    public partial class ucManageGitBranches : UserControl
    {
        public ucManageGitBranches()
        {
            InitializeComponent();
        }

        private void btnExportToExcel_OnClick(object sender, RoutedEventArgs e)
        {
            var gitBranchInfoViewModel = DataContext as GitBranchInfoViewModel;
            if (gitBranchInfoViewModel != null)
            {
                var sfd = new SaveFileDialog
                    {
                        InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                        DefaultExt = ".xls",
                        FileName = "GIT-Branches.xls",
                        Filter = "Excel-Datei (.xls)|*.xls"
                    };

                if (sfd.ShowDialog() == true)
                    gitBranchInfoViewModel.ExportGitBranchInfos(sfd.FileName);
            } 
        }
    }
}
