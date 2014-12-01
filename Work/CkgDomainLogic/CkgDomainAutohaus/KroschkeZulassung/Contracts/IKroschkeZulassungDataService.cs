using System.Collections.Generic;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.KroschkeZulassung.Models;

namespace CkgDomainLogic.KroschkeZulassung.Contracts
{
    public interface IKroschkeZulassungDataService : ICkgGeneralDataService
    {
        Vorgang Zulassung { get; set; }

        List<Kunde> Kunden { get; }

        List<Domaenenfestwert> Fahrzeugarten { get; }

        List<Material> Zulassungsarten { get; }

        List<Zusatzdienstleistung> Zusatzdienstleistungen { get; }

        List<Kennzeichengroesse> Kennzeichengroessen { get; }

        void MarkForRefresh();

        Bankdaten GetBankdaten(string iban);

        string GetZulassungskreis();

        string SaveZulassung(bool saveDataInSap, bool mitKundenformular);
    }
}
