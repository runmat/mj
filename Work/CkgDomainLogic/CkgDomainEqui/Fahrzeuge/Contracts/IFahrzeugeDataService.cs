using System;
using System.Collections.Generic;
using CkgDomainLogic.DomainCommon.Models;
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

        void UnfallmeldungenCancel(List<Unfallmeldung> list, string cancelText, out int cancelCount, out string errorMessage);

        void MeldungCreateTryLoadEqui(ref Unfallmeldung model, out string errorMessage);
        void MeldungCreate(Unfallmeldung model, out string errorMessage);

        List<Adresse> GetStationCodes();

        List<Domaenenfestwert> GetFarben();

        List<Fzg> GetFahrzeugeForZulassung();

        List<KennzeichenSerie> GetKennzeichenSerie();

        List<Fzg> GetZulassungenAnzahlForPdiAndDate(DateTime date, out string errorMessage);

        string ZulassungSave(List<Fzg> fahrzeuge, DateTime zulassungsDatum, string kennzeichenSerie);

        List<FloorcheckHaendler> GetFloorcheckHaendler(FloorcheckHaendler haendler);

        List<Floorcheck> GetFloorchecks(string haendlerNo);
    }
}
