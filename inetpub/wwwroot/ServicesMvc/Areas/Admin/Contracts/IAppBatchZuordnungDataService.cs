using System.Collections.Generic;
using CkgDomainLogic.Admin.Models;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Database.Models;

namespace CkgDomainLogic.Admin.Contracts
{
    public interface IAppBatchZuordnungDataService  : ICkgGeneralDataService
    {
        List<Application> GetApplications();

        List<AppZuordnung> GetAppZuordnungen(AppBatchZuordnungSelektor selektor);

        string SaveAppZuordnungen(List<AppZuordnung> zuordnungen);
    }
}
