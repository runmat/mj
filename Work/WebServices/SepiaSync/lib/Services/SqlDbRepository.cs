using System;
using System.Collections.Generic;
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

        private IList<WebUser> _persons;


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


        public void Action()
        {
            LoadPersons();
        }

        private static ISessionFactory ConfigureNHibernate()
        {
            var conectionStringName = System.Configuration.ConfigurationManager.AppSettings["SqlDbConnectionStringName"];
            return Fluently.Configure()
                            .Database(MsSqlConfiguration.MsSql2008.ConnectionString(c => c.FromConnectionStringWithKey(conectionStringName)))
                            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<SqlDbRepository>())
                            .BuildSessionFactory();
        }

        /// <summary>
        /// Retrieves all objects of a given type.
        /// </summary>
        /// <typeparam name="T">The type of the objects to be retrieved.</typeparam>
        /// <returns>A list of all objects of the specified type.</returns>
        public IList<T> RetrieveAll<T>()
        {
            // Retrieve all objects of the type passed in
            var targetObjects = _session.CreateCriteria(typeof(T));
            var itemList = targetObjects.List<T>();

            // Set return value
            return itemList;
        }

        /// <summary>
        /// Retrieves objects of a specified type where a specified property equals a specified value.
        /// </summary>
        /// <typeparam name="T">The type of the objects to be retrieved.</typeparam>
        /// <param name="propertyName">The name of the property to be tested.</param>
        /// <param name="propertyValue">The value that the named property must hold.</param>
        /// <returns>A list of all objects meeting the specified criteria.</returns>
        public IList<T> RetrieveEquals<T>(string propertyName, object propertyValue)
        {
            // Create a criteria object with the specified criteria
            var criteria = _session.CreateCriteria(typeof(T));
            criteria.Add(Restrictions.Eq(propertyName, propertyValue));

            // Get the matching objects
            var matchingObjects = criteria.List<T>();

            // Set return value
            return matchingObjects;
        }

        private void LoadPersons()
        {
            _persons = RetrieveAll<WebUser>();

            var person = _persons.Single(p => p.UserID == 3006);
        }
    }
}
