using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using CkgDomainLogic.CoC.Contracts;
using CkgDomainLogic.CoC.Models;
using System.Linq;
using CkgDomainLogic.General.ViewModels;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.CoC.ViewModels
{
    public class CocReportsViewModel : CkgBaseViewModel 
    {
        [XmlIgnore]
        public ICocErfassungDataService DataService { get { return CacheGet<ICocErfassungDataService>(); } }

        [XmlIgnore]
        public List<CocEntity> CocAuftraege { get { return DataService.CocAuftraege; } }

        public string NullSelectionString { get { return "(alle)"; } }

        public List<string> LaenderList
        {
            get
            {
                var laender = CocAuftraege.GroupBy(c => c.LAND).OrderBy(c => c.Key).Select(c => c.Key);
                return new List<string> { NullSelectionString }.Concat(laender).ToList();
            }
        }

        #region Filter

        [LocalizedDisplay(LocalizeConstants.OnlyItemsWithValidOrderDate)]
        public bool FilterNurAuftragsDatumGesetzt { get; set; }

        [LocalizedDisplay(LocalizeConstants.OnlyItemsInDeliveryDateRange)]
        public bool FilterNurAusliefDatumRange { get; set; }

        public DateTime? FilterAusliefDatumStart
        {
            get { return PropertyCacheGet(() => DateTime.Today.AddDays(-30)); }
            set { PropertyCacheSet(value); }
        }

        public DateTime? FilterAusliefDatumEnd
        {
            get { return PropertyCacheGet(() => DateTime.Today.AddDays(30)); }
            set { PropertyCacheSet(value); }
        }

        [LocalizedDisplay(LocalizeConstants.OnlyCountry)]
        public string FilterLand
        {
            get { return PropertyCacheGet(() => NullSelectionString); }
            set { PropertyCacheSet(value); }
        }

        bool Filter(CocEntity cocEntity)
        {
            var ok = true;

            if (FilterNurAuftragsDatumGesetzt)
                ok = cocEntity.AUFTRAG_DAT != null;

            if (ok)
                if (FilterNurAusliefDatumRange && FilterAusliefDatumStart.HasValue && FilterAusliefDatumEnd.HasValue)
                    ok = (cocEntity.AUSLIEFER_DATUM.GetValueOrDefault() >= FilterAusliefDatumStart.GetValueOrDefault() &&
                          cocEntity.AUSLIEFER_DATUM.GetValueOrDefault() <= FilterAusliefDatumEnd.GetValueOrDefault());

            if (ok)
                if (FilterLand.IsNotNullOrEmpty() && FilterLand != NullSelectionString)
                    ok = (cocEntity.LAND.ToLower() == FilterLand.ToLower());

            return ok;
        }

        [XmlIgnore]
        public List<CocEntity> CocAuftraegeFiltered
        {
            get { return PropertyCacheGet(() => CocAuftraege); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterCocAuftraege(string filterValue = "", string filterProperties = "")
        {
            CocAuftraegeFiltered = CocAuftraege.Where(Filter).ToList().SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
    }
}
