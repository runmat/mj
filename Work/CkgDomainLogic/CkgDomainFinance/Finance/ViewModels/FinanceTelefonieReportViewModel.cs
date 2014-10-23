using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using CkgDomainLogic.Finance.Contracts;
using CkgDomainLogic.Finance.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Finance.ViewModels
{
    public class FinanceTelefonieReportViewModel : FinanceViewModelBase
    {
        [XmlIgnore]
        public IFinanceTelefonieReportDataService DataService { get { return CacheGet<IFinanceTelefonieReportDataService>(); } }

        [XmlIgnore]
        public List<TelefoniedatenItem> Telefoniedaten { get { return DataService.Telefoniedaten; } }

        public void LoadTelefoniedaten(TelefoniedatenSuchparameter suchparameter)
        {
            DataService.Suchparameter.Vertragsart = suchparameter.Vertragsart;
            DataService.Suchparameter.DatumRange = suchparameter.DatumRange;
            DataService.Suchparameter.Anrufart = suchparameter.Anrufart;
            DataService.MarkForRefreshTelefoniedaten();
            PropertyCacheClear(this, m => m.TelefoniedatenFiltered);
        }

        public void FillVertragsarten()
        {
            DataService.Suchparameter.AuswahlVertragsart = GetVertragsarten();

            if ((String.IsNullOrEmpty(DataService.Suchparameter.Vertragsart)) && (DataService.Suchparameter.AuswahlVertragsart.Count > 0))
                DataService.Suchparameter.Vertragsart = DataService.Suchparameter.AuswahlVertragsart[0];
        }

        #region Filter

        [XmlIgnore]
        public List<TelefoniedatenItem> TelefoniedatenFiltered
        {
            get { return PropertyCacheGet(() => Telefoniedaten); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterTelefoniedaten(string filterValue, string filterProperties)
        {
            TelefoniedatenFiltered = Telefoniedaten.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
    }
}
