using System;
using System.Collections.Generic;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Autohaus.Models;

namespace CkgDomainLogic.Autohaus.Contracts
{
    public interface IZulassungDataService : ICkgGeneralDataService
    {
        List<Kunde> Kunden { get; }

        bool WarenkorbNurEigeneAuftraege { get; }

        List<Kunde> KundenauswahlWarenkorb { get; }

        List<Domaenenfestwert> Fahrzeugarten { get; }

        List<Zusatzdienstleistung> Zusatzdienstleistungen { get; }

        List<Kennzeichengroesse> Kennzeichengroessen { get; }

        void MarkForRefresh();

        Bankdaten GetBankdaten(string iban);

        void GetZulassungskreisUndKennzeichen(Vorgang zulassung, out string kreis, out string kennzeichen);
        
        void GetZulassungsKennzeichen(string kreis, out string kennzeichen);

        Adresse GetLieferantZuKreis(string kreis);

        string GetZulassungsstelleWkzUrl(string kreis);

        string Check48hExpress(Vorgang zulassung);

        string SaveZulassungen(List<Vorgang> zulassungen, bool saveDataToSap, bool saveFromShoppingCart);

        List<Vorgang> LoadVorgaengeForShoppingCart(List<string> kundenNummern);

        string DeleteVorgangFromShoppingCart(string belegNr);

        List<Domaenenfestwert> GetFahrzeugfarben { get; }

        List<Material> GetZulassungsAbmeldeArten(string kreis, bool zulassungsartenAutomatischErmitteln, bool sonderzulassung);

        #region Zulassungs Report

        List<ZulassungsReportModel> GetZulassungsReportItems(ZulassungsReportSelektor selector, List<Kunde> kunden, Action<string, string> addModelError);

        #endregion

        #region Dokumentencenter Formulare

        List<Zulassungskreis> Zulassungskreise { get; }

        List<PdfFormular> GetFormulare(FormulareSelektor selector, Action<string, string> addModelError);

        ZiPoolDaten GetZiPoolDaten(string kreis, Action<string, string> addModelError);

        #endregion
    }
}
