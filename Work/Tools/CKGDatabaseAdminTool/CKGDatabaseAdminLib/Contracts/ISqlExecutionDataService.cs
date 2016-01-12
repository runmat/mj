using CkgDomainLogic.General.Contracts;

namespace CKGDatabaseAdminLib.Contracts
{
    public interface ISqlExecutionDataService : ICkgGeneralDataService
    {
        void InitDestinationDataContext(string connectionName);

        void ExecuteSqlNonQuery(string sqlString);
    }
}