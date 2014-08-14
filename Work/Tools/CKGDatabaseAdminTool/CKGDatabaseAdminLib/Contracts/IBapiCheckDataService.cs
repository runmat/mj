using System.Collections.Generic;
using System.Collections.ObjectModel;
using CKGDatabaseAdminLib.Models;
using CkgDomainLogic.General.Contracts;

namespace CKGDatabaseAdminLib.Contracts
{
    public interface IBapiCheckDataService : ICkgGeneralDataService
    {
        List<BapiCheckAbweichung> BapiCheckAbweichungen { get; }

        void InitDataContext(string connectionName);

        string PerformBapiCheck();
    }
}