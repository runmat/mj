using System;
using System.Collections.Generic;
using System.Configuration;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using SepiaSyncLib.Models;

namespace SepiaSyncLib.Services
{
    public class SqlDbRepository : IDisposable
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly ISession _session;


        public SqlDbRepository()
        {
            _sessionFactory = ConfigureNHibernate();
            _session = _sessionFactory.OpenSession();
        }

        public void Dispose()
        {
            _session.Close();
            _session.Dispose();
        }

        
        public IList<WebUserSepiaAccess> GetUsers()
        {
            return RetrieveAll<WebUserSepiaAccess>();
        }

        public void SetSepiaSyncDateForUser(WebUserSepiaAccess user)
        {
            using (_session.BeginTransaction())
            {
                user.SepiaSyncDate = DateTime.Now;
                user.SepiaSyncStatus = "OK";
                Save(user);
            }
        }

        public void SetSepiaSyncStatusForUser(WebUserSepiaAccess user, string status)
        {
            using (_session.BeginTransaction())
            {
                user.SepiaSyncStatus = status;
                Save(user);
            }
        }


        #region Helpers

        private static ISessionFactory ConfigureNHibernate()
        {
            var conectionStringName = ConfigurationManager.AppSettings["SqlDbConnectionStringName"];

            return Fluently.Configure()
                           .Database(
                               MsSqlConfiguration.MsSql2008.ConnectionString(
                                   c => c.FromConnectionStringWithKey(conectionStringName)))
                           .Mappings(m => m.FluentMappings.AddFromAssemblyOf<SqlDbRepository>())
                           .BuildSessionFactory();
        }

        private void Save<T>(T item)
        {
            using (_session.BeginTransaction())
            {
                _session.SaveOrUpdate(item);
                _session.Transaction.Commit();
            }
        }

        private IList<T> RetrieveAll<T>()
        {
            // Retrieve all objects of the type passed in
            var targetObjects = _session.CreateCriteria(typeof (T));
            var itemList = targetObjects.List<T>();

            return itemList;
        }

        #endregion
    }
}
