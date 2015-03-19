using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Equi.Models;

namespace CkgDomainLogic.Equi.Contracts
{
    public interface IKlaerfaelleVhcDataService : ICkgGeneralDataService
    {
        List<KlaerfallVhc> KlaerfaelleVhc { get; }

        void MarkForRefreshKlaerfaelleVhc();
    }
}
