using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.Entity;
using CKGDatabaseAdminLib.Contracts;
using CkgDomainLogic.General.Services;
using CKGDatabaseAdminLib.Models.DbModels;

namespace CKGDatabaseAdminLib.Services
{
    public class FieldTranslationCopyDataServiceSql : CkgGeneralDataService, IFieldTranslationCopyDataService
    {
        public ObservableCollection<ApplicationInfo> Applications { get { return _dataContext.Applications.Local; } }

        public ObservableCollection<ApplicationField> FieldTranslations { get; set; }

        private DatabaseContext _dataContext;

        public bool IsInEditMode { get { return (_dataContext != null && _dataContext.CurrentAppId != null); } }

        public FieldTranslationCopyDataServiceSql(string connectionName)
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
            FieldTranslations = new ObservableCollection<ApplicationField>(_dataContext.GetApplicationFieldsForApplication());
        }

        public void ResetCurrentApp()
        {
            _dataContext.CurrentAppId = null;
            _dataContext.CurrentAppURL = "";
            if (FieldTranslations != null)
                FieldTranslations.Clear();
        }

        public void CopyFieldTranslations(string destinationDb)
        {
            var sectionData = (NameValueCollection)ConfigurationManager.GetSection("dbConnections");

            using (var destinationDbContext = new DatabaseContext(sectionData.Get(destinationDb)))
            {
                destinationDbContext.ClearApplicationFieldsForApplication(_dataContext.CurrentAppURL);
                destinationDbContext.CopyApplicationFields(FieldTranslations);
            }
        }
    }
}