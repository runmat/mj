// ReSharper disable RedundantUsingDirective

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Database.Models;
using CkgDomainLogic.General.Database.Services;
using GeneralTools.Models;
using GeneralTools.Services;

namespace CkgDomainLogic.DomainCommon.Services
{
    public class GridAdminDataServiceSql : IGridAdminDataService 
    {
        static string ConnectionStringWorkServer { get { return ConfigurationManager.AppSettings["ConnectionString"].NotNullOrEmpty(); } }

        static string ConnectionStringTestServer { get { return ConfigurationManager.AppSettings["ConnectionStringTestServer"].NotNullOrEmpty(); } }
        
        static string ConnectionStringProdServer { get { return ConfigurationManager.AppSettings["ConnectionStringProdServer"].NotNullOrEmpty(); } }


        static DomainDbContext CreateDbContext(string connectionString)
        {
            return new DomainDbContext(connectionString, "");
        }

        public List<Customer> GetCustomers()
        {
            var ct = CreateDbContext(ConnectionStringWorkServer);
            return ct.GetAllCustomer();
        }

        public List<User> GetUsersForCustomer(Customer customer)
        {
            var ct = CreateDbContext(ConnectionStringWorkServer);
            return ct.GetUserForCustomer(customer);
        }

        public string GetAppUrl(int appId)
        {
            var ct = CreateDbContext(ConnectionStringWorkServer);
            return ct.Database.SqlQuery<string>("select AppURL from Application where AppID = {0}", appId).FirstOrDefault();
        }

        public string GetAppFriendlyName(int appId)
        {
            var ct = CreateDbContext(ConnectionStringWorkServer);
            return ct.Database.SqlQuery<string>("select AppFriendlyName from Application where AppID = {0}", appId).FirstOrDefault();
        }


        #region Translated Resource

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

        #endregion


        public void TranslationsMarkForRefresh()
        {
            GeneralConfiguration.SetConfigValue("Localization", "TimeOfLastResourceUpdate", DateTime.Now.ToString("yyyyMMddHHmmss"));
        }
    }
}
