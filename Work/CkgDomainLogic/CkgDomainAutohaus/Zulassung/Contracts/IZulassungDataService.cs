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

        List<Domaenenfestwert> Fahrzeugarten { get; }

        List<Material> Zulassungsarten { get; }

        List<Material> Abmeldearten { get; }

        List<Zusatzdienstleistung> Zusatzdienstleistungen { get; }

        List<Kennzeichengroesse> Kennzeichengroessen { get; }

        void MarkForRefresh();

        Bankdaten GetBankdaten(string iban);

        void GetZulassungskreisUndKennzeichen(Vorgang zulassung, out string kreis, out string kennzeichen);
        
        void GetZulassungsKennzeichen(string kreis, out string kennzeichen);

        Adresse GetLieferantZuKreis(string kreis);

        string Check48hExpress(Vorgang zulassung);

        string SaveZulassungen(List<Vorgang> zulassungen, bool saveDataToSap, bool saveFromShoppingCart, bool modusAbmeldung, bool modusVersandzulassung);


        #region Zulassungs Report

        List<ZulassungsReportModel> GetZulassungsReportItems(ZulassungsReportSelektor selector, List<Kunde> kunden, Action<string, string> addModelError);

        #endregion

        #region Dokumentencenter Formulare

        List<Zulassungskreis> Zulassungskreise { get; }

        List<PdfFormular> GetFormulare(FormulareSelektor selector, Action<string, string> addModelError);

        #endregion
    }
}
