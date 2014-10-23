using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using CkgDomainLogic.Finance.Contracts;
using CkgDomainLogic.Finance.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Finance.ViewModels
{
    public class FinanceMahnungenVorErsteingangViewModel : FinanceViewModelBase
    {
        [XmlIgnore]
        public IFinanceMahnungenVorErsteingangDataService DataService { get { return CacheGet<IFinanceMahnungenVorErsteingangDataService>(); } }

        [XmlIgnore]
        public List<Mahnung> Mahnungen { get { return DataService.Mahnungen; } }

        public void LoadMahnungen(MahnungVorErsteingangSuchparameter suchparameter)
        {
            DataService.Suchparameter = suchparameter;
            DataService.MarkForRefreshMahnungen();
            PropertyCacheClear(this, m => m.MahnungenFiltered);
        }

        public void FillVertragsarten()
        {
            DataService.Suchparameter.AuswahlVertragsart = GetVertragsarten();

            if ((String.IsNullOrEmpty(DataService.Suchparameter.Vertragsart)) && (DataService.Suchparameter.AuswahlVertragsart.Count > 0))
                DataService.Suchparameter.Vertragsart = DataService.Suchparameter.AuswahlVertragsart[0];
        }

        #region Filter

        [XmlIgnore]
        public List<Mahnung> MahnungenFiltered
        {
            get { return PropertyCacheGet(() => Mahnungen); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterMahnungen(string filterValue, string filterProperties)
        {
            MahnungenFiltered = Mahnungen.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
    }
}
