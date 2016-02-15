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
    public class EsdBeauftragungViewModel : CkgBaseViewModel
    {
        [XmlIgnore, ScriptIgnore]
        public IZulassungDataService ZulassungDataService
        {
            get { return CacheGet<IZulassungDataService>(); }
        }

        [XmlIgnore, ScriptIgnore]
        public IEsdBeauftragungDataService EsdBeauftragungDataService
        {
            get { return CacheGet<IEsdBeauftragungDataService>(); }
        }

        [XmlIgnore, ScriptIgnore]
        public List<Domaenenfestwert> Fahrzeugarten { get { return ZulassungDataService.Fahrzeugarten.Where(f => !string.IsNullOrEmpty(f.Wert)).OrderBy(f => f.Wert).ToList(); } }

        [XmlIgnore, ScriptIgnore]
        public List<Land> LaenderAuswahlliste
        {
            get
            {
                return EsdBeauftragungDataService.LaenderAuswahlliste
                        .Where(l => !string.IsNullOrEmpty(l.ID))
                        .OrderByDescending(l => l.ID == "DE").ThenBy(l => l.Name.IsNullOrEmpty() ? "ZZ" : l.Name)
                        .Concat(new List<Land> { new Land { ID = "", Name = Localize.DropdownDefaultOptionOther } })
                        .ToList();
            }
        }

        public EsdBeauftragung EsdBeauftragung
        {
            get { return PropertyCacheGet(() => new EsdBeauftragung()); }
            set { PropertyCacheSet(value); }
        }


        public void DataInit()
        {
            DataMarkForRefresh();

            EsdBeauftragung = new EsdBeauftragung
            {
                AnsprechVorname = LogonContext.FirstName,
                AnsprechNachname = LogonContext.LastName,
                AnsprechEmail = LogonContext.UserInfo.Mail,
                AnsprechTelefonNr = LogonContext.UserInfo.Telephone,

                // ToDo: Remove test code:
                KundeVorname = "Walter - REMOVE ME",
                KundeNachname = "Zabel - REMOVE ME",
                KundeEmail = "matthias.jenzen@kroschke.de",
                KundeTelefonNr = "0151",
            };
            EsdBeauftragung.InitDienstleistungen();
        }

        public void DataMarkForRefresh()
        {
            ZulassungDataService.MarkForRefresh();
        }

        public void SetCountryCode(string countryCode)
        {
            EsdBeauftragung.Land = countryCode;
        }

        private void ApplyDetails(EsdBeauftragung model)
        {
            EsdBeauftragung = model;
            EsdBeauftragung.InitDienstleistungen();
        }

        public void EsdBeauftragungAbsenden(EsdBeauftragung model, Action<string, string> addModelError)
        {
            ApplyDetails(model);

            var empfaengerEmail = EsdBeauftragungDataService.GetEmpfaengerEmailAdresse();

            // ToDo: Remove test code:
            if (empfaengerEmail.IsNullOrEmpty())
                empfaengerEmail = "matthias.jenzen@kroschke.de";

                if (string.IsNullOrEmpty(empfaengerEmail))
            {
                addModelError("", string.Format("{0}: {1}", Localize.ErrorMailCouldNotBeSent, Localize.NoEmailAddressFound));
                return;
            }

            var mailService = new SmtpMailService(AppSettings);

            var mailText = "Folgende Auslandsbeauftragung wurde soeben im Portal Kroschke ON aufgegeben:<br/>";
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

            if (!mailService.SendMail(empfaengerEmail, "Auslandsbeauftragung", mailText))
                addModelError("", Localize.ErrorMailCouldNotBeSent);
        }
    }
}
