using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.Entity;
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
        private DomainDbContext _dbContext;

        private DomainDbContext DbContext
        {
            get { return _dbContext ?? (_dbContext = new DomainDbContext(ConfigurationManager.AppSettings["Connectionstring"], LogonContext.UserName)); }
        }

        private List<Application> _applications;
        public List<Application> Applications { get { return (_applications ?? (_applications = DbContext.GetAllApplications())); } }

        private List<Customer> _customers;
        private List<Customer> Customers { get { return (_customers ?? (_customers = DbContext.GetAllCustomer())); } }

        private ObservableCollection<UserGroup> UserGroups { get { return DbContext.UserGroups.Local; } }

        private ObservableCollection<ApplicationCustomerRight> ApplicationCustomerRights { get { return DbContext.ApplicationCustomerRights.Local; } }

        private ObservableCollection<ApplicationGroupRight> ApplicationGroupRights { get { return DbContext.ApplicationGroupRights.Local; } }

        public void InitSqlData()
        {
            DbContext.UserGroups.Load();
            DbContext.ApplicationCustomerRights.Load();
            DbContext.ApplicationGroupRights.Load();
        }

        public List<Application> GetApplications()
        {
            return DbContext.GetAllApplications();
        }

        public List<AppZuordnung> GetAppZuordnungen(int appId)
        {
            var liste = new List<AppZuordnung>();

            var anwendung = Applications.FirstOrDefault(a => a.AppID == appId);

            if (anwendung != null)
            {
                var kunden = Customers.Where(c => !c.Master);

                foreach (var kundenItem in kunden)
                {
                    var kunde = kundenItem;

                    var gruppen = UserGroups.Where(ug => ug.CustomerID == kunde.CustomerID).ToListOrEmptyList();

                    var gruppenZuordnungen = ApplicationGroupRights.Where(agr => agr.AppID == anwendung.AppID).ToListOrEmptyList();

                    foreach (var gruppenItem in gruppen)
                    {
                        var gruppe = gruppenItem;

                        var gruppenZuordnungExists = gruppenZuordnungen.Any(z => z.GroupID == gruppe.GroupID);

                        liste.Add(new AppZuordnung(anwendung, kunde, gruppe, gruppenZuordnungExists));
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

                foreach (var item in zuordnungen)
                {
                    var zuordnung = item;

                    var kundenZuordnungen = ApplicationCustomerRights.Where(acr => acr.AppID == zuordnung.AppID).ToListOrEmptyList();
                    var kundenZuordnungExists = kundenZuordnungen.Any(z => z.CustomerID == zuordnung.CustomerID);
                    var gruppenZuordnungen = ApplicationGroupRights.Where(agr => agr.AppID == zuordnung.AppID).ToListOrEmptyList();
                    var gruppenZuordnungExists = gruppenZuordnungen.Any(z => z.GroupID == zuordnung.GroupID);

                    if (zuordnung.IsAssigned)
                    {
                        if (!kundenZuordnungExists)
                            AddApplicationCustomerRightWithHistory(zuordnung.AppID, zuordnung.CustomerID, zeitstempel);

                        if (!gruppenZuordnungExists)
                            AddApplicationGroupRightWithHistory(zuordnung.AppID, zuordnung.GroupID, zeitstempel);
                    }
                    else if (gruppenZuordnungExists)
                    {
                        RemoveApplicationGroupRightWithHistory(zuordnung.AppID, zuordnung.GroupID, zeitstempel);
                    }
                }

                DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                erg = ex.Message;
            }

            return erg;
        }

        private void AddApplicationCustomerRightWithHistory(int appId, int customerId, DateTime zeitstempel)
        {
            ApplicationCustomerRights.Add(new ApplicationCustomerRight { AppID = appId, CustomerID = customerId });

            DbContext.WriteApplicationCustomerRightBatchHistory(appId, customerId, zeitstempel, DomainDbContext.ApplicationBatchZuordnungHistoryChangeType.Insert);

            var childApps = Applications.Where(a => a.AppParent == appId).ToListOrEmptyList();
            foreach (var appChild in childApps)
            {
                ApplicationCustomerRights.Add(new ApplicationCustomerRight { AppID = appChild.AppID, CustomerID = customerId });

                DbContext.WriteApplicationCustomerRightBatchHistory(appChild.AppID, customerId, zeitstempel, DomainDbContext.ApplicationBatchZuordnungHistoryChangeType.Insert);
            }
        }

        private void AddApplicationGroupRightWithHistory(int appId, int groupId, DateTime zeitstempel)
        {
            ApplicationGroupRights.Add(new ApplicationGroupRight { AppID = appId, GroupID = groupId });

            DbContext.WriteApplicationGroupRightBatchHistory(appId, groupId, zeitstempel, DomainDbContext.ApplicationBatchZuordnungHistoryChangeType.Insert);

            var childApps = Applications.Where(a => a.AppParent == appId).ToListOrEmptyList();
            foreach (var appChild in childApps)
            {
                ApplicationGroupRights.Add(new ApplicationGroupRight { AppID = appChild.AppID, GroupID = groupId });

                DbContext.WriteApplicationCustomerRightBatchHistory(appChild.AppID, groupId, zeitstempel, DomainDbContext.ApplicationBatchZuordnungHistoryChangeType.Insert);
            }
        }

        private void RemoveApplicationGroupRightWithHistory(int appId, int groupId, DateTime zeitstempel)
        {
            var item = ApplicationGroupRights.FirstOrDefault(acr => acr.AppID == appId && acr.GroupID == groupId);
            ApplicationGroupRights.Remove(item);

            DbContext.WriteApplicationGroupRightBatchHistory(appId, groupId, zeitstempel, DomainDbContext.ApplicationBatchZuordnungHistoryChangeType.Delete);

            var childApps = Applications.Where(a => a.AppParent == appId).ToListOrEmptyList();
            foreach (var appChild in childApps)
            {
                var childItem = ApplicationGroupRights.FirstOrDefault(acr => acr.AppID == appChild.AppID && acr.GroupID == groupId);
                ApplicationGroupRights.Remove(childItem);

                DbContext.WriteApplicationGroupRightBatchHistory(appChild.AppID, groupId, zeitstempel, DomainDbContext.ApplicationBatchZuordnungHistoryChangeType.Delete);
            }
        }
    }
}
