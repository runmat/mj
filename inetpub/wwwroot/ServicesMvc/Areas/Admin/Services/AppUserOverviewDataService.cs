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
                // const string sql = "SELECT AppID, CustomerID, AppName, AppFriendlyName, Customername, GroupName, WebUserCount, HasActiveWebUsers FROM vwAppUserOverview;";
                const string sql = "SELECT AppID, CustomerID, AppName, AppFriendlyName, AppUrl, Customername, GroupName, WebUserCount, HasActiveWebUsers FROM vwAppUserOverview;";  // MaihoferM Weitere Spalte "AppUrl" eingebunden

                appUserOverviewList = dbContext.Database.SqlQuery<AppUserOverviewResults>(sql).ToListOrEmptyList();

                // Falls vom Benutzer gewünscht, Ergebnisliste dahingehend filtern, dass nur aktive Anwendungen angezeigt werden...
                if (selector.ShowActiveOnly == "Active")
                {
                    appUserOverviewList = appUserOverviewList.Where(x => x.HasActiveWebUsers == "x").ToListOrEmptyList();
                }
            }

            return appUserOverviewList;

        }

    }
}
