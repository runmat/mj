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
    }
}
