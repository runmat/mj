// ReSharper disable RedundantUsingDirective
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.AppUserOverview.Contracts;
using CkgDomainLogic.AppUserOverview.Models;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Database.Services;
using GeneralTools.Models;
using ServicesMvc.AppUserOverview.Models;

//using AppModelMappings = CkgDomainLogic.Strafzettel.Models.AppModelMappings;
//using AppModelMappings = CkgDomainLogic.AppUserOverview.Models.AppModelMappings;
// ReSharper restore RedundantUsingDirective

namespace CkgDomainLogic.AppUserOverview.Services
{
    public class AppUserOverviewDataService : CkgGeneralDataService, IAppUserOverviewDataService 
    {

        public List<AppUserOverviewResults> GetGridData(AppUserOverviewSelektor selector, ILogonContextDataService logonContext)
        {

            List<AppUserOverviewResults> appUserOverviewList;

            using (var dbContext = new DomainDbContext(ConfigurationManager.AppSettings["Connectionstring"], logonContext.UserName))
            {
                const string sql = "SELECT dbo._vwAnwendungProKunde.AppID, dbo._vwAnwendungProWebUser.CustomerID, dbo._vwAnwendungProKunde.AppName, dbo._vwAnwendungProKunde.AppFriendlyName, " +
                                   "dbo._vwAnwendungProWebUser.Customername, dbo._vwAnwendungProWebUser.GroupName, COUNT(dbo._vwAnwendungProWebUser.WebUserUsername) AS WebUserCount, " +
                                   "CASE WHEN SUM(CASE WHEN dbo._vwAnwendungProWebUser.AccountIsLockedOut = 'true' THEN 0 ELSE 1 END) > 0 THEN 'x' ELSE '' END AS HatAktiveWebUser " +
                                   "FROM dbo._vwAnwendungProKunde INNER JOIN dbo._vwAnwendungProWebUser ON dbo._vwAnwendungProKunde.CustomerID = dbo._vwAnwendungProWebUser.CustomerID " +
                                   "AND dbo._vwAnwendungProKunde.AppID = dbo._vwAnwendungProWebUser.AppID GROUP BY dbo._vwAnwendungProKunde.AppID, dbo._vwAnwendungProKunde.AppName, " +
                                   "dbo._vwAnwendungProKunde.AppFriendlyName, dbo._vwAnwendungProWebUser.Customername, dbo._vwAnwendungProWebUser.GroupName, dbo._vwAnwendungProWebUser.CustomerID " +
                                   "ORDER BY dbo._vwAnwendungProKunde.AppFriendlyName, dbo._vwAnwendungProWebUser.Customername, dbo._vwAnwendungProWebUser.GroupName";

                appUserOverviewList = dbContext.Database.SqlQuery<AppUserOverviewResults>(sql).ToListOrEmptyList();

                // Falls vom Benutzer gewünscht, Ergebnisliste dahingehend filtern, dass nur aktive Anwendungen angezeigt werden...
                if (selector.ShowActiveOnly == "Active")
                {
                    appUserOverviewList = appUserOverviewList.Where(x => x.HatAktiveWebUser == "x").ToListOrEmptyList();
                }
            }

            return appUserOverviewList;

        }

    }
}
