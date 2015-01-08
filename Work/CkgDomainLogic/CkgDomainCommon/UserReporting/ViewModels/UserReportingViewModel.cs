using System.Collections.Generic;
using System.Xml.Serialization;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.UserReporting.Contracts;
using CkgDomainLogic.UserReporting.Models;
using GeneralTools.Log.Models.MultiPlatform;
using GeneralTools.Models;

namespace CkgDomainLogic.UserReporting.ViewModels
{
    public class UserReportingViewModel : CkgBaseViewModel 
    {
        [XmlIgnore]
        public IUserReportingDataService DataService { get { return CacheGet<IUserReportingDataService>(); } }

        public WebUserSuchparameter Suchparameter
        {
            get { return PropertyCacheGet(() => new WebUserSuchparameter { UserSelection = "A" }); }
            set { PropertyCacheSet(value); }
        }

        public List<UserInfo> Users
        {
            get { return PropertyCacheGet(() => new List<UserInfo>()); }
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<UserInfo> UsersFiltered
        {
            get { return PropertyCacheGet(() => Users); }
            private set { PropertyCacheSet(value); }
        }

        public List<MpCustomer> Customers { get { return PropertyCacheGet(() => DataService.GetCustomerList()); } }

        public void DataInit()
        {
            DataMarkForRefresh();

            WebUserSuchparameter.AllCustomers = Customers;
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.UsersFiltered);
            PropertyCacheClear(this, m => m.Users);
            PropertyCacheClear(this, m => m.Customers);
        }

        public void FilterUsers(string filterValue, string filterProperties)
        {
            UsersFiltered = Users.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public bool LoadUserList(WebUserSuchparameter wusp)
        {
            Suchparameter = wusp;

            DataMarkForRefresh();

            Users = DataService.GetWebUsers(Suchparameter);

            return true;
        }
    }
}
