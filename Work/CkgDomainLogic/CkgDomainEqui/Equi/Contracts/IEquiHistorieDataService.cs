using System.Collections.Generic;
using CkgDomainLogic.Archive.Models;
using CkgDomainLogic.Equi.Models;

namespace CkgDomainLogic.Equi.Contracts
{
    public interface IEquiHistorieDataService
    {
        EquiHistorieSuchparameter Suchparameter { get; set; }

        List<EquiHistorieInfo> HistorieInfos { get; }

        void MarkForRefreshHistorieInfos();

        EquiHistorie GetEquiHistorie(string fahrgestellnummer, int appId);

        List<EasyAccessArchiveDefinition> GetArchiveDefinitions();
    }
}
