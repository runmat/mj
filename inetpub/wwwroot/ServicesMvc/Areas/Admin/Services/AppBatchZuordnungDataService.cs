using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using CkgDomainLogic.Admin.Contracts;
using CkgDomainLogic.Admin.Models;
using CkgDomainLogic.General.Database.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.Database.Services;
using GeneralTools.Models;

namespace CkgDomainLogic.Admin.Services
{
    public class AppBatchZuordnungDataService : CkgGeneralDataService, IAppBatchZuordnungDataService 
    {
        public List<Application> GetApplications()
        {
            using (var dbContext = new DomainDbContext(ConfigurationManager.AppSettings["Connectionstring"], LogonContext.UserName))
            {
                return dbContext.Applications.OrderBy(a => a.AppFriendlyName).ToListOrEmptyList();
            }
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

                    foreach (var kundenItem in kunden)
                    {
                        var kunde = kundenItem;

                        var gruppen = dbContext.UserGroups.Where(ug => ug.CustomerID == kunde.CustomerID);

                        var gruppenZuordnungen = dbContext.ApplicationGroupRights.Where(agr => agr.AppID == anwendung.AppID);

                        foreach (var gruppenItem in gruppen)
                        {
                            var gruppe = gruppenItem;

                            var gruppenZuordnungExists = gruppenZuordnungen.Any(z => z.GroupID == gruppe.GroupID);

                            liste.Add(new AppZuordnung(anwendung, kunde, gruppe, gruppenZuordnungExists));
                        }
                    }
                }
            }

            return liste;
        }

        public string SaveAppZuordnungen(List<AppZuordnung> zuordnungen)
        {
            var erg = "";

            try
            {
                var zeitstempel = DateTime.Now;

                using (var dbContext = new DomainDbContext(ConfigurationManager.AppSettings["Connectionstring"], LogonContext.UserName))
                {
                    foreach (var item in zuordnungen)
                    {
                        var zuordnung = item;

                        var kundenZuordnungen = dbContext.ApplicationCustomerRights.Where(acr => acr.AppID == zuordnung.AppID);
                        var kundenZuordnungExists = kundenZuordnungen.Any(z => z.CustomerID == zuordnung.CustomerID);
                        var gruppenZuordnungen = dbContext.ApplicationGroupRights.Where(agr => agr.AppID == zuordnung.AppID);
                        var gruppenZuordnungExists = gruppenZuordnungen.Any(z => z.GroupID == zuordnung.GroupID);

                        if (zuordnung.IsAssigned)
                        {
                            if (!kundenZuordnungExists)
                            {
                                dbContext.AddApplicationCustomerRight(zuordnung.AppID, zuordnung.CustomerID);
                                dbContext.WriteApplicationCustomerRightBatchHistory(zuordnung.AppID, zuordnung.CustomerID,
                                    zeitstempel, DomainDbContext.ApplicationBatchZuordnungHistoryChangeType.Insert);
                            }

                            if (!gruppenZuordnungExists)
                            {
                                dbContext.AddApplicationGroupRight(zuordnung.AppID, zuordnung.GroupID);
                                dbContext.WriteApplicationGroupRightBatchHistory(zuordnung.AppID, zuordnung.GroupID,
                                    zeitstempel, DomainDbContext.ApplicationBatchZuordnungHistoryChangeType.Insert);
                            }
                        }
                        else
                        {
                            if (gruppenZuordnungExists)
                            {
                                dbContext.RemoveApplicationGroupRight(zuordnung.AppID, zuordnung.GroupID);
                                dbContext.WriteApplicationGroupRightBatchHistory(zuordnung.AppID, zuordnung.GroupID,
                                    zeitstempel, DomainDbContext.ApplicationBatchZuordnungHistoryChangeType.Delete);
                            }
                        }
                    }

                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                erg = ex.Message;
            }

            return erg;
        }
    }
}
