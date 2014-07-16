using System.Collections.ObjectModel;
using CKGDatabaseAdminLib.Models.DbModels;
using CkgDomainLogic.General.Contracts;

namespace CKGDatabaseAdminLib.Contracts
{
    public interface IGitBranchInfoDataService : ICkgGeneralDataService
    {
        ObservableCollection<GitBranchInfo> GitBranches { get; }

        string SaveChanges();

        void InitDataContext(string connectionName);
    }
}