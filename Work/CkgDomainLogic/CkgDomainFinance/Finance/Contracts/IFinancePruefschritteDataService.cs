using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Finance.Models;

namespace CkgDomainLogic.Finance.Contracts
{
    public interface IFinancePruefschritteDataService : ICkgGeneralDataService
    {
        PruefschrittSuchparameter Suchparameter { get; set; }

        List<Pruefschritt> Pruefschritte { get; }

        void MarkForRefreshPruefschritte();
    }
}
