using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Xml.Serialization;
using CKGDatabaseAdminLib.Contracts;
using CKGDatabaseAdminLib.Models.DbModels;
using CKGDatabaseAdminLib.Services;
using WpfTools4.Commands;
using WpfTools4.ViewModels;

namespace CKGDatabaseAdminLib.ViewModels
{
    public class GitBranchInfoViewModel : ViewModelBase
    {
        #region Properties

        public ObservableCollection<GitBranchInfo> GitBranches { get { return DataService.GitBranchesFiltered; } }

        [XmlIgnore]
        private readonly IGitBranchInfoDataService DataService;

        public MainViewModel Parent { get; set; }

        public GitBranchViewFilter AnzeigeFilter
        {
            get { return DataService.AnzeigeFilter; }
            set { DataService.AnzeigeFilter = value; SendPropertyChanged("AnzeigeFilter"); FilterGitBranches(); }
        }

        public ICommand CommandManageGitBranches { get; private set; }
        public ICommand CommandSaveGitBranchInfos { get; private set; }
        public ICommand CommandCancelGitBranchInfos { get; private set; }

        #endregion

        public GitBranchInfoViewModel(MainViewModel parentVM)
        {
            Parent = parentVM;

            DataService = new GitBranchInfoDataServiceSql(Parent.ActualDatabase);
            
            CommandManageGitBranches = new DelegateCommand(ManageGitBranches);
            CommandSaveGitBranchInfos = new DelegateCommand(SaveGitBranchInfos);
            CommandCancelGitBranchInfos = new DelegateCommand(CancelGitBranchInfos);
        }

        #region Commands

        public void ManageGitBranches(object parameter)
        {
            Parent.ActiveViewModel = this;
        }

        public void SaveGitBranchInfos(object parameter)
        {
            var ergebnis = DataService.SaveChanges();
            if (String.IsNullOrEmpty(ergebnis))
            {
                Parent.ShowMessage("Änderungen wurden gespeichert", MessageType.Success);
            }
            else
            {
                Parent.ShowMessage("Speichern nicht möglich: " + ergebnis, MessageType.Error);
            }
        }

        public void CancelGitBranchInfos(object parameter)
        {
            DataService.ReloadData(Parent.ActualDatabase);
            SendPropertyChanged("GitBranches");
            Parent.ShowMessage("Änderungen verworfen", MessageType.Success);
        }

        #endregion

        private void FilterGitBranches()
        {
            DataService.FilterGitBranches();
            SendPropertyChanged("GitBranches");
        }

    }
}
