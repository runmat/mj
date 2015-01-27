// ReSharper disable RedundantUsingDirective

using System.Collections.Generic;
using System.Linq;
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


        public List<string> DashboardItems
        {
            get { return PropertyCacheGet(() => DataService.DashboardItems.ToList()); }
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
