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

namespace CkgDomainLogic.AppUserOverview.Services
{
    public class AppBatchZuordnungDataService : CkgGeneralDataService, IAppBatchZuordnungDataService 
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


        public List<AppZuordnung> GetAppZuordnungen(AppBatchZuordnungSelektor selektor)
        {
            var liste = new List<AppZuordnung>();

            using (var dbContext = new DomainDbContext(ConfigurationManager.AppSettings["Connectionstring"], LogonContext.UserName))
            {
                var anwendung = dbContext.Applications.FirstOrDefault(a => a.AppID == selektor.SelectedAppId);

                if (anwendung != null)
                {
                    var kunden = dbContext.GetAllCustomer();

                    var kundenZuordnungen = dbContext.ApplicationCustomerRights.Where(acr => acr.AppID == anwendung.AppID);

                    foreach (var kundenItem in kunden)
                    {
                        var kunde = kundenItem;

                        var kundenZuordnungExists = kundenZuordnungen.Any(z => z.CustomerID == kunde.CustomerID);

                        var gruppen = dbContext.UserGroups.Where(ug => ug.CustomerID == kunde.CustomerID);

                        if (gruppen.None())
                        {
                            liste.Add(new AppZuordnung(anwendung, kunde, kundenZuordnungExists));
                        }
                        else
                        {
                            var gruppenZuordnungen = dbContext.ApplicationGroupRights.Where(agr => agr.AppID == anwendung.AppID);

                            foreach (var gruppenItem in gruppen)
                            {
                                var gruppe = gruppenItem;

                                var gruppenZuordnungExists = gruppenZuordnungen.Any(z => z.GroupID == gruppe.GroupID);

                                liste.Add(new AppZuordnung(anwendung, kunde, kundenZuordnungExists, gruppe, gruppenZuordnungExists));
                            }
                        }
                    }
                }
            }

            return liste;
        }

        public bool SaveChangedAppZuordnungen(List<AppZuordnung> zuordnungen)
        {
            using (var dbContext = new DomainDbContext(ConfigurationManager.AppSettings["Connectionstring"], LogonContext.UserName))
            {
                foreach (var item in zuordnungen)
                {
                    var zuordnung = item;

                    var kundenZuordnungen = dbContext.ApplicationCustomerRights.Where(acr => acr.AppID == zuordnung.AppID);
                    var kundenZuordnungExists = kundenZuordnungen.Any(z => z.CustomerID == zuordnung.CustomerID);
                    var gruppenZuordnungen = dbContext.ApplicationGroupRights.Where(agr => agr.AppID == zuordnung.AppID);
                    var gruppenZuordnungExists = gruppenZuordnungen.Any(z => z.GroupID == zuordnung.GroupID);

                    if (zuordnung.IsAssignedToCustomer)
                    {
                        if (!kundenZuordnungExists)
                            dbContext.AddKundenZuordnung(zuordnung.AppID, zuordnung.CustomerID, true);
                    }
                    else
                    {
                        if (kundenZuordnungExists)
                            dbContext.RemoveKundenZuordnung(zuordnung.AppID, zuordnung.CustomerID, true);
                    }

                    if (zuordnung.IsAssignedToGroup)
                    {
                        if (!gruppenZuordnungExists)
                            dbContext.AddGruppenZuordnung(zuordnung.AppID, zuordnung.GroupID, true);
                    }
                    else
                    {
                        if (gruppenZuordnungExists)
                            dbContext.RemoveGruppenZuordnung(zuordnung.AppID, zuordnung.GroupID, true);
                    }
                }

                dbContext.SaveChanges();
            }
        }
    }
}
