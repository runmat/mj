using System.Collections.Generic;
using System.Linq;
using MvcTools.Models;
using MvcTools.Web;

namespace MvcTools.Data
{
    public class MvcRepository
    {
        private static MvcDbContext Ct { get { return SessionStoreAutoCreate<MvcDbContext>.Model; } }

        static public List<ContentEntity> ContentEntities
        {
            get { return SessionHelper.GetSessionList<ContentEntity>("ContentEntities", Ct.ContentEntities.ToList); }
        }
        static public void AddContentEntity(ContentEntity ce)
        {
            InsertOnSubmit(ce);
            SubmitChanges();
            SessionHelper.SetSessionValue<ContentEntity>("ContentEntities", null);
        }

        public static void SubmitChanges()
        {
            Ct.SaveChanges();
        }

        public static void InsertOnSubmit<TEntity>(TEntity entity) where TEntity : class
        {
            Ct.Set<TEntity>().Add(entity);
        }

        public static void DeleteOnSubmit<TEntity>(TEntity entity) where TEntity : class
        {
            Ct.Set<TEntity>().Remove(entity);
        }
    }
}
