// ReSharper disable RedundantUsingDirective
using System.Collections.Generic;
using System.Configuration;
using CkgDomainLogic.DomainCommon.Contracts;
using System.Linq;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Database.Models;
using CkgDomainLogic.General.Database.Services;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;

namespace CkgDomainLogic.DomainCommon.Services
{
    public class GridAdminDataServiceSql : IGridAdminDataService 
    {
        static string ConnectionStringTestServer { get { return ConfigurationManager.AppSettings["ConnectionStringTestServer"].NotNullOrEmpty(); } }
        static string ConnectionStringProdServer { get { return ConfigurationManager.AppSettings["ConnectionStringProdServer"].NotNullOrEmpty(); } }

        static DomainDbContext CreateDbContext(string connectionString)
        {
            return new DomainDbContext(connectionString, "");
        }

        public TranslatedResource TranslatedResourceLoad(string resourceKey)
        {
            var dbContextTestServer = CreateDbContext(ConnectionStringTestServer);
            return dbContextTestServer.TranslatedResourceLoad(resourceKey);
        }

        public TranslatedResourceCustom TranslatedResourceCustomerLoad(string resourceKey, int customerID)
        {
            var dbContextTestServer = CreateDbContext(ConnectionStringTestServer);
            return dbContextTestServer.TranslatedResourceCustomerLoad(resourceKey, customerID);
        }

        public void TranslatedResourceUpdate(TranslatedResource r)
        {
            var dbContextTestServer = CreateDbContext(ConnectionStringTestServer);
            dbContextTestServer.TranslatedResourceUpdate(r);

            var dbContextProdServer = CreateDbContext(ConnectionStringProdServer);
            dbContextProdServer.TranslatedResourceUpdate(r);
        }

        public void TranslatedResourceCustomerUpdate(TranslatedResourceCustom r)
        {
            var dbContextTestServer = CreateDbContext(ConnectionStringTestServer);
            dbContextTestServer.TranslatedResourceCustomerUpdate(r);

            var dbContextProdServer = CreateDbContext(ConnectionStringProdServer);
            dbContextProdServer.TranslatedResourceCustomerUpdate(r);
        }

        public void TranslatedResourceCustomerDelete(TranslatedResourceCustom r)
        {
            var dbContextTestServer = CreateDbContext(ConnectionStringTestServer);
            dbContextTestServer.TranslatedResourceCustomerDelete(r);

            var dbContextProdServer = CreateDbContext(ConnectionStringProdServer);
            dbContextProdServer.TranslatedResourceCustomerDelete(r);
        }
    }
}
