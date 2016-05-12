using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Equi.Models;

namespace CkgDomainLogic.Equi.Contracts
{
    public interface IErweiterterBriefbestandDataService : ICkgGeneralDataService
    {
        FahrzeugbriefSuchparameter Suchparameter { get; set; }

        List<FahrzeugbriefErweitert> Fahrzeugbriefe { get; }

        void MarkForRefreshFahrzeugbriefe();

        string SaveSperrvermerk(FahrzeugbriefErweitert model);
    }
}
