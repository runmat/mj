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
            get { return PropertyCacheGet(() => DataService.GetApplications()); }
        }

        public AppBatchZuordnungSelektor AppBatchZuordnungSelektor
        {
            get { return PropertyCacheGet(() => new AppBatchZuordnungSelektor()); }
            set { PropertyCacheSet(value); }
        }

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

        public void DataInit()
        {
            PropertyCacheClear(this, m => m.Applications);

            DataMarkForRefresh();
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.AppZuordnungenFiltered);
        }

        public void LoadAppZuordnungen()
        {
            AppZuordnungen = DataService.GetAppZuordnungen(AppBatchZuordnungSelektor);

            DataMarkForRefresh();
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

            var success = message.IsNullOrEmpty();

            if (success)
                LoadAppZuordnungen();

            return success;
        }
    }
}
