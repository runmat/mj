using System.Collections.ObjectModel;
using CKGDatabaseAdminLib.Models;
using CkgDomainLogic.General.Contracts;

namespace CKGDatabaseAdminLib.Contracts
{
    public interface IBapiApplicationDataService : ICkgGeneralDataService
    {
        ObservableCollection<Application> Applications { get; }

        ObservableCollection<BapiTable> Bapis { get; }

        ObservableCollection<Application> BapiApplications { get; }

        void InitDataContext(string connectionName);

        void GetBapiUsage(int bapiId);

        void ResetCurrentBapiId();
    }
}