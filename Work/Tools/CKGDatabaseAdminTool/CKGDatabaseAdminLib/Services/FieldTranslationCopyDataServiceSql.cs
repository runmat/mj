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
    public class FieldTranslationCopyDataServiceSql : CkgGeneralDataService, IFieldTranslationCopyDataService
    {
        public ObservableCollection<Application> Applications { get; private set; }

        public ObservableCollection<ApplicationField> FieldTranslations { get; set; }

        private DatabaseContext _dataContext;

        private DatabaseContext _destinationDataContext;

        public bool IsInEditMode { get { return (_dataContext != null && _dataContext.CurrentAppId != null); } }

        public FieldTranslationCopyDataServiceSql(string connectionName)
        {
            InitDataContext(connectionName);
        }

        public void InitDataContext(string connectionName)
        {
            var sectionData = Config.GetAllDbConnections();
            _dataContext = new DatabaseContext(sectionData.Get(connectionName));

            _dataContext.Applications.Load();
            Applications = _dataContext.Applications.Local;
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

            var allApps = _dataContext.Applications.Local;

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
            FieldTranslations = new ObservableCollection<ApplicationField>(_dataContext.GetApplicationFieldsForApplication());
        }

        public void ResetCurrentApp()
        {
            _dataContext.CurrentAppId = null;
            _dataContext.CurrentAppURL = "";
            if (FieldTranslations != null)
                FieldTranslations.Clear();
        }

        public void CopyFieldTranslations()
        {
            _destinationDataContext.ClearApplicationFieldsForApplication(_dataContext.CurrentAppURL);
            _destinationDataContext.CopyApplicationFields(FieldTranslations);
        }
    }
}