using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Archive.Contracts;
using CkgDomainLogic.Archive.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Services;

namespace CkgDomainLogic.Archive.ViewModels
{
    public class DateiDownloadViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IDateiDownloadDataService DataService { get { return CacheGet<IDateiDownloadDataService>(); } }

        [XmlIgnore]
        public List<DateiInfo> Dateien
        {
            get { return PropertyCacheGet(() => new List<DateiInfo>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<DateiInfo> DateienFiltered
        {
            get { return PropertyCacheGet(() => Dateien); }
            private set { PropertyCacheSet(value); }
        }

        public string Verzeichnis { get; private set; }

        public string SuchPattern { get; private set; }

        public int CurrentAppID { get; set; }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.DateienFiltered);
        }

        public void Init()
        {
            GetCurrentAppID();

            Verzeichnis = ApplicationConfiguration.GetApplicationConfigValue("VerzeichnisPfad", CurrentAppID.ToString(), LogonContext.Customer.CustomerID, LogonContext.Group.GroupID);
            SuchPattern = ApplicationConfiguration.GetApplicationConfigValue("SuchPattern", CurrentAppID.ToString(), LogonContext.Customer.CustomerID, LogonContext.Group.GroupID);

            if (String.IsNullOrEmpty(SuchPattern))
                SuchPattern = "*";

            Dateien = DataService.LoadDateiInfos(Verzeichnis, SuchPattern);

            DataMarkForRefresh();
        }

        public void FilterDateien(string filterValue, string filterProperties)
        {
            DateienFiltered = Dateien.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        private void GetCurrentAppID()
        {
            CurrentAppID = LogonContext.GetAppIdCurrent();
        }
    }
}
