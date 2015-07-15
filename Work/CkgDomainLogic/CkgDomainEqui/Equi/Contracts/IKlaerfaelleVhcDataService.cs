using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Equi.Models;

namespace CkgDomainLogic.Equi.Contracts
{
    public interface IKlaerfaelleVhcDataService : ICkgGeneralDataService
    {
        KlaerfaelleVhcSuchparameter Suchparameter { get; set; }

        List<KlaerfallVhc> KlaerfaelleVhc { get; }

        void MarkForRefreshKlaerfaelleVhc();
    }
}
