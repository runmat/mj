using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CKGDatabaseAdminLib.ViewModels;
using DevExpress.Xpf.Grid;
using Microsoft.Win32;

namespace CKGDatabaseAdminTool.UserControls
{
    /// <summary>
    /// Interaktionslogik für ucManageGitBranchesDevX.xaml
    /// </summary>
    public partial class ucManageGitBranchesDevX 
    {
        public ucManageGitBranchesDevX()
        {
            InitializeComponent();
            DataControlBase.AllowInfiniteGridSize = true;
            ((TableView)Control.View).AutoWidth = true;

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
                    //((TableView)Control.View).ExportToXls(sfd.FileName);
            }
        }

        private void Control_OnLoaded(object sender, RoutedEventArgs e)
        {
            ((TableView)Control.View).BestFitColumn(Control.Columns["Bemerkung"]);
        }
    }
}
