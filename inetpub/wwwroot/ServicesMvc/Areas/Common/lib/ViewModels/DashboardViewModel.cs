// ReSharper disable RedundantUsingDirective

using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Contracts;
using GeneralTools.Models;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.General.ViewModels;

namespace CkgDomainLogic.DomainCommon.ViewModels
{
    public class DashboardViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IDashboardDataService DataService { get { return CacheGet<IDashboardDataService>(); } }


        public List<IDashboardItem> DashboardItems
        {
            get { return PropertyCacheGet(() => DataService.GetDashboardItems().ToList()); }
        }
        
        public void DataInit()
        {
            DataMarkForRefresh();
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.DashboardItems);
        }
    }
}
