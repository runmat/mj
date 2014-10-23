using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Xml.Serialization;
using CkgDomainLogic.Finance.Contracts;
using CkgDomainLogic.Finance.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Finance.ViewModels
{
    public class FinanceBewertungViewModel : FinanceViewModelBase
    {
        [XmlIgnore]
        public IFinanceBewertungDataService DataService { get { return CacheGet<IFinanceBewertungDataService>(); } }

        [XmlIgnore]
        public List<VorgangInfo> Vorgaenge { get { return DataService.Vorgaenge; } }

        public void LoadVorgaenge(VorgangSuchparameter suchparameter)
        {
            DataService.Suchparameter.Vertragsart = suchparameter.Vertragsart;
            DataService.Suchparameter.Kontonummer = suchparameter.Kontonummer;
            DataService.Suchparameter.CIN = suchparameter.CIN;
            DataService.Suchparameter.PAID = suchparameter.PAID;
            DataService.MarkForRefreshVorgaenge();
            PropertyCacheClear(this, m => m.VorgaengeFiltered);
        }

        public void FillVertragsarten()
        {
            DataService.Suchparameter.AuswahlVertragsart = GetVertragsarten();

            if ((String.IsNullOrEmpty(DataService.Suchparameter.Vertragsart)) && (DataService.Suchparameter.AuswahlVertragsart.Count > 0))
                DataService.Suchparameter.Vertragsart = DataService.Suchparameter.AuswahlVertragsart[0];
        }

        public VorgangBewertung GetVorgangFuerBewertung(string kontonr, string cin, string paid)
        {
            return new VorgangBewertung { Kontonummer = kontonr, CIN = cin, PAID = paid, Erfassungstyp = Erfassungsart.Gutachterwert };
        }

        public void SaveBewertung(VorgangBewertung vb, ModelStateDictionary state)
        {
            vb.Pruefdatum = DateTime.Today;
            var erg = DataService.SaveBewertung(vb);
            if (!String.IsNullOrEmpty(erg))
            {
                state.AddModelError("", erg);
            }
        }

        #region Filter

        [XmlIgnore]
        public List<VorgangInfo> VorgaengeFiltered
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
