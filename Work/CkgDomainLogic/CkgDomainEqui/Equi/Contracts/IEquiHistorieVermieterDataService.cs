using System.Collections.Generic;
using CkgDomainLogic.Equi.Models;

namespace CkgDomainLogic.Equi.Contracts
{
    public interface IEquiHistorieVermieterDataService
    {
        EquiHistorieSuchparameter Suchparameter { get; set; }

        List<EquiHistorieInfoVermieter> HistorieInfos { get; }

        void MarkForRefreshHistorieInfos();

        EquiHistorieVermieter GetEquiHistorie(string equiNr, string meldungsNr);

        byte[] GetHistorieAsPdf(string equiNr, string meldungsNr);
    }
}
