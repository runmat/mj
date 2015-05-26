using System.Collections.Generic;
using CkgDomainLogic.Fahrzeuge.Models;
using CkgDomainLogic.General.Contracts;

namespace CkgDomainLogic.Fahrzeuge.Contracts
{
    public interface IFahrzeugeDataService : ICkgGeneralDataService 
    {
        List<AbgemeldetesFahrzeug> GetAbgemeldeteFahrzeuge(AbgemeldeteFahrzeugeSelektor selector);

        List<AbmeldeHistorie> GetAbmeldeHistorien(string fin);

        List<Zb2BestandSecurityFleet> GetZb2BestandSecurityFleet(Zb2BestandSecurityFleetSelektor selector);

        List<Fahrzeughersteller> GetFahrzeugHersteller();

        // teste 7876
        List<AbgemeldetesFahrzeug> GetAbgemeldeteFahrzeuge2(AbgemeldeteFahrzeugeSelektor selector);

        List<Treuhandbestand> GetTreuhandbestandFromSap();

        List<Unfallmeldung> GetUnfallmeldungen(UnfallmeldungenSelektor selektor);

        List<Fahrzeug> GetFahrzeugeForZulassung();

        List<KennzeichenSerie> GetKennzeichenSerie();
    }
}
