using System.Collections.Generic;
using CkgDomainLogic.Zulassung.MobileErfassung.Models;

namespace CkgDomainLogic.Zulassung.MobileErfassung.Contracts
{
    public interface IZulMobileErfassungDataService
    {
        List<Anwendung> GetAnwendungen();

        Stammdatencontainer GetStammdaten();

        void GetAemterMitVorgaengen(out List<AmtVorgaenge> aemterMitVorgaengen, out List<Vorgang> vorgaenge);

        string SaveVorgaenge(List<Vorgang> vorgaenge);

        List<VorgangStatus> GetVorgangBebStatus(List<string> vorgIds);

        List<string> GetVkBueros();

        void GetStammdatenKundenUndHauptdienstleistungen(string vkBur, out List<Kunde> kunden, out List<Dienstleistung> dienstleistungen);

        List<Amt> GetStammdatenAemter();
    }
}
