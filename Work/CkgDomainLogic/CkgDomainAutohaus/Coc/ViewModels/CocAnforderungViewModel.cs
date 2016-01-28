using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
        public List<Domaenenfestwert> Fahrzeugarten { get { return ZulassungDataService.Fahrzeugarten; } }

        [XmlIgnore, ScriptIgnore]
        public List<Hersteller> Hersteller { get { return ZulassungDataService.Hersteller; } }

        [XmlIgnore, ScriptIgnore]
        public List<SelectItem> Laendergruppen
        {
            get
            {
                return new List<SelectItem>
                {
                    new SelectItem("EU", Localize.Eu),
                    new SelectItem("NICHT-EU", Localize.NonEu)
                };
            }
        }

        [XmlIgnore, ScriptIgnore]
        public List<SelectItem> Jahre { get { return PropertyCacheGet(() => GetJahreListe()); } }

        [XmlIgnore, ScriptIgnore]
        public List<SelectItem> Anreden
        {
            get
            {
                return new List<SelectItem>
                {
                    new SelectItem("HERR", Localize.SalutationMr),
                    new SelectItem("FRAU", Localize.SalutationMrs)
                };
            }
        }

        public CocAnforderung CocAnforderung
        {
            get { return PropertyCacheGet(() => new CocAnforderung()); }
            set { PropertyCacheSet(value); }
        }


        public void DataInit()
        {
            DataMarkForRefresh();
        }

        public void DataMarkForRefresh()
        {
            ZulassungDataService.MarkForRefresh();
            CocAnforderung = new CocAnforderung();
        }

        private static List<SelectItem> GetJahreListe()
        {
            var liste = new List<SelectItem>();

            for (var jahr = 1996; jahr <= DateTime.Now.Year; jahr++)
            {
                liste.Add(new SelectItem(jahr.ToString(), jahr.ToString()));
            }

            return liste;
        } 

        public void CocAnforderungAbsenden(Action<string, string> addModelError)
        {
            var empfaengerEmail = CocAnforderungDataService.GetEmpfaengerEmailAdresse();

            if (string.IsNullOrEmpty(empfaengerEmail))
            {
                addModelError("", string.Format("{0}: {1}", Localize.ErrorMailCouldNotBeSent, Localize.NoEmailAddressFound));
                return;
            }

            var mailService = new SmtpMailService(AppSettings);

            var mailText = "CoC-Bestellung<br/>";
            mailText += "<br/>";
            mailText += string.Format("{0}: {1}<br/>", Localize.VehicleType, CocAnforderung.FahrzeugTyp);
            mailText += string.Format("{0}: {1}<br/>", Localize.Manufacturer, CocAnforderung.Hersteller);
            mailText += string.Format("{0}: {1}<br/>", Localize.CountryOfFirstRegistration, CocAnforderung.LandDerErstenZulassung);
            mailText += string.Format("{0}: {1}<br/>", Localize.YearOfFirstRegistration, CocAnforderung.JahrDerErstzulassung);
            mailText += string.Format("{0}: {1} {2} {3}<br/>", Localize.Name, CocAnforderung.Anrede, CocAnforderung.Vorname, CocAnforderung.Nachname);
            mailText += string.Format("{0}: {1}<br/>", Localize.Email, CocAnforderung.Email);
            mailText += string.Format("{0}: {1}<br/>", Localize.PhoneNo, CocAnforderung.TelefonNr);

            if (!mailService.SendMail(empfaengerEmail, "CoC-Bestellung", mailText))
                addModelError("", Localize.ErrorMailCouldNotBeSent);
        }
    }
}
