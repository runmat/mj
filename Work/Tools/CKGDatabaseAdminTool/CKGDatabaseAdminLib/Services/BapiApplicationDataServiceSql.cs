using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.Entity;
using CKGDatabaseAdminLib.Contracts;
using CkgDomainLogic.General.Services;
using CKGDatabaseAdminLib.Models;

namespace CKGDatabaseAdminLib.Services
{
    public class BapiApplicationDataServiceSql : CkgGeneralDataService, IBapiApplicationDataService
    {
        public ObservableCollection<Application> Applications { get { return _dataContext.Applications.Local; } }

        public ObservableCollection<BapiTable> Bapis { get { return _dataContext.BapisSorted; } }

        public ObservableCollection<Application> BapiApplications { get; set; }

        private DatabaseContext _dataContext;

        public BapiApplicationDataServiceSql(string connectionName)
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

        public void GetBapiUsage(int bapiId)
        {
            _dataContext.CurrentBapiId = bapiId;
            BapiApplications = new ObservableCollection<Application>(_dataContext.GetApplicationsForBapi());
        }

        public void ResetCurrentBapiId()
        {
            _dataContext.CurrentBapiId = null;
            if (BapiApplications != null)
            {
                BapiApplications.Clear();
            }
        }
    }
}