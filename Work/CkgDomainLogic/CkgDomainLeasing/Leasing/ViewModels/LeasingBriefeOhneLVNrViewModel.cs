using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Services;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Leasing.Contracts;
using CkgDomainLogic.Leasing.Models;
using GeneralTools.Models;
using GeneralTools.Services;
using WebTools.Services;

namespace CkgDomainLogic.Leasing.ViewModels
{
    public class LeasingBriefeOhneLVNrViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public ILeasingUnzugelFzgDataService DataService { get { return CacheGet<ILeasingUnzugelFzgDataService>(); } }

        [XmlIgnore]
        public List<UnzugelFzg> UnzugelFzge { get { return DataService.UnzugelFzge; } }

        [XmlIgnore]
        public List<UnzugelFzg> UnzugelFzgeToSubmit
        {
            get { return PropertyCacheGet(() => UnzugelFzge); }
            private set { PropertyCacheSet(value); }
        }

        public bool SubmitMode { get; set; }

        public int CurrentAppID { get; set; }

        [XmlIgnore]
        public List<UnzugelFzg> GridItems
        {
            get
            {
                if (SubmitMode)
                {
                    return UnzugelFzgeToSubmit;
                }
                else
                {
                    return UnzugelFzgeFiltered;
                }
            }
        }

        public void LoadUnzugelFzge(ModelStateDictionary state)
        {
            GetCurrentAppID();

            DataService.MarkForRefreshUnzugelFzge();
            PropertyCacheClear(this, m => m.UnzugelFzgeFiltered);

            if (UnzugelFzge.Count == 0)
            {
                state.AddModelError("", Localize.NoDataFound);
            }
        }

        public void ApplyLVNummern(IEnumerable<UnzugelFzg> liste)
        {
            if (liste != null)
            {
                foreach (UnzugelFzg fzg in liste)
                {
                    UnzugelFzge.Find(f => f.Equipmentnummer == fzg.Equipmentnummer).Leasingvertragsnummer = fzg.Leasingvertragsnummer;
                }
            }

            SwitchToSubmitMode();
        }

        public void SaveChangesToSap(ModelStateDictionary state)
        {
            if ((UnzugelFzgeToSubmit != null) && (UnzugelFzgeToSubmit.Count > 0))
            {
                DataService.SaveBriefLVNummern(UnzugelFzgeToSubmit);
                if (!SendStatusMail())
                    state.AddModelError("", Localize.EmailSentError);
            }
            else
            {
                state.AddModelError("", Localize.NoDataChanged);
            }
        }

        public void SwitchToSubmitMode()
        {
            UnzugelFzgeToSubmit = UnzugelFzge.FindAll(f => !String.IsNullOrEmpty(f.Leasingvertragsnummer));
            SubmitMode = true;
        }

        public void ResetSubmitState()
        {
            UnzugelFzgeToSubmit.Clear();
            SubmitMode = false;
        }

        private void GetCurrentAppID()
        {
            CurrentAppID = HttpContextService.TryGetAppIdFromUrlOrSession();
        }

        private bool SendStatusMail()
        {
            bool erg = false;

            try
            {
                var mailEmpfaenger = ApplicationConfiguration.GetApplicationConfigValue("MailEmpfaenger", CurrentAppID.ToString(), LogonContext.Customer.CustomerID, LogonContext.Group.GroupID);

                if (!String.IsNullOrEmpty(mailEmpfaenger))
                {
                    var mailBetreff = String.Format("{0} LV-Nr. Erfassung (Benutzer: {1})", LogonContext.CustomerName, LogonContext.UserName);

                    var mailService = new SmtpMailService(AppSettings);

                    string mailText = "Für nachfolgende(s) Fahrzeug(e) wurde(n) die LV-Nummern im Web erfasst:<br/>";
                    mailText += "<br/>";
                    foreach (var item in UnzugelFzgeToSubmit)
                    {
                        mailText += String.Format("Fahrgestellnummer: {0} - Leasingvertragsnummer: {1}<br/>", item.Fahrgestellnummer, item.Leasingvertragsnummer);
                    }

                    erg = mailService.SendMail(mailEmpfaenger, mailBetreff, mailText);
                }
            }
            catch (Exception)
            {
                erg = false;
            }

            return erg;
        }

        #region Filter

        public string FilterEquipmentnummer { get; set; }

        public string FilterFahrgestellnummer { get; set; }

        public string FilterHaendlername { get; set; }

        public string FilterHaendlerort { get; set; }

        public string FilterLeasingvertragsnummer { get; set; }

        public string FilterBriefnummer { get; set; }

        [XmlIgnore]
        public List<UnzugelFzg> UnzugelFzgeFiltered
        {
            get { return PropertyCacheGet(() => UnzugelFzge); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterUnzugelFzge(string filterValue, string filterProperties)
        {
            UnzugelFzgeFiltered = UnzugelFzge.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
    }
}
