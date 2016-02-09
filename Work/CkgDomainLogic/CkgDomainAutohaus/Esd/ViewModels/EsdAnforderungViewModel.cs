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
    public class EsdAnforderungViewModel : CkgBaseViewModel
    {
        [XmlIgnore, ScriptIgnore]
        public IZulassungDataService ZulassungDataService
        {
            get { return CacheGet<IZulassungDataService>(); }
        }

        [XmlIgnore, ScriptIgnore]
        public IEsdAnforderungDataService EsdAnforderungDataService
        {
            get { return CacheGet<IEsdAnforderungDataService>(); }
        }

        [XmlIgnore, ScriptIgnore]
        public List<Domaenenfestwert> Fahrzeugarten { get { return ZulassungDataService.Fahrzeugarten.Where(f => !string.IsNullOrEmpty(f.Wert)).OrderBy(f => f.Wert).ToList(); } }

        [XmlIgnore, ScriptIgnore]
        public List<Hersteller> Hersteller
        {
            get
            {
                return EsdAnforderungDataService.HerstellerGesamtliste
                    .Concat(new List<Hersteller> { new Hersteller { Code = "", Name = Localize.DropdownDefaultOptionPleaseChoose } }).OrderBy(f => f.Name).ToList();
            }
        }

        [XmlIgnore, ScriptIgnore]
        public List<Land> LaenderAuswahlliste
        {
            get
            {
                return EsdAnforderungDataService.LaenderAuswahlliste
                        .Where(l => !string.IsNullOrEmpty(l.ID))
                        .OrderByDescending(l => l.ID == "DE").ThenBy(l => l.Name.IsNullOrEmpty() ? "ZZ" : l.Name)
                        .Concat(new List<Land> { new Land { ID = "", Name = Localize.DropdownDefaultOptionOther } })
                        .ToList();
            }
        }

        [XmlIgnore, ScriptIgnore]
        public List<SelectItem> Jahre { get { return PropertyCacheGet(() => GetJahreSelectList()); } }

        public EsdAnforderung EsdAnforderung
        {
            get { return PropertyCacheGet(() => new EsdAnforderung()); }
            set { PropertyCacheSet(value); }
        }


        public void DataInit()
        {
            DataMarkForRefresh();

            EsdAnforderung = new EsdAnforderung
            {
                AnsprechVorname = LogonContext.FirstName,
                AnsprechNachname = LogonContext.LastName,
                AnsprechEmail = LogonContext.UserInfo.Mail,
                AnsprechTelefonNr = LogonContext.UserInfo.Telephone
            };
            EsdAnforderung.InitDienstleistungen();
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

        private void ApplyDetails(EsdAnforderung model)
        {
            //EsdAnforderung.JahrDerErstzulassung = model.JahrDerErstzulassung;
            //EsdAnforderung.FahrgestellNr = model.FahrgestellNr;
            //EsdAnforderung.Vorname = model.Vorname;
            //EsdAnforderung.Nachname = model.Nachname;
            //EsdAnforderung.Email = model.Email;
            //EsdAnforderung.TelefonNr = model.TelefonNr;
        }

        public void EsdAnforderungAbsenden(EsdAnforderung model, Action<string, string> addModelError)
        {
            ApplyDetails(model);

            var empfaengerEmail = EsdAnforderungDataService.GetEmpfaengerEmailAdresse();

            if (string.IsNullOrEmpty(empfaengerEmail))
            {
                addModelError("", string.Format("{0}: {1}", Localize.ErrorMailCouldNotBeSent, Localize.NoEmailAddressFound));
                return;
            }

            var mailService = new SmtpMailService(AppSettings);

            var mailText = "Folgende Esd-Bestellung wurde eben im Portal Kroschke ON aufgegeben:<br/>";
            mailText += "<br/>";
            mailText += "Fahrzeugdaten:<br/>";
            //mailText += string.Format("{0}: {1}<br/>", Localize.VehicleType, EsdAnforderung.Kopfdaten.FahrzeugTypBezeichnung);
            //mailText += string.Format("{0}: {1}<br/>", Localize.Manufacturer, EsdAnforderung.Kopfdaten.HerstellerBezeichnung);
            //mailText += string.Format("{0}: {1}<br/>", Localize.CountryOfFirstRegistration, EsdAnforderung.Kopfdaten.LandDerErstenZulassungBezeichnung);
            //mailText += string.Format("{0}: {1}<br/>", Localize.YearOfFirstRegistration, EsdAnforderung.JahrDerErstzulassung);
            //mailText += "<br/>";
            //mailText += "Kundendaten:<br/>";
            //mailText += string.Format("{0}: {1}<br/>", Localize.Company, LogonContext.CustomerName);
            //mailText += string.Format("{0}: {1} {2}<br/>", Localize.Name, EsdAnforderung.Vorname, EsdAnforderung.Nachname);
            //mailText += string.Format("{0}: {1}<br/>", Localize.Email, EsdAnforderung.Email);
            //mailText += string.Format("{0}: {1}<br/>", Localize.PhoneNo, EsdAnforderung.TelefonNr);

            if (!mailService.SendMail(empfaengerEmail, "Esd-Bestellung", mailText))
                addModelError("", Localize.ErrorMailCouldNotBeSent);
        }
    }
}
