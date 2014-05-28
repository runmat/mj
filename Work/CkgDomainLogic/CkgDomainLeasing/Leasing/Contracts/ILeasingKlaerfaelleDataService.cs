using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Leasing.Models;

namespace CkgDomainLogic.Leasing.Contracts
{
    public interface ILeasingKlaerfaelleDataService : ICkgGeneralDataService
    {
        KlaerfallSuchparameter Suchparameter { get; set; }
        List<Klaerfall> Klaerfaelle { get; }

        void MarkForRefreshKlaerfaelle();
    }
}
