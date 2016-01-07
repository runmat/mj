using System;

namespace CkgDomainLogic.General.Contracts
{
    public interface IDashboardItemOptions
    {
        int ColumnSpan { get; set; }

        bool IsAuthorized { get; set; }

        string AuthorizedIfAppUrlsAuth { get; set; }

        string ItemType { get; set; }

        int JsonDataCacheExpirationMinutes { get; set; }

        bool IsChart { get; }

        bool IsPartialView { get; }

        bool JsonDataCacheExpired(DateTime? dt);
    }
}
