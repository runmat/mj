using System.Collections.Generic;
using System.Xml.Serialization;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Finance.Contracts;
using CkgDomainLogic.Finance.Models;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Finance.ViewModels
{
    public class TempZb2VersandViewModel : FinanceViewModelBase
    {

        [XmlIgnore]
        public IFinanceTempZb2VersandDataService DataService { get { return CacheGet<IFinanceTempZb2VersandDataService>(); } }

        [XmlIgnore]
        public List<TempZb2Versand> TempZb2Versands { get { return DataService.TempZb2Versands; } }


        #region Filter

        [XmlIgnore]
        public List<TempZb2Versand> TempZb2VersandsFiltered
        {
            get { return PropertyCacheGet(() => TempZb2Versands); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterTempZb2Versands(string filterValue, string filterProperties)
        {
            TempZb2VersandsFiltered = TempZb2Versands.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion

    }
}
