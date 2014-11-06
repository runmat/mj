using System.Collections.Generic;
using System.Data.Entity;
using CKGDatabaseAdminLib.Contracts;
using CkgDomainLogic.General.Services;
using CKGDatabaseAdminLib.Models;

namespace CKGDatabaseAdminLib.Services
{
    public class Bapi2Report4CsvExportDataServiceSql : CkgGeneralDataService, IBapi2Report4CsvExportDataService
    {
        public Dictionary<string, Bapi2Report4CsvExport> ListItems { get { return _dataContext.GetBapi2Report4CsvExportAggregatedItems(); } }

        private DatabaseContext _dataContext;

        public Bapi2Report4CsvExportDataServiceSql(string connectionString)
        {
            InitDataContext(connectionString);
        }

        private void InitDataContext(string connectionString)
        {
            _dataContext = new DatabaseContext(connectionString);

            _dataContext.Bapi2Report4CsvExportItemsRaw.Load();
        }
    }
}