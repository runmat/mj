using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Equi.Models;

namespace CkgDomainLogic.Equi.Contracts
{
    public enum BriefversandModus { Brief, Schluessel, BriefMitSchluessel, Stueckliste }

    public interface IBriefbestandDataService : ICkgGeneralDataService
    {
        #region Briefbestand

        FahrzeugbriefFilter DatenFilter { get; set; }

        List<Fahrzeugbrief> FahrzeugbriefeBestand { get; }

        BriefversandModus FahrzeugbriefeZumVersandModus { get; set; }
        List<Fahrzeugbrief> FahrzeugbriefeZumVersand { get; }

        void MarkForRefreshFahrzeugbriefe();

        #endregion


        #region VersandBeauftragungen

        List<Fahrzeugbrief> GetVersandBeauftragungen(VersandBeauftragungSelektor model);

        string DeleteVersandBeauftragungen(string fin, string kennzeichen);

        #endregion


        #region ZBII Ein- und Ausgaenge

        List<Fahrzeugbrief> GetEinAusgaenge(EinAusgangSelektor model);

        #endregion


        #region Fahrzeugbriefe

        IEnumerable<Fahrzeugbrief> GetFahrzeugBriefe(Fahrzeugbrief fahrzeug);

        #endregion


        #region Stuecklisten

        IEnumerable<StuecklistenKomponente> GetStuecklistenKomponenten(IEnumerable<string> fahrgestellnummern);

        List<Stuecklisten> GetStuecklisten(string equinummer);

        #endregion
    }
}
