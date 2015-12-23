using CkgDomainLogic.General.Contracts;
using GeneralTools.Models;

namespace CkgDomainLogic.General.Database.Models
{
    public class DashboardItemOptions : IDashboardItemOptions
    {
        private int _columnSpan = 1;
        private string _itemType = "chart";
        private int _jsonDataCacheExpirationMinutes = 1440; // 24h

        public int ColumnSpan
        {
            get { return _columnSpan; }
            set { _columnSpan = value; }
        }

        public bool IsAuthorized { get; set; }

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
    }
}