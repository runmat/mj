using System.Collections.Generic;
using CkgDomainLogic.AppUserOverview.Models;
using CkgDomainLogic.General.Contracts;
using ServicesMvc.AppUserOverview.Models;

namespace CkgDomainLogic.AppUserOverview.Contracts
{
    public interface IAppBatchZuordnungDataService  : ICkgGeneralDataService
    {
        List<AppZuordnung> GetAppZuordnungen(AppBatchZuordnungSelektor selektor);

        bool SaveChangedAppZuordnungen(List<AppZuordnung> zuordnungen);
    }
}
