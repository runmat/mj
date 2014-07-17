using System.Collections.ObjectModel;
using CKGDatabaseAdminLib.Models.DbModels;
using CkgDomainLogic.General.Contracts;

namespace CKGDatabaseAdminLib.Contracts
{
    public interface IGitBranchInfoDataService : ICkgGeneralDataService
    {
        GitBranchViewFilter AnzeigeFilter { get; set; }

        ObservableCollection<GitBranchInfo> GitBranchesFiltered { get; }

        string SaveChanges();

        void ReloadData(string connectionName);

        void FilterGitBranches();
    }
}