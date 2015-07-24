using System.Collections.Generic;
using CkgDomainLogic.Fahrzeuge.Models;

namespace CkgDomainLogic.Fahrzeuge.Contracts
{
    public interface IFahrzeugzulaeufeDataService
    {
        FahrzeugzulaeufeSelektor Suchparameter { get; set; }

        List<Fahrzeuge.Models.Hersteller> HerstellerListe { get; }

        List<Fahrzeugzulauf> Fahrzeugzulaeufe { get; }

        void MarkForRefreshFahrzeugzulaeufe();

        void MarkForRefreshHerstellerListe();
    }
}
