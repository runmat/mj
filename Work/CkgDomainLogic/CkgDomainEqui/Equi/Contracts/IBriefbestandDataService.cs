using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Equi.Models;

namespace CkgDomainLogic.Equi.Contracts
{
    public interface IBriefbestandDataService : ICkgGeneralDataService
    {
        FahrzeugbriefBestandFilter DatenFilter { get; set; }

        List<FahrzeugbriefBestand> Fahrzeugbriefe { get; }

        void MarkForRefreshFahrzeugbriefe();
    }
}
