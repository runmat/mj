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

        string GetZulassungskreis(Vorgang zulassung);

        string SaveZulassungen(List<Vorgang> zulassungen, bool saveDataToSap, bool saveFromShoppingCart);


        #region Zulassungs Report

        List<ZulassungsReportModel> GetZulassungsReportItems(ZulassungsReportSelektor selector);

        #endregion
    }
}
