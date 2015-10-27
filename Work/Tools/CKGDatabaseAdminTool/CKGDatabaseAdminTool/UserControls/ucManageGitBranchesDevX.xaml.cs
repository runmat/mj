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
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;
using CKGDatabaseAdminLib.Models;
using GeneralTools.Models;

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
            ((TableView) Control.View).NewItemRowPosition = NewItemRowPosition.Top;
            Control.Columns[0].GroupIndex = 1;

            ((TableView) Control.View).ShowGroupedColumns = true;

            TryExpandGroupRowFoarActiveDeveloper();
        }

        void TryExpandGroupRowFoarActiveDeveloper() 
        {
            if (MainViewModel.Instance.Developer.IsNullOrEmpty())
                return;

            for (int i = 0; i < Control.VisibleRowCount; i++)
            {
                int rowHandle = Control.GetRowHandleByVisibleIndex(i);
                if (!Control.IsGroupRowHandle(rowHandle))
                    continue;

                var gitBranch = Control.GetRow(rowHandle) as GitBranchInfo;
                if (gitBranch == null)
                    continue;

                if (gitBranch.Entwickler.ToLower() == MainViewModel.Instance.Developer.ToLower())
                    Control.ExpandGroupRow(rowHandle);
            }
        }

        private void GridTableViewKeyUp(object sender, KeyEventArgs e)
        {
            var view = (TableView)sender;

            if (view.IsEditing == false && e.Key == Key.Delete)
                view.DeleteRow(view.FocusedRowHandle);

            if (e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
                (view.DataContext as GitBranchInfoViewModel).SaveGitBranchInfos(null);
        }

        private void GridTableViewCellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            var view = (TableView)sender;

            view.PostEditor();
        }
    }

    public class GitBranchEntwicklerCellTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DefaultTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var cellData = (item as EditGridCellData);
            if (MainViewModel.Instance.Developer.IsNullOrEmpty())
                return base.SelectTemplate(item, container);

            if (cellData == null || cellData.Value == null || MainViewModel.Instance.Developer.ToLower() == cellData.Value.ToString().NotNullOrEmpty().ToLower())
                return DefaultTemplate;

            return base.SelectTemplate(item, container);
        }
    }
}
