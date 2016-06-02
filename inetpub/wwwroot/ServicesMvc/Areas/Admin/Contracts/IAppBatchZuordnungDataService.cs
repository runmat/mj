using System.Collections.Generic;
using CkgDomainLogic.Admin.Models;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Database.Models;

namespace CkgDomainLogic.Admin.Contracts
{
    public interface IAppBatchZuordnungDataService  : ICkgGeneralDataService
    {
        List<Application> Applications { get; }

        void InitSqlData();

        List<AppZuordnung> GetAppZuordnungen(int appId);

        string SaveAppZuordnungen(List<AppZuordnung> zuordnungen);
    }
}
