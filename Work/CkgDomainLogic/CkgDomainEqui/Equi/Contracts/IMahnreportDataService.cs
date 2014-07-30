using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Equi.Models;

namespace CkgDomainLogic.Equi.Contracts
{
    public interface IMahnreportDataService : ICkgGeneralDataService
    {
        List<EquiMahn> Fahrzeuge { get; }

        void MarkForRefreshFahrzeuge();
    }
}
