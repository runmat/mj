using System.Collections.Specialized;
using System.Configuration;
using CKGDatabaseAdminLib.Contracts;
using CkgDomainLogic.General.Services;

namespace CKGDatabaseAdminLib.Services
{
    public class SqlExecutionDataServiceSql : CkgGeneralDataService, ISqlExecutionDataService
    {
        private DatabaseContext _destinationDataContext;

        public void InitDestinationDataContext(string connectionName)
        {
            var sectionData = (NameValueCollection)ConfigurationManager.GetSection("dbConnections");
            _destinationDataContext = new DatabaseContext(sectionData.Get(connectionName));
        }

        public void ExecuteSqlNonQuery(string sqlString)
        {
            _destinationDataContext.ExecuteSqlNonQuery(sqlString);
        }
    }
}