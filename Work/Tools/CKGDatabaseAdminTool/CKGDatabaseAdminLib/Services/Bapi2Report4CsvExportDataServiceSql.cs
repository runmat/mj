using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using CKGDatabaseAdminLib.Contracts;
using CkgDomainLogic.General.Services;
using CKGDatabaseAdminLib.Models.DbModels;

namespace CKGDatabaseAdminLib.Services
{
    public class Bapi2Report4CsvExportDataServiceSql : CkgGeneralDataService, IBapi2Report4CsvExportDataService
    {
        public Dictionary<string, Bapi2Report4CsvExport> ListItems { get { return _dataContext.GetBapi2Report4CsvExportAggregatedItems(); } }

        private DatabaseContext _dataContext;

        public Bapi2Report4CsvExportDataServiceSql()
        {
            InitDataContext();
        }

        private void InitDataContext()
        {
            _dataContext = new DatabaseContext(ConfigurationManager.AppSettings["Connectionstring"]);

            _dataContext.Bapi2Report4CsvExportItemsRaw.Load();
        }
    }
}