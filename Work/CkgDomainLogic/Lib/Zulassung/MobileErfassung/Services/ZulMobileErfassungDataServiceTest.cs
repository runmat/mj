using System.Collections.Generic;
using System;
using CkgDomainLogic.Zulassung.MobileErfassung.Contracts;
using CkgDomainLogic.Zulassung.MobileErfassung.Models;
using GeneralTools.Contracts;

namespace CkgDomainLogic.Zulassung.MobileErfassung.Services
{
    public class ZulMobileErfassungDataServiceTest : IZulMobileErfassungDataService
    {
        public ILogonContext LogonContext { get; private set; }

        public ZulMobileErfassungDataServiceTest(ILogonContext logonContext)
        {
            LogonContext = logonContext;
        }

        public List<Anwendung> GetAnwendungen()
        {
            List<Anwendung> liste = new List<Anwendung> { new Anwendung("ZLD-Vorgänge bearbeiten", "EditZLDVorgaenge", "ErfassungMobil") };

            return liste;
        }

        public Stammdatencontainer GetStammdaten()
        {
            Stammdatencontainer stda = new Stammdatencontainer();

            return stda;
        }

        public void GetAemterMitVorgaengen(out List<AmtVorgaenge> aemterMitVorgaengen, out List<Vorgang> vorgaenge)
        {
            // Ämter
            List<AmtVorgaenge> aemter = new List<AmtVorgaenge>();

            aemterMitVorgaengen = aemter;

            // Vorgänge
            List<Vorgang> vgs = new List<Vorgang>();

            vorgaenge = vgs;
        }

        public string SaveVorgaenge(List<Vorgang> vorgaenge)
        {
            return "";
        }

        public List<VorgangStatus> GetVorgangBebStatus(List<string> vorgIds)
        {
            throw new NotImplementedException();
        }

        public List<string> GetVkBueros()
        {
            throw new NotImplementedException();
        }

        public void GetStammdatenKundenUndHauptdienstleistungen(string vkBur, out List<Kunde> kunden, out List<Dienstleistung> dienstleistungen)
        {
            throw new NotImplementedException();
        }

        public List<Amt> GetStammdatenAemter()
        {
            throw new NotImplementedException();
        }
    }
}
