using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Equi.Models;

namespace CkgDomainLogic.Equi.Contracts
{
    public interface IBriefbestandVhcDataService : ICkgGeneralDataService
    {
        List<FahrzeugbriefVhc> FahrzeugbriefeVhc { get; }

        void MarkForRefreshFahrzeugbriefeVhc();
    }
}
