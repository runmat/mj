using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.Entity;
using CKGDatabaseAdminLib.Contracts;
using CkgDomainLogic.General.Services;
using CKGDatabaseAdminLib.Models;
using System.Collections.ObjectModel;

namespace CKGDatabaseAdminLib.Services
{
    public class BapiCheckBapiDataService : CkgGeneralDataService, IBapiCheckDataService
    {
        public List<BapiCheckResult> BapiCheckResults { get; set; }

        private DatabaseContext _dataContext;

        private bool _testSap;

        public BapiCheckBapiDataService(string connectionName, bool testSap)
        {
            _testSap = testSap;
            InitDataContext(connectionName);
        }

        public void InitDataContext(string connectionName)
        {
            var sectionData = (NameValueCollection)ConfigurationManager.GetSection("dbConnections");
            _dataContext = new DatabaseContext(sectionData.Get(connectionName));

            _dataContext.Bapis.Load();
            _dataContext.BapiCheckItems.Load();
        }

        public void PerformBapiCheck()
        {
            BapiCheckResults = new List<BapiCheckResult>();

            foreach (var bapi in _dataContext.BapisSorted)
            {
                var result = new BapiCheckResult {BapiName = bapi.BAPI};

                //TODO: Abgleich

                BapiCheckResults.Add(result);
            }
        }
    }
}