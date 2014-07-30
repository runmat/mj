using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Equi.Models;

namespace CkgDomainLogic.Equi.Contracts
{
    public interface IDatenOhneDokumenteDataService : ICkgGeneralDataService
    {
        DatenOhneDokumenteFilter DatenFilter { get; set; }

        List<DatenOhneDokumente> DatenOhneDokumente { get; }

        void MarkForRefreshDatenOhneDokumente();

        List<DatenOhneDokumente> SaveDatenOhneDokumente(List<DatenOhneDokumente> vorgaenge, ref string message);
    }
}
