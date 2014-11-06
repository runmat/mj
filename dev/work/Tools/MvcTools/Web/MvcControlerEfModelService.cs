using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq;
using MvcTools.Data;

namespace MvcTools.Web
{
    public class MvcControlerEfModelService<T> where T : class, new()
    {
        #region POPUP EDITING

        static public bool Update(DbSet<T> dbEntities, Predicate<T> tryUpdateModel, object id)
        {
            var tInDb = dbEntities.ToList().FirstOrDefault(tx => EfDataService.KeyEquals(id, tx));
            if (tInDb != null)
                if (tryUpdateModel(tInDb))
                    return true; 

            return false;
        }

        static public bool Insert(DbSet<T> dbEntities, Predicate<T> tryUpdateModel)
        {
            var tNew = new T();

            if (tryUpdateModel(tNew))
            {
                // The model is valid - insert the entity.
                dbEntities.Add(tNew);
                return true;
            }

            return false;
        }

        static public bool Delete(DbSet<T> dbEntities, object id)
        {
            var tInDb = dbEntities.ToList().FirstOrDefault(tx => EfDataService.KeyEquals(id, tx));
            if (tInDb != null)
            {
                // delete the entity.
                dbEntities.Remove(tInDb);
                return true;
            }

            return false;
        }

        #endregion


        #region BATCH EDITING

        static private void BatchInsert(IDbSet<T> dbEntities, T t)
        {
            //if (!TryUpdateModel(T)) return;

            var newT = new T();

            EfDataService.Copy(t, newT);

            dbEntities.Add(newT);
            //dc.SaveChanges();
        }

        static private void BatchSave(IEnumerable<T> dbEntities, T t)
        {
            //if (!TryUpdateModel(T)) return;

            var tInDb = dbEntities.ToList().FirstOrDefault(tx => EfDataService.KeyEquals(t, tx));
            if (tInDb == null) return;

            EfDataService.Copy(t, tInDb);

            //dc.SaveChanges();
        }

        static private void BatchDelete(IDbSet<T> dbEntities, T t)
        {
            var tInDb = dbEntities.ToList().FirstOrDefault(tx => EfDataService.KeyEquals(t, tx));
            if (tInDb == null) return;

            dbEntities.Remove(tInDb);
            //dc.SaveChanges();
        }


        static public void BatchEditing(IDbSet<T> dbEntities, IDbSet<T> insertedEntities, IDbSet<T> updatedEntities, IEnumerable<T> deletedEntities)
        {
            if (insertedEntities != null)
                insertedEntities.ToList().ForEach(e => BatchInsert(dbEntities, e));
            if (updatedEntities != null)
                updatedEntities.ToList().ForEach(e => BatchSave(dbEntities, e));
            if (deletedEntities != null)
                deletedEntities.ToList().ForEach(e => BatchDelete(dbEntities, e));
        }

        //static public void BatchEditingP(DbContext dc, IEnumerable<T> dbEntities, params object[] p)
        //{
        //    var insertedEntities = (IEnumerable<T>)p[0];
        //    var updatedEntities = (IEnumerable<T>)p[1];
        //    var deletedEntities = (IEnumerable<T>)p[2];

        //    if (insertedEntities != null)
        //        insertedEntities.ToList().ForEach(e => BatchInsert(dc, e));
        //    if (updatedEntities != null)
        //        updatedEntities.ToList().ForEach(e => BatchSave(dc, dbEntities, e));
        //    if (deletedEntities != null)
        //        deletedEntities.ToList().ForEach(e => BatchDelete(dc, dbEntities, e));
        //}

        #endregion
    }
}
