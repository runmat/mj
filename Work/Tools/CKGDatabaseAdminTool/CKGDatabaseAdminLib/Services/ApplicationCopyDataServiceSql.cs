using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using CKGDatabaseAdminLib.Contracts;
using CkgDomainLogic.General.Services;
using CKGDatabaseAdminLib.Models;

namespace CKGDatabaseAdminLib.Services
{
    public class ApplicationCopyDataServiceSql : CkgGeneralDataService, IApplicationCopyDataService
    {
        public ObservableCollection<ApplicationInfo> Applications { get { return _dataContext.ApplicationsInMenuOnly; } }

        public ObservableCollection<ApplicationInfo> ChildApplications { get; set; }

        public ObservableCollection<ApplicationField> FieldTranslations { get; set; }

        public ObservableCollection<ColumnTranslation> ColumnTranslations { get; set; }

        private DatabaseContext _dataContext;

        public bool IsInEditMode { get { return (_dataContext != null && _dataContext.CurrentAppId != null); } }

        public ApplicationCopyDataServiceSql(string connectionName)
        {
            InitDataContext(connectionName);
        }

        public void InitDataContext(string connectionName)
        {
            var sectionData = (NameValueCollection)ConfigurationManager.GetSection("dbConnections");
            _dataContext = new DatabaseContext(sectionData.Get(connectionName));

            _dataContext.Applications.Load();
        }

        public void BeginEdit(int appId, string appURL)
        {
            _dataContext.CurrentAppId = appId;
            _dataContext.CurrentAppURL = appURL;
            ChildApplications = new ObservableCollection<ApplicationInfo>(_dataContext.GetChildApplicationsForApplication());
            FieldTranslations = new ObservableCollection<ApplicationField>(_dataContext.GetApplicationFieldsForApplication());
            ColumnTranslations = new ObservableCollection<ColumnTranslation>(_dataContext.GetColumnTranslationsForApplication());
        }

        public void ResetCurrentApp()
        {
            _dataContext.CurrentAppId = null;
            _dataContext.CurrentAppURL = "";
            if (ChildApplications != null)
                ChildApplications.Clear();
            if (FieldTranslations != null)
                FieldTranslations.Clear();
            if (ColumnTranslations != null)
                ColumnTranslations.Clear();
        }

        /// <summary>
        /// Kopiert eine Anwendung (ggf. inkl. Child-Apps, Feld- und Spaltenübersetzungen) in eine andere DB
        /// </summary>
        /// <param name="destinationDb"></param>
        /// <param name="blnChildApplications"></param>
        /// <param name="blnFieldTranslations"></param>
        /// <param name="blnColumnTranslations"></param>
        /// <returns>ID der App-Kopie</returns>
        public int? CopyApplication(string destinationDb, bool blnChildApplications, bool blnFieldTranslations, bool blnColumnTranslations)
        {
            int? neueID;

            var currentApp = _dataContext.Applications.Single(a => a.AppID == _dataContext.CurrentAppId);

            var sectionData = (NameValueCollection)ConfigurationManager.GetSection("dbConnections");

            using (var destinationDbContext = new DatabaseContext(sectionData.Get(destinationDb)))
            {
                destinationDbContext.CopyApplication(currentApp, (blnFieldTranslations ? FieldTranslations : null), (blnColumnTranslations ? ColumnTranslations : null), false);
                neueID = destinationDbContext.CurrentAppId;
                if (blnChildApplications)
                {
                    var childApps = _dataContext.Applications.Where(a => a.AppParent == _dataContext.CurrentAppId).ToList();
                    foreach (var item in childApps)
                    {
                        var childFieldTranslations = (blnFieldTranslations ? _dataContext.GetApplicationFieldsForApplication(item.AppURL) : null);
                        var childColumnTranslations = (blnColumnTranslations ? _dataContext.GetColumnTranslationsForApplication(item.AppID) : null);
                        destinationDbContext.CopyApplication(item, childFieldTranslations, childColumnTranslations, true);
                    }
                }
            }

            return neueID;
        }
    }
}