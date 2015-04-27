// ReSharper disable RedundantUsingDirective

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Database.Models;
using GeneralTools.Models;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.General.Services;
using ServicesMvc.DomainCommon.Models;

namespace CkgDomainLogic.DomainCommon.ViewModels
{
    public class GridAdminViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IGridAdminDataService DataService { get { return CacheGet<IGridAdminDataService>(); } }

        public bool TmpDeleteCustomerTranslation { get; set; }

        public int CurrentCustomerID { get; set; }

        public string CurrentResourceID { get; set; }

        public TranslatedResource CurrentTranslatedResource { get; set; }

        public TranslatedResourceCustom CurrentTranslatedResourceCustomer { get; set; }

        public ReportSolution ReportSettings { get; set; }


        public Customer CurrentCustomer { get; set; }

        public List<Customer> Customers
        {
            get
            {
                return PropertyCacheGet(() => DataService.GetCustomers().OrderBy(c => c.Customername).ToListOrEmptyList()
                            .CopyAndInsertAtTop(new Customer { CustomerID = -1, Customername = "" }));
            }
        }


        public User CurrentUser { get; set; }
        
        public List<User> Users { get; set; }


        public bool DataInit(Type modelType, string columnMember)
        {
            var propertyInfo = modelType.GetProperty(columnMember);
            if (propertyInfo  == null)
                return false;

            var localizeAttribute = propertyInfo.GetCustomAttributes(true).OfType<LocalizedDisplayAttribute>().FirstOrDefault();
            if (localizeAttribute == null)
                return false;

            CurrentResourceID = localizeAttribute.ResourceID;
            CurrentTranslatedResource = DataService.TranslatedResourceLoad(CurrentResourceID);
            CurrentTranslatedResourceCustomer = DataService.TranslatedResourceCustomerLoad(CurrentResourceID, CurrentCustomerID);

            return true;
        }

        public void DataSave(GridAdminViewModel model)
        {
            if (model.TmpDeleteCustomerTranslation)
            {
                DataService.TranslatedResourceCustomerDelete(model.CurrentTranslatedResourceCustomer);
                return;
            }

            DataService.TranslatedResourceUpdate(model.CurrentTranslatedResource);
            
            
            DataService.TranslatedResourceCustomerUpdate(model.CurrentTranslatedResourceCustomer);
        }

        public bool TrySetReportSettings(string un)
        {
            ReportSettings = new ReportSolution();

            var items = un.NotNullOrEmpty().Split('-');
            if (items.None() || items.Count() < 3)
                return false;

            var adminDate = new DateTime(items[2].ToLong(0));
            if ((DateTime.Now - adminDate).TotalMinutes > 1)
                return false;

            var appID = items[0].ToInt();
            if (appID < 0)
                return false;

            ReportSettings.CallingDateTime = adminDate;
            ReportSettings.AppID = items[0].ToInt();
            ReportSettings.AdminUserName = items[1];
            ReportSettings.AdminIsAuthorized = true;
            return true;
        }

        public void LoadUserForCustomer(int customerId)
        {
            Users = new List<User>();
            CurrentUser = null;

            CurrentCustomer = Customers.FirstOrDefault(c => c.CustomerID == customerId);
            if (CurrentCustomer == null)
                return;

            Users = DataService.GetUsersForCustomer(CurrentCustomer)
                        .CopyAndInsertAtTop(new User { UserID = -1, Username = Localize.DropdownDefaultOptionNotSpecified });
        }

        public void SetCurrentUser(int userId)
        {
            if (Users == null || Users.None())
                return;

            CurrentUser = Users.FirstOrDefault(user => user.UserID == userId);
        }

        public User GetCurrentValidUser()
        {
            if (CurrentUser != null)
                return CurrentUser;

            return Users.FirstOrDefault(user => user.UserID > 0);
        }

        public string GetRelativeAppUrl()
        {
            var appUrl = DataService.GetAppUrl(ReportSettings.AppID);
            appUrl = LogonContext.FormatUrl(appUrl).ToString();

            return appUrl;
        }
    }
}
