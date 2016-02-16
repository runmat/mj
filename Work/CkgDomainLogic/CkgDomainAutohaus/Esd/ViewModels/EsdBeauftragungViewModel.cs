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
                        .Concat(new List<Land> { new Land { ID = "-", Name = Localize.DropdownDefaultOptionOther } })
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
                AnsprechTelefonNr = LogonContext.UserInfo.Telephone
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

            if (string.IsNullOrEmpty(empfaengerEmail))
            {
                addModelError("", string.Format("{0}: {1}", Localize.ErrorMailCouldNotBeSent, Localize.NoEmailAddressFound));
                return;
            }

            var mailService = new SmtpMailService(AppSettings);

            if (!mailService.SendMail(empfaengerEmail, Localize.Autohaus_EsdAnforderung, EsdBeauftragung.GetMailText()))
                addModelError("", Localize.ErrorMailCouldNotBeSent);
        }
    }
}
