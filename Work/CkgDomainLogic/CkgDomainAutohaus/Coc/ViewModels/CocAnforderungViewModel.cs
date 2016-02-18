using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Autohaus.Contracts;
using CkgDomainLogic.Autohaus.Models;
using CkgDomainLogic.DomainCommon.Models;
using GeneralTools.Models;
using WebTools.Services;

namespace CkgDomainLogic.Autohaus.ViewModels
{
    [SuppressMessage("ReSharper", "ConvertClosureToMethodGroup")]
    public class CocAnforderungViewModel : CkgBaseViewModel
    {
        [XmlIgnore, ScriptIgnore]
        public IZulassungDataService ZulassungDataService
        {
            get { return CacheGet<IZulassungDataService>(); }
        }

        [XmlIgnore, ScriptIgnore]
        public ICocAnforderungDataService CocAnforderungDataService
        {
            get { return CacheGet<ICocAnforderungDataService>(); }
        }

        [XmlIgnore, ScriptIgnore]
        public List<Domaenenfestwert> Fahrzeugarten { get { return ZulassungDataService.Fahrzeugarten.Where(f => !string.IsNullOrEmpty(f.Wert)).OrderBy(f => f.Wert).ToList(); } }

        [XmlIgnore, ScriptIgnore]
        public List<Hersteller> Hersteller
        {
            get
            {
                return CocAnforderungDataService.HerstellerGesamtliste
                    .Concat(new List<Hersteller> { new Hersteller { Code = "", Name = Localize.DropdownDefaultOptionPleaseChoose } }).OrderBy(f => f.Name).ToList();
            }
        }

        [XmlIgnore, ScriptIgnore]
        public List<Land> Laender { get { return ZulassungDataService.Laender.Where(l => !string.IsNullOrEmpty(l.ID)).OrderByDescending(l => l.ID == "DE").ThenBy(l => l.ID).ToList(); } }

        [XmlIgnore, ScriptIgnore]
        public List<SelectItem> Jahre { get { return PropertyCacheGet(() => GetJahreSelectList()); } }

        public CocAnforderung CocAnforderung
        {
            get { return PropertyCacheGet(() => new CocAnforderung()); }
            set { PropertyCacheSet(value); }
        }


        public void DataInit()
        {
            DataMarkForRefresh();

            CocAnforderung = new CocAnforderung
            {
                Vorname = LogonContext.FirstName,
                Nachname = LogonContext.LastName,
                Email = LogonContext.UserInfo.Mail,
                TelefonNr = LogonContext.UserInfo.Telephone2
            };
        }

        public void DataMarkForRefresh()
        {
            ZulassungDataService.MarkForRefresh();
        }

        private static List<SelectItem> GetJahreSelectList()
        {
            var liste = new List<SelectItem> { new SelectItem("", Localize.DropdownDefaultOptionPleaseChoose) };

            for (var jahr = DateTime.Now.Year; jahr > 1995; jahr--)
            {
                liste.Add(new SelectItem(jahr.ToString(), jahr.ToString()));
            }

            return liste;
        }

        public void ApplyKopfdaten(CocAnforderungKopfdaten model)
        {
            CocAnforderung.Kopfdaten = model;
        }

        private void ApplyDetails(CocAnforderung model)
        {
            CocAnforderung.JahrDerErstzulassung = model.JahrDerErstzulassung;
            CocAnforderung.FahrgestellNr = model.FahrgestellNr;
            CocAnforderung.Vorname = model.Vorname;
            CocAnforderung.Nachname = model.Nachname;
            CocAnforderung.Email = model.Email;
            CocAnforderung.TelefonNr = model.TelefonNr;
        }

        public void CocAnforderungAbsenden(CocAnforderung model, Action<string, string> addModelError)
        {
            ApplyDetails(model);

            var empfaengerEmail = CocAnforderungDataService.GetEmpfaengerEmailAdresse();

            if (string.IsNullOrEmpty(empfaengerEmail))
            {
                addModelError("", string.Format("{0}: {1}", Localize.ErrorMailCouldNotBeSent, Localize.NoEmailAddressFoundInCusomizing));
                return;
            }

            var mailService = new SmtpMailService(AppSettings);

            var mailText = "Folgende CoC-Bestellung wurde eben im Portal Kroschke ON aufgegeben:<br/>";
            mailText += "<br/>";
            mailText += "Fahrzeugdaten:<br/>";
            mailText += string.Format("{0}: {1}<br/>", Localize.VehicleType, CocAnforderung.Kopfdaten.FahrzeugTypBezeichnung);
            mailText += string.Format("{0}: {1}<br/>", Localize.Manufacturer, CocAnforderung.Kopfdaten.HerstellerBezeichnung);
            mailText += string.Format("{0}: {1}<br/>", Localize.CountryOfFirstRegistration, CocAnforderung.Kopfdaten.LandDerErstenZulassungBezeichnung);
            mailText += string.Format("{0}: {1}<br/>", Localize.YearOfFirstRegistration, CocAnforderung.JahrDerErstzulassung);
            mailText += "<br/>";
            mailText += "Kundendaten:<br/>";
            mailText += string.Format("{0}: {1}<br/>", Localize.Company, LogonContext.CustomerName);
            mailText += string.Format("{0}: {1} {2}<br/>", Localize.Name, CocAnforderung.Vorname, CocAnforderung.Nachname);
            mailText += string.Format("{0}: {1}<br/>", Localize.Email, CocAnforderung.Email);
            mailText += string.Format("{0}: {1}<br/>", Localize.PhoneNo, CocAnforderung.TelefonNr);

            if (!mailService.SendMail(empfaengerEmail, "CoC-Bestellung", mailText))
                addModelError("", Localize.ErrorMailCouldNotBeSent);
        }
    }
}
