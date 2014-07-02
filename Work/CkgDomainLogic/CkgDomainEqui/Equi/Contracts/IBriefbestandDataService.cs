using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Equi.Models;

namespace CkgDomainLogic.Equi.Contracts
{
    public interface IBriefbestandDataService : ICkgGeneralDataService
    {
        FahrzeugbriefFilter DatenFilter { get; set; }

        List<Fahrzeugbrief> Fahrzeugbriefe { get; }

        void MarkForRefreshFahrzeugbriefe();
    }
}
