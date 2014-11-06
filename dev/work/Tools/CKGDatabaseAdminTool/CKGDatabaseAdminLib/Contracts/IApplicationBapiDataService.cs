using System.Collections.ObjectModel;
using CKGDatabaseAdminLib.Models;
using CkgDomainLogic.General.Contracts;

namespace CKGDatabaseAdminLib.Contracts
{
    public interface IApplicationBapiDataService : ICkgGeneralDataService
    {
        ObservableCollection<ApplicationInfo> Applications { get; }

        ObservableCollection<BapiTable> Bapis { get; }

        ObservableCollection<BapiTable> ApplicationBapis { get; }

        bool IsInEditMode { get; }

        void InitDataContext(string connectionName);

        int AddBapi(string name);

        void BeginEdit(int appId);

        void AddApplicationBapi(int bapiId);

        void DeleteApplicationBapi(int bapiId);

        void ResetCurrentAppId();
    }
}