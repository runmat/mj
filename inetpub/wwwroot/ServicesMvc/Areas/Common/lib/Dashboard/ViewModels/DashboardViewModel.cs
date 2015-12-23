using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Contracts;
using GeneralTools.Models;
using System.Xml.Serialization;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.General.Services;

namespace CkgDomainLogic.DomainCommon.ViewModels
{
    public class DashboardViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IDashboardDataService DataService { get { return CacheGet<IDashboardDataService>(); } }


        public List<IDashboardItem> DashboardItems
        {
            get { return PropertyCacheGet(() => FilterItemsAvailableForUser(DataService.GetDashboardItems(LogonContext.UserName))); }
        }

        public List<IDashboardItem> VisibleSortedDashboardItems
        {
            get { return DashboardItems.Where(item => item.IsUserVisible).OrderBy(item => item.UserSort).ToList(); }
        }

        public List<IDashboardItem> HiddenDashboardItems
        {
            get { return DashboardItems.Where(item => !item.IsUserVisible).ToList(); }
        }

        public void DataInit()
        {
            DataMarkForRefresh();

            DashboardSessionSaveAllItems(DashboardItems);
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.DashboardItems);
        }

        List<IDashboardItem> FilterItemsAvailableForUser(IList<IDashboardItem> items)
        {
            return (LogonContext.UserApps == null) 
                            ? items.ToList() 
                            : items.Where(item => 
                                item.Options.IsAuthorized || LogonContext.UserApps.Any(userApp => UserAppUrlContainsUrl(userApp.AppURL, item.RelatedAppUrl)) 
                        ).ToList();
        }

        static bool UserAppUrlContainsUrl(string userAppUrl, string url)
        {
            var translatedAppUrl = LogonContextHelper.ExtractUrlFromUserApp(userAppUrl);
            url = url.ToLower().SubstringTry(4);
            return translatedAppUrl.ToLower() == url;
        }

        public void DashboardItemsSave(string commaSeparatedIds)
        {
            DataService.SaveDashboardItems(DashboardItems, LogonContext.UserName, commaSeparatedIds);
        }

        public object GetChartData(string id, out IDashboardItem dashboardItem)
        {
            var dbId = id.Replace("id_", "").Replace("#", "");
            dashboardItem = DashboardItems.FirstOrDefault(item => item.ID == dbId.ToInt());

            if (dashboardItem == null)
                return new { };

            if (dashboardItem.IsPartialView)
                return new { };

            var data = DashboardAppUrlService.InvokeViewModelForAppUrl(dashboardItem.RelatedAppUrl, dashboardItem.ItemKey);
            
            return ChartService.PrepareChartDataAndOptions(data, AppSettings.DataPath, dashboardItem.ChartJsonOptions, dashboardItem.ChartJsonDataCustomizingScriptFunction);
        }
    }
}
