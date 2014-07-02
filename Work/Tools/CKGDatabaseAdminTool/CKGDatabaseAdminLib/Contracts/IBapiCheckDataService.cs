using System.Collections.Generic;
using System.Collections.ObjectModel;
using CKGDatabaseAdminLib.Models;
using CkgDomainLogic.General.Contracts;

namespace CKGDatabaseAdminLib.Contracts
{
    public interface IBapiCheckDataService : ICkgGeneralDataService
    {
        List<BapiCheckResult> BapiCheckResults { get; }

        void InitDataContext(string connectionName);

        void PerformBapiCheck();
    }
}