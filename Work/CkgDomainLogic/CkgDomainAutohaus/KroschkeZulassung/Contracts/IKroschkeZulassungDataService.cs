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

        string CheckIban();

        string GetZulassungskreis();

        string SaveZulassung(bool simulation);
    }
}
