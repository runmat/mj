using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Type;
using StockCapture.Models;

namespace StockCapture
{
    public class Repository : IDisposable
    {
        private readonly ISession _session;

        public static string ConnectionString
        {
            get
            {
                var connStringKey = (Environment.MachineName.ToUpper() == "CT114176" ? "ConnectionstringTM" : "ConnectionstringLocal");
                return ConfigurationManager.AppSettings[connStringKey];
            }
        }

        public Repository()
        {
            var sessionFactory = ConfigureNHibernate();
            _session = sessionFactory.OpenSession();
        }

        public void Dispose()
        {
            _session.Close();
            _session.Dispose();
        }


        public void Action()
        {
            //CreateObjects();

            LoadStockQuotes();
        }


        private static ISessionFactory ConfigureNHibernate()
        {
            return Fluently.Configure()
                            .Database(MsSqlConfiguration.MsSql2008.ConnectionString(ConnectionString))
                            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Repository>())
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

        public void Delete<T>(T item)
        {
            using (_session.BeginTransaction())
            {
                _session.Delete(item);
                _session.Transaction.Commit();
            }
        }

        /// <summary>
        /// Retrieves all objects of a given type.
        /// </summary>
        /// <typeparam name="T">The type of the objects to be retrieved.</typeparam>
        /// <returns>A list of all objects of the specified type.</returns>
        public IList<T> RetrieveAll<T>(int maxItems=-1, bool ascending=true)
        {
            /* Note that NHibernate guarantees that two object references will point to the
             * same object only if the references are set in the same session. For example,
             * Order #123 under the Customer object Able Inc and Order #123 in the Orders
             * list will point to the same object only if we load Customers and Orders in 
             * the same session. If we load them in different sessions, then changes that
             * we make to Able Inc's Order #123 will not be reflected in Order #123 in the
             * Orders list, since the references point to different objects. That's why we
             * maintain a session as a member variable, instead of as a local variable. */

            // Retrieve all objects of the type passed in
            var targetObjects = _session.CreateCriteria(typeof(T));
            if (maxItems >= 0)
                targetObjects.SetMaxResults(maxItems);
            targetObjects.AddOrder(new Order("ID", ascending));
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

        public void SaveStockQuote(StockQuote quote)
        {
            using (_session.BeginTransaction())
            {
                Save(quote);
            }
        }

        public List<StockQuote> LoadStockQuotes()
        {
            var stocks = RetrieveAll<StockQuote>();
            return stocks.ToList();
        }

        public List<StockQuote> GetLatestStockQuotes(int maxItems)
        {
            var stocks = RetrieveAll<StockQuote>(maxItems, false);
            return stocks.ToList();
        }
    }
}
