using System;
using System.Collections.Generic;
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
        public ObservableCollection<Application> Applications { get; private set; }

        public ObservableCollection<Application> ChildApplications { get; private set; }

        public ObservableCollection<ApplicationField> FieldTranslations { get; private set; }

        public ObservableCollection<ColumnTranslation> ColumnTranslations { get; private set; }

        public ObservableCollection<ApplicationConfig> ConfigurationValues { get; private set; }

        private DatabaseContext _dataContext;

        private DatabaseContext _destinationDataContext;

        public bool IsInEditMode { get { return (_dataContext != null && _dataContext.CurrentAppId != null); } }

        public ApplicationCopyDataServiceSql(string connectionName)
        {
            InitDataContext(connectionName);
        }

        public void InitDataContext(string connectionName)
        {
            var sectionData = Config.GetAllDbConnections();
            _dataContext = new DatabaseContext(sectionData.Get(connectionName));

            _dataContext.Applications.Load();
            Applications = _dataContext.ApplicationsInMenuOnly;
        }

        public void InitDestinationDataContext(string connectionName)
        {
            if (String.IsNullOrEmpty(connectionName))
            {
                _destinationDataContext = null;
            }
            else
            {
                var sectionData = Config.GetAllDbConnections();
                _destinationDataContext = new DatabaseContext(sectionData.Get(connectionName));

                _destinationDataContext.Applications.Load();
            }
        }

        public void FilterData(bool onlyNew)
        {
            IEnumerable<Application> listeTemp;

            var allApps = _dataContext.ApplicationsInMenuOnly;

            if (!onlyNew || _destinationDataContext == null)
            {
                listeTemp = allApps;
            }
            else
            {
                listeTemp = from a in allApps
                            where !_destinationDataContext.Applications.Any(d => d.AppURL.ToLower() == a.AppURL.ToLower())
                            select a;
            }

            Applications = new ObservableCollection<Application>(listeTemp.OrderBy(g => g.AppID));
        }

        public void BeginEdit(int appId, string appURL)
        {
            _dataContext.CurrentAppId = appId;
            _dataContext.CurrentAppURL = appURL;
            ChildApplications = new ObservableCollection<Application>(_dataContext.GetChildApplicationsForApplication());
            FieldTranslations = new ObservableCollection<ApplicationField>(_dataContext.GetApplicationFieldsForApplication());
            ColumnTranslations = new ObservableCollection<ColumnTranslation>(_dataContext.GetColumnTranslationsForApplication());
            ConfigurationValues = new ObservableCollection<ApplicationConfig>(_dataContext.GetApplicationConfigsForApplication());
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
            if (ConfigurationValues != null)
                ConfigurationValues.Clear();
        }

        /// <summary>
        /// Kopiert eine Anwendung (ggf. inkl. Child-Apps, Feld-/Spaltenübersetzungen und Config-Einstellungen) in eine andere DB
        /// </summary>
        /// <param name="blnChildApplications"></param>
        /// <param name="blnFieldTranslations"></param>
        /// <param name="blnColumnTranslations"></param>
        /// <param name="blnConfigurationValues"></param>
        /// <returns>ID der App-Kopie</returns>
        public int? CopyApplication(bool blnChildApplications, bool blnFieldTranslations, bool blnColumnTranslations, bool blnConfigurationValues)
        {
            int? neueID;

            var currentApp = _dataContext.Applications.Single(a => a.AppID == _dataContext.CurrentAppId);

            _destinationDataContext.CopyApplication(currentApp, (blnFieldTranslations ? FieldTranslations : null), (blnColumnTranslations ? ColumnTranslations : null), (blnConfigurationValues ? ConfigurationValues : null), false);
            neueID = _destinationDataContext.CurrentAppId;
            if (blnChildApplications)
            {
                var childApps = _dataContext.Applications.Where(a => a.AppParent == _dataContext.CurrentAppId).ToList();
                foreach (var item in childApps)
                {
                    var childFieldTranslations = (blnFieldTranslations ? _dataContext.GetApplicationFieldsForApplication(item.AppURL) : null);
                    var childColumnTranslations = (blnColumnTranslations ? _dataContext.GetColumnTranslationsForApplication(item.AppID) : null);
                    var childConfigurationValues = (blnConfigurationValues ? _dataContext.GetApplicationConfigsForApplication(item.AppID) : null);
                    _destinationDataContext.CopyApplication(item, childFieldTranslations, childColumnTranslations, childConfigurationValues, true);
                }
            }

            return neueID;
        }
    }
}