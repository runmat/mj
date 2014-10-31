using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Criterion;
using SepiaSyncLib.Models;

namespace SepiaSyncLib.Services
{
    public class SqlDbRepository : IDisposable
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly ISession _session;

        private IList<WebUserSepiaAccess> _users;


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
        
        public bool SetSepiaSyncDateForUser(WebUserSepiaAccess user)
        {
            using (_session.BeginTransaction())
            {
                user.SepiaSyncDate = DateTime.Now;
                Save(user);
            }

            return true;
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

        public void Save<T>(T item)
        {
            using (_session.BeginTransaction())
            {
                _session.SaveOrUpdate(item);
                _session.Transaction.Commit();
            }
        }

        public IList<T> RetrieveAll<T>()
        {
            // Retrieve all objects of the type passed in
            var targetObjects = _session.CreateCriteria(typeof (T));
            var itemList = targetObjects.List<T>();

            return itemList;
        }

        public IList<T> RetrieveEquals<T>(string propertyName, object propertyValue)
        {
            // Create a criteria object with the specified criteria
            var criteria = _session.CreateCriteria(typeof (T));
            criteria.Add(Restrictions.Eq(propertyName, propertyValue));

            // Get the matching objects
            var matchingObjects = criteria.List<T>();

            // Set return value
            return matchingObjects;
        }


        #endregion
    }
}
