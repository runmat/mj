// ReSharper disable RedundantUsingDirective

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Database.Models;
using CkgDomainLogic.General.Database.Services;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Services;
using SapORM.Contracts;

namespace CkgDomainLogic.DomainCommon.Services
{
    public class GridAdminDataServiceSql : CkgGeneralDataServiceSAP, IGridAdminDataService 
    {
        static string ConnectionStringWorkServer { get { return ConfigurationManager.AppSettings["ConnectionString"].NotNullOrEmpty(); } }

        static string ConnectionStringTestServer { get { return ConfigurationManager.AppSettings["ConnectionStringTestServer"].NotNullOrEmpty(); } }
        
        static string ConnectionStringProdServer { get { return ConfigurationManager.AppSettings["ConnectionStringProdServer"].NotNullOrEmpty(); } }


        public GridAdminDataServiceSql(ISapDataService sap)
            : base(sap)
        {
        }

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
            dbContextTestServer.TranslatedResourceUpdate(r, UserName);

            var dbContextProdServer = CreateDbContext(ConnectionStringProdServer);
            dbContextProdServer.TranslatedResourceUpdate(r, UserName);
        }

        public void TranslatedResourceCustomerUpdate(TranslatedResourceCustom r)
        {
            var dbContextTestServer = CreateDbContext(ConnectionStringTestServer);
            dbContextTestServer.TranslatedResourceCustomerUpdate(r, UserName);

            var dbContextProdServer = CreateDbContext(ConnectionStringProdServer);
            dbContextProdServer.TranslatedResourceCustomerUpdate(r, UserName);
        }

        public void TranslatedResourceDelete(TranslatedResource r)
        {
            var dbContextTestServer = CreateDbContext(ConnectionStringTestServer);
            dbContextTestServer.TranslatedResourceDelete(r, UserName);

            var dbContextProdServer = CreateDbContext(ConnectionStringProdServer);
            dbContextProdServer.TranslatedResourceDelete(r, UserName);
        }

        public void TranslatedResourceCustomerDelete(TranslatedResourceCustom r)
        {
            var dbContextTestServer = CreateDbContext(ConnectionStringTestServer);
            dbContextTestServer.TranslatedResourceCustomerDelete(r, UserName);

            var dbContextProdServer = CreateDbContext(ConnectionStringProdServer);
            dbContextProdServer.TranslatedResourceCustomerDelete(r, UserName);
        }

        #endregion


        public void TranslationsMarkForRefresh()
        {
            GeneralConfiguration.SetConfigValue("Localization", "TimeOfLastResourceUpdate", DateTime.Now.ToString("yyyyMMddHHmmss"));
        }
    }
}
