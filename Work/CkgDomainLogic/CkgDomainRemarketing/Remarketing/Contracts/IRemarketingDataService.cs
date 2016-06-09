using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Remarketing.Models;

namespace CkgDomainLogic.Remarketing.Contracts
{
    public interface IRemarketingDataService : ICkgGeneralDataService
    {
        List<Vermieter> GetVermieter();

        List<Hereinnahmecenter> GetHereinnahmecenter();
    }
}
