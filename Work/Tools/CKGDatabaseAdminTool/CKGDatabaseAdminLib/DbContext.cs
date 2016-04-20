using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using CKGDatabaseAdminLib.Models;
using CKGDatabaseAdminLib.Services;
using GeneralTools.Models;

namespace CKGDatabaseAdminLib
{
    public class DatabaseContext : DbContext
    {
        public int? CurrentAppId { get; set; }

        public string CurrentAppURL { get; set; }

        public int? CurrentBapiId { get; set; }

        public DatabaseContext(string connectionString) : base(Config.EnsurePwd(connectionString)) { }

        public DbSet<LoginUserMessage> LoginUserMessages { get; set; }

        public DbSet<GitBranchInfo> GitBranchInfos { get; set; }

        public DbSet<CkgEntwickler> CkgEntwickler { get; set; }

        public DbSet<Application> Applications { get; set; }

        public ObservableCollection<Application> ApplicationsInMenuOnly
        {
            get
            {
                var liste = from a in Applications
                            where a.AppInMenu
                            select a;

                return new ObservableCollection<Application>(liste);
            }
        }

        public DbSet<BapiTable> Bapis { get; set; }

        public ObservableCollection<BapiTable> BapisSorted
        {
            get
            {
                var liste = from b in Bapis
                            orderby b.BAPI
                            select b;

                return new ObservableCollection<BapiTable>(liste);
            }       
        }

        public DbSqlQuery<BapiTable> GetBapisForApplication()
        {
            var query = "SELECT b.* FROM BAPI b, ApplicationBapi a WHERE a.BapiId = b.ID";
            if (CurrentAppId.HasValue)
            {
                query += " AND a.ApplicationId = " + CurrentAppId.Value.ToString();
            }
            query += " ORDER BY b.BAPI";
            return Bapis.SqlQuery(query);
        }

        public void SaveBapiForApplication(int bapiId)
        {
            if (CurrentAppId.HasValue)
            {
                Database.ExecuteSqlCommand("INSERT INTO ApplicationBapi (ApplicationID, BapiID) VALUES ({0}, {1})", CurrentAppId.Value, bapiId);
            }    
        }

        public void RemoveBapiForApplication(int bapiId)
        {
            if (CurrentAppId.HasValue)
            {
                Database.ExecuteSqlCommand("DELETE FROM ApplicationBapi WHERE ApplicationID = {0} AND BapiID = {1}", CurrentAppId.Value, bapiId);
            }
        }

        public DbSqlQuery<Application> GetApplicationsForBapi()
        {
            var query = "SELECT a.* FROM Application a, ApplicationBapi ab WHERE ab.ApplicationId = a.AppId";
            if (CurrentBapiId.HasValue)
            {
                query += " AND ab.BapiId = " + CurrentBapiId.Value.ToString();
            }
            query += " ORDER BY a.AppId";
            return Applications.SqlQuery(query);
        }

        public DbSet<Bapi2Report> Bapi2ReportItemsRaw { get; set; }

        public List<Bapi2Report4CsvExport> GetBapi2Report4CsvExportAggregatedItems()
        {
            var liste = new List<Bapi2Report4CsvExport>();

            var rawItems = Bapi2ReportItemsRaw.ToList();

            foreach (var rawItem in rawItems)
            {
                var item = rawItem;

                var strKey = item.KUNNR + "-" + item.Customername + "-" + item.AppFriendlyName;
                if (liste.None(i => i.KUNNR == item.KUNNR && i.Customername == item.Customername && i.AppFriendlyName == item.AppFriendlyName))
                {
                    var bapiList = Bapi2ReportItemsRaw.Where(x => (x.KUNNR + "-" + x.Customername + "-" + x.AppFriendlyName) == strKey && !String.IsNullOrEmpty(x.BAPI)).Select(x => x.BAPI).ToList();
                    var bapiString = String.Join("|", bapiList);

                    liste.Add(new Bapi2Report4CsvExport
                        {
                            KUNNR = item.KUNNR,
                            Customername = item.Customername,
                            AppFriendlyName = item.AppFriendlyName.NotNullOrEmpty().Replace(";", "").Replace("\r", "").Replace("\n", ""),
                            AppName = item.AppName,
                            AppURL = item.AppURL,
                            BAPI = bapiString.NotNullOrEmpty().ToUpper()
                        });
                }
            }

            return liste;
        }

        public DbSet<BapiCheckItem> BapiCheckItems { get; set; }

        public List<BapiCheckItem> GetBapiCheckItemsForCheck(bool testSap)
        {
            List<BapiCheckItem> liste = (
                from s in BapiCheckItems
                where s.TestSap == testSap
                select s
            ).ToList();

            return liste;
        }

        public void SaveBapiCheckItem(string bapi, byte[] bapiStruktur, bool neu, bool testSap)
        {
            BapiCheckItem itemToSave;

            if (neu)
            {
                itemToSave = BapiCheckItems.Create();
                itemToSave.BapiName = bapi;
                itemToSave.TestSap = testSap;
            }
            else
            {
                itemToSave = BapiCheckItems.First(b => b.BapiName == bapi && b.TestSap == testSap);
            }

            itemToSave.BapiStruktur = bapiStruktur;
            itemToSave.Updated = DateTime.Now;

            if (neu)
                BapiCheckItems.Add(itemToSave);

            SaveChanges();
        }

        public List<Application> GetChildApplicationsForApplication()
        {
            List<Application> liste = new List<Application>();
            if (CurrentAppId.HasValue)
            {
                liste = (
                    from a in Applications
                    where a.AppParent == CurrentAppId
                    select a
                ).ToList();
            }
            return liste;
        }

        public DbSet<ApplicationField> ApplicationFields { get; set; }

        public List<ApplicationField> GetApplicationFieldsForApplication()
        {
            List<ApplicationField> liste = new List<ApplicationField>();
            if (!String.IsNullOrEmpty(CurrentAppURL))
            {
                liste = (
                    from f in ApplicationFields
                    where f.CustomerID == 1 && f.AppURL.ToUpper() == CurrentAppURL.ToUpper()
                    select f
                ).ToList();
            }   
            return liste;
        }

        public List<ApplicationField> GetApplicationFieldsForApplication(string appUrl)
        {
            List<ApplicationField> liste = new List<ApplicationField>();
            if (!String.IsNullOrEmpty(appUrl))
            {
                liste = (
                    from f in ApplicationFields
                    where f.CustomerID == 1 && f.AppURL.ToUpper() == appUrl.ToUpper()
                    select f
                ).ToList();
            }
            return liste;
        }

        public DbSet<ColumnTranslation> ColumnTranslations { get; set; }

        public List<ColumnTranslation> GetColumnTranslationsForApplication()
        {
            List<ColumnTranslation> liste = new List<ColumnTranslation>();
            if (CurrentAppId.HasValue)
            {
                liste = (
                    from t in ColumnTranslations
                    where t.AppID == CurrentAppId.Value
                    select t
                ).ToList();
            }
            return liste;
        }

        public List<ColumnTranslation> GetColumnTranslationsForApplication(int? appId)
        {
            List<ColumnTranslation> liste = new List<ColumnTranslation>();
            if (appId.HasValue)
            {
                liste = (
                    from t in ColumnTranslations
                    where t.AppID == appId.Value
                    select t
                ).ToList();
            }
            return liste;
        }

        public DbSet<ApplicationConfig> ApplicationConfigs { get; set; }

        public List<ApplicationConfig> GetApplicationConfigsForApplication()
        {
            List<ApplicationConfig> liste = new List<ApplicationConfig>();
            if (CurrentAppId.HasValue)
            {
                liste = (
                    from c in ApplicationConfigs
                    where c.CustomerID == 1 && c.AppID == CurrentAppId.Value
                    select c
                ).ToList();
            }
            return liste;
        }

        public List<ApplicationConfig> GetApplicationConfigsForApplication(int? appId)
        {
            List<ApplicationConfig> liste = new List<ApplicationConfig>();
            if (appId.HasValue)
            {
                liste = (
                    from c in ApplicationConfigs
                    where c.ConfigID == 1 && c.AppID == appId.Value
                    select c
                ).ToList();
            }
            return liste;
        }

        public void ClearApplicationFieldsForApplication(string appUrl)
        {
            if (!String.IsNullOrEmpty(appUrl))
            {
                Database.ExecuteSqlCommand("DELETE FROM ApplicationField WHERE UPPER(AppURL) = {0}", appUrl.ToUpper());
            }
        }

        public void CopyApplicationFields(IEnumerable<ApplicationField> fields)
        {
            foreach (var item in fields)
            {
                var newItem = ApplicationFields.Create();
                newItem.AppURL = item.AppURL;
                newItem.Content = item.Content;
                newItem.CustomerID = item.CustomerID;
                newItem.FieldName = item.FieldName;
                newItem.FieldType = item.FieldType;
                newItem.LanguageID = item.LanguageID;
                newItem.ToolTip = item.ToolTip;
                newItem.Visibility = item.Visibility;
                ApplicationFields.Add(newItem);
            }
            SaveChanges();
        }

        public void CopyApplication(Application appToCopy, IEnumerable<ApplicationField> fields, IEnumerable<ColumnTranslation> cols, IEnumerable<ApplicationConfig> configs, bool appIsChild)
        {
            var newApp = Applications.Create();
            newApp.AppComment = appToCopy.AppComment;
            newApp.AppDescription = appToCopy.AppDescription;
            newApp.AppFriendlyName = appToCopy.AppFriendlyName;
            newApp.AppInMenu = appToCopy.AppInMenu;
            newApp.AppName = appToCopy.AppName;
            newApp.AppParent = appToCopy.AppParent;
            newApp.AppRank = appToCopy.AppRank;
            newApp.AppSchwellwert = appToCopy.AppSchwellwert;
            newApp.AppTechType = appToCopy.AppTechType;
            newApp.AppType = appToCopy.AppType;
            newApp.AppURL = appToCopy.AppURL;
            newApp.AuthorizationLevel = appToCopy.AuthorizationLevel;
            newApp.BatchAuthorization = appToCopy.BatchAuthorization;
            newApp.LogDuration = appToCopy.LogDuration;
            newApp.MaxLevel = appToCopy.MaxLevel;
            newApp.MaxLevelsPerGroup = appToCopy.MaxLevelsPerGroup;
            Applications.Add(newApp);

            SaveChanges();

            if (!appIsChild)
            {
                CurrentAppId = newApp.AppID;
                CurrentAppURL = newApp.AppURL;
            }

            if (fields != null)
            {
                ClearApplicationFieldsForApplication(newApp.AppURL);
                CopyApplicationFields(fields);
            }

            if (cols != null)
            {
                foreach (var item in cols)
                {
                    var newItem = ColumnTranslations.Create();
                    newItem.ABEDaten = item.ABEDaten;
                    newItem.Alignment = item.Alignment;
                    newItem.AppID = newApp.AppID;
                    newItem.DisplayOrder = item.DisplayOrder;
                    newItem.IstDatum = item.IstDatum;
                    newItem.IstZeit = item.IstZeit;
                    newItem.NewName = item.NewName;
                    newItem.NullenEntfernen = item.NullenEntfernen;
                    newItem.OrgName = item.OrgName;
                    newItem.TextBereinigen = item.TextBereinigen;
                    ColumnTranslations.Add(newItem);
                }
            }

            if (configs != null)
            {
                foreach (var item in configs)
                {
                    var newItem = ApplicationConfigs.Create();
                    newItem.AppID = newApp.AppID;
                    newItem.ConfigKey = item.ConfigKey;
                    newItem.ConfigType = item.ConfigType;
                    newItem.ConfigValue = item.ConfigValue;
                    newItem.CustomerID = item.CustomerID;
                    newItem.Description = item.Description;
                    newItem.GroupID = item.GroupID;
                    ApplicationConfigs.Add(newItem);
                }
            }

            SaveChanges();
        }

        public void ExecuteSqlNonQuery(string sqlString)
        {
            Database.ExecuteSqlCommand(sqlString);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            Database.SetInitializer<DatabaseContext>(null);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>(); 
        }
    }
}
