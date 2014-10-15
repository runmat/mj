using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Finance.Models;

namespace CkgDomainLogic.Finance.Contracts
{
    public interface IFinanceAnzeigePruefpunkteDataService : ICkgGeneralDataService
    {
        PruefpunktSuchparameter Suchparameter { get; set; }

        List<Pruefpunkt> Pruefpunkte { get; }

        void MarkForRefreshPruefpunkte();
    }
}
