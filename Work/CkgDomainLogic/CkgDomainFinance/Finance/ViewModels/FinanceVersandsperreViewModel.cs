using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Finance.Contracts;
using CkgDomainLogic.Finance.Models;
using GeneralTools.Models;
using MvcTools.Web;
using System.Linq;

namespace CkgDomainLogic.Finance.ViewModels
{
    public class FinanceVersandsperreViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IFinanceVersandsperreDataService DataService { get { return CacheGet<IFinanceVersandsperreDataService>(); } }

        [XmlIgnore]
        public List<VorgangVersandsperre> Vorgaenge { get { return DataService.Vorgaenge; } }

        [XmlIgnore]
        public List<VorgangVersandsperre> SelektierteVorgaenge
        {
            get { return PropertyCacheGet(() => Vorgaenge); }
            private set { PropertyCacheSet(value); }
        }

        public bool EditMode { get; set; }

        [XmlIgnore]
        public List<VorgangVersandsperre> GridItems
        {
            get
            {
                if (EditMode)
                {
                    return VorgaengeFiltered;
                }
                else
                {
                    return SelektierteVorgaenge;
                }
            }
        }

        public void LoadVorgaenge(VorgangVersandperreSuchparameter suchparameter, ModelStateDictionary state)
        {
            EditMode = true;
            DataService.Suchparameter.Sperrtyp = suchparameter.Sperrtyp;
            DataService.Suchparameter.Vertragsart = suchparameter.Vertragsart;
            DataService.Suchparameter.Kontonummer = suchparameter.Kontonummer;
            DataService.Suchparameter.CIN = suchparameter.CIN;
            DataService.Suchparameter.PAID = suchparameter.PAID;
            DataService.MarkForRefreshVorgaenge();
            PropertyCacheClear(this, m => m.VorgaengeFiltered);

            if (Vorgaenge.Count == 0)
            {
                state.AddModelError("", Localize.NoDataFound);
            }
            else if (suchparameter.Sperrtyp == "N" && Vorgaenge.Any(v => v.Versandsperre))
            {
                state.AddModelError("", Localize.RecordIsAlreadyLocked);
            }
            else if (suchparameter.Sperrtyp == "D" && Vorgaenge.None(v => v.Versandsperre))
            {
                state.AddModelError("", Localize.RecordIsNotLocked);
            }
        }

        public void FillVertragsarten()
        {
            var vertragsArten = new List<string>();

            var myLogonContext = (LogonContext as LogonContextDataServiceDadServices);
            if (myLogonContext != null)
            {
                if ((myLogonContext.Organization != null) && (!String.IsNullOrEmpty(myLogonContext.Organization.OrganizationName)))
                {
                    var vArten = myLogonContext.Organization.OrganizationName.Split('+');
                    Array.Sort(vArten);
                    foreach (var vArt in vArten)
                    {
                        vertragsArten.Add(vArt.Trim());
                    }
                }
            }

            DataService.Suchparameter.AuswahlVertragsart = vertragsArten;
            if ((String.IsNullOrEmpty(DataService.Suchparameter.Vertragsart)) && (vertragsArten.Count > 0))
            {
                DataService.Suchparameter.Vertragsart = vertragsArten[0];
            }
        }

        public void SaveVersandsperren(string selectedItems, ModelStateDictionary state)
        {
            var liste = JSon.Deserialize<string[]>(selectedItems);
            foreach (var vg in Vorgaenge)
            {
                vg.Selektiert = liste.Contains(vg.PAID);
            }
            SelektierteVorgaenge = Vorgaenge.FindAll(v => v.Selektiert);

            if ((SelektierteVorgaenge != null) && (SelektierteVorgaenge.Count > 0))
            {
                EditMode = false;
                var message = "";
                SelektierteVorgaenge = DataService.SaveVersandsperren(SelektierteVorgaenge, ref message);
                if (!String.IsNullOrEmpty(message))
                {
                    state.AddModelError("", message);
                }
            }
            else
            {
                state.AddModelError("", Localize.NoDataSelected);
            }
        }

        #region Filter

        [XmlIgnore]
        public List<VorgangVersandsperre> VorgaengeFiltered
        {
            get { return PropertyCacheGet(() => Vorgaenge); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterVorgaenge(string filterValue, string filterProperties)
        {
            VorgaengeFiltered = Vorgaenge.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
    }
}
