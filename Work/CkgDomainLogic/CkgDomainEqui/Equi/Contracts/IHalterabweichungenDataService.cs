using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Equi.Models;

namespace CkgDomainLogic.Equi.Contracts
{
    public interface IHalterabweichungenDataService : ICkgGeneralDataService
    {
        List<Halterabweichung> Halterabweichungen { get; }

        void MarkForRefreshHalterabweichungen();

        List<Halterabweichung> SaveHalterabweichungen(List<Halterabweichung> vorgaenge, ref string message);
    }
}
