using System;
using CkgDomainLogic.General.Contracts;
using GeneralTools.Models;

namespace CkgDomainLogic.General.Database.Models
{
    public class DashboardItemOptions : IDashboardItemOptions
    {
        private int _rowSpan = 1;
        private string _itemType = "chart";
        private int _jsonDataCacheExpirationMinutes = 1440; // 24h

        public int RowSpan
        {
            get { return _rowSpan; }
            set { _rowSpan = value; }
        }

        public bool IsAuthorized { get; set; }

        public string AuthorizedIfAppUrlsAuth { get; set; }

        public string ItemType
        {
            get { return _itemType; }
            set { _itemType = value; }
        }

        public int JsonDataCacheExpirationMinutes
        {
            get { return _jsonDataCacheExpirationMinutes; }
            set { _jsonDataCacheExpirationMinutes = value; }
        }

        public bool IsChart { get { return ItemType.NotNullOrEmpty().ToLower() == "chart"; } }

        public bool IsPartialView { get { return ItemType.NotNullOrEmpty().ToLower() == "partialview"; } }

        public string SettingsScriptFunction { get; set; }

        public bool JsonDataCacheExpired(DateTime? dt)
        {
            return dt < (DateTime.Now.AddMinutes(-1*JsonDataCacheExpirationMinutes));
        }
    }
}