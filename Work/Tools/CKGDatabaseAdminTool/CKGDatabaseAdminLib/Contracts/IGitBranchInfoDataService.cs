using System.Collections.Generic;
using System.Collections.ObjectModel;
using CKGDatabaseAdminLib.Models;
using CkgDomainLogic.General.Contracts;

namespace CKGDatabaseAdminLib.Contracts
{
    public interface IGitBranchInfoDataService : ICkgGeneralDataService
    {
        GitBranchViewFilter AnzeigeFilter { get; set; }

        ObservableCollection<CkgEntwickler> CkgEntwickler { get; }

        ObservableCollection<GitBranchInfo> GitBranches { get; }

        string SaveChanges();

        void ReloadData(string connectionName);

        void FilterGitBranches();

        List<GitBranchInfo> GetBranchesForTransportMail();
    }
}