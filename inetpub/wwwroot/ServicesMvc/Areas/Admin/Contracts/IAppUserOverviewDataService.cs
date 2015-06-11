using System.Collections.Generic;
using CkgDomainLogic.AppUserOverview.Models;
using CkgDomainLogic.General.Contracts;
using ServicesMvc.AppUserOverview.Models;
using GeneralTools.Contracts;

namespace CkgDomainLogic.AppUserOverview.Contracts
{
    public interface IAppUserOverviewDataService  : ICkgGeneralDataService
    {
        List<AppUserOverviewResults> GetGridData(AppUserOverviewSelektor selector, ILogonContextDataService logonContext);

    }
}
