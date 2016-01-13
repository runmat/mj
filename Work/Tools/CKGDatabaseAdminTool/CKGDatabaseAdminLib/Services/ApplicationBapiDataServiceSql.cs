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
    public class ApplicationBapiDataServiceSql : CkgGeneralDataService, IApplicationBapiDataService
    {
        public ObservableCollection<Application> Applications { get { return _dataContext.Applications.Local; } }

        public ObservableCollection<BapiTable> Bapis { get { return _dataContext.BapisSorted; } }

        public ObservableCollection<BapiTable> ApplicationBapis { get; set; }

        private DatabaseContext _dataContext;

        public bool IsInEditMode { get { return (_dataContext != null && _dataContext.CurrentAppId != null); } }

        public ApplicationBapiDataServiceSql(string connectionName)
        {
            InitDataContext(connectionName);
        }

        public void InitDataContext(string connectionName)
        {
            var sectionData = Config.GetAllDbConnections();
            _dataContext = new DatabaseContext(sectionData.Get(connectionName));

            _dataContext.Applications.Load();
            _dataContext.Bapis.Load();
        }

        public int AddBapi(string name)
        {
            int erg = 0;

            var existingItem = Bapis.FirstOrDefault(b => b.BAPI.ToUpper() == name.ToUpper());
            if (existingItem == null)
            {
                var newItem = new BapiTable { BAPI = name };
                _dataContext.Bapis.Add(newItem);
                _dataContext.SaveChanges();
                existingItem = Bapis.FirstOrDefault(b => b.BAPI == name);
            }
            if (existingItem != null)
            {
                erg = existingItem.ID;
            }
            return erg;
        }

        public void BeginEdit(int appId)
        {
            _dataContext.CurrentAppId = appId;
            ApplicationBapis = new ObservableCollection<BapiTable>(_dataContext.GetBapisForApplication());
        }

        public void ResetCurrentAppId()
        {
            _dataContext.CurrentAppId = null;
            if (ApplicationBapis != null)
            {
                ApplicationBapis.Clear();
            }
        }

        public void AddApplicationBapi(int bapiId)
        {
            var existingItem = ApplicationBapis.FirstOrDefault(b => b.ID == bapiId);
            if (existingItem == null)
            {
                _dataContext.SaveBapiForApplication(bapiId);
                ApplicationBapis = new ObservableCollection<BapiTable>(_dataContext.GetBapisForApplication());
            }
        }

        public void DeleteApplicationBapi(int bapiId)
        {
            _dataContext.RemoveBapiForApplication(bapiId);
            ApplicationBapis = new ObservableCollection<BapiTable>(_dataContext.GetBapisForApplication());
        }
    }
}