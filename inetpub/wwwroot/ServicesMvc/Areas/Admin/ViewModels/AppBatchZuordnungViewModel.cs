using System.Collections.Generic;
using System.Xml.Serialization;
using CkgDomainLogic.Admin.Contracts;
using CkgDomainLogic.Admin.Models;
using CkgDomainLogic.General.Database.Models;
using CkgDomainLogic.General.ViewModels;
using GeneralTools.Models;
using System.Linq;

namespace CkgDomainLogic.Admin.ViewModels
{
    public class AppBatchZuordnungViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IAppBatchZuordnungDataService DataService { get { return CacheGet<IAppBatchZuordnungDataService>(); } }

        [XmlIgnore]
        public List<Application> Applications
        {
            get { return PropertyCacheGet(() => new List<Application>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<Application> ApplicationsFiltered
        {
            get { return PropertyCacheGet(() => Applications); }
            private set { PropertyCacheSet(value); }
        }

        public int SelectedAppId { get; set; }

        [XmlIgnore]
        public List<AppZuordnung> AppZuordnungen
        {
            get { return PropertyCacheGet(() => new List<AppZuordnung>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<AppZuordnung> AppZuordnungenFiltered
        {
            get { return PropertyCacheGet(() => AppZuordnungen); }
            private set { PropertyCacheSet(value); }
        }

        public int AppZuordnungenAssignedCount { get { return AppZuordnungen.Count(z => z.IsAssigned); } }

        public void DataInit()
        {
            DataService.InitSqlData();

            Applications = DataService.Applications.Where(a => a.AppInMenu).OrderBy(a => a.AppFriendlyName).ToListOrEmptyList();

            DataMarkForRefresh();
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.AppZuordnungenFiltered);
        }

        public void LoadAppZuordnungen(int appId)
        {
            SelectedAppId = appId;

            AppZuordnungen = DataService.GetAppZuordnungen(SelectedAppId);

            DataMarkForRefresh();
        }

        public void FilterApplications(string filterValue, string filterProperties)
        {
            ApplicationsFiltered = Applications.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public void FilterAppZuordnungen(string filterValue, string filterProperties)
        {
            AppZuordnungenFiltered = AppZuordnungen.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public void ChangeZuordnung(string id, bool select)
        {
            var zo = AppZuordnungen.FirstOrDefault(z => z.ZuordnungID == id);
            if (zo == null)
                return;

            zo.IsAssigned = select;
        }

        public void ChangeZuordnungen(bool select)
        {
            AppZuordnungen.ForEach(z => z.IsAssigned = select);
        }

        public bool SaveZuordnungen(out string message)
        {
            message = DataService.SaveAppZuordnungen(AppZuordnungen);

            LoadAppZuordnungen(SelectedAppId);

            return message.IsNullOrEmpty();
        }
    }
}
