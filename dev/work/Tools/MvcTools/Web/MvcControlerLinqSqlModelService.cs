using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web.Mvc;
using MvcTools.Data;
using System.Data.Objects;

namespace MvcTools.Web
{
    public class MvcControlerLinqSqlModelService<T> where T : class, new()
    {
        #region POPUP EDITING

        static public object Update(DataContext dc, IEnumerable<T> dbEntities, Predicate<T> tryUpdateModel, object id)
        {
            var tInDb = dbEntities.FirstOrDefault(tx => LinqSqlDataService.KeyEquals(id, tx));
            if (tInDb != null)
                if (tryUpdateModel(tInDb))
                    dc.SubmitChanges();

            return tInDb;
        }

        static public object Insert(DataContext dc, IEnumerable<T> dbEntities, Predicate<T> tryUpdateModel)
        {
            var tNew = new T();

            if (tryUpdateModel(tNew))
            {
                // The model is valid - insert the entity.
                dc.GetTable(tNew.GetType()).InsertOnSubmit(tNew);
                dc.SubmitChanges();
            }

            return tNew;
        }

        static public object Delete(DataContext dc, IEnumerable<T> dbEntities, object id)
        {
            var tInDb = dbEntities.FirstOrDefault(tx => LinqSqlDataService.KeyEquals(id, tx));
            if (tInDb != null)
            {
                // delete the entity.
                dc.GetTable(tInDb.GetType()).DeleteOnSubmit(tInDb);
                dc.SubmitChanges();
            }

            return tInDb;
        }

        #endregion


        #region BATCH EDITING

        static private void BatchInsert(DataContext dc, T t)
        {
            //if (!TryUpdateModel(T)) return;

            var tEntities = dc.GetTable(typeof(T));

            var newT = new T();

            LinqSqlDataService.Copy(t, newT);

            tEntities.InsertOnSubmit(newT);
            dc.SubmitChanges();
        }

        static private void BatchSave(DataContext dc, IEnumerable<T> dbEntities, T t)
        {
            //if (!TryUpdateModel(T)) return;

            var tInDb = dbEntities.FirstOrDefault(tx => LinqSqlDataService.KeyEquals(t, tx));
            if (tInDb == null) return;

            LinqSqlDataService.Copy(t, tInDb);

            dc.SubmitChanges();
        }

        static private void BatchDelete(DataContext dc, IEnumerable<T> dbEntities, T t)
        {
            var tEntities = dc.GetTable(typeof(T));
            var tInDb = dbEntities.FirstOrDefault(tx => LinqSqlDataService.KeyEquals(t, tx));
            if (tInDb == null) return;

            tEntities.DeleteOnSubmit(tInDb);
            dc.SubmitChanges();
        }


        static public void BatchEditing(DataContext dc, IEnumerable<T> dbEntities, IEnumerable<T> insertedEntities, IEnumerable<T> updatedEntities, IEnumerable<T> deletedEntities)
        {
            if (insertedEntities != null)
                insertedEntities.ToList().ForEach(e => BatchInsert(dc, e));
            if (updatedEntities != null)
                updatedEntities.ToList().ForEach(e => BatchSave(dc, dbEntities, e));
            if (deletedEntities != null)
                deletedEntities.ToList().ForEach(e => BatchDelete(dc, dbEntities, e));
        }

        static public void BatchEditingP(DataContext dc, IEnumerable<T> dbEntities, params object[] p)
        {
            var insertedEntities = (IEnumerable<T>)p[0];
            var updatedEntities = (IEnumerable<T>)p[1];
            var deletedEntities = (IEnumerable<T>)p[2];

            if (insertedEntities != null)
                insertedEntities.ToList().ForEach(e => BatchInsert(dc, e));
            if (updatedEntities != null)
                updatedEntities.ToList().ForEach(e => BatchSave(dc, dbEntities, e));
            if (deletedEntities != null)
                deletedEntities.ToList().ForEach(e => BatchDelete(dc, dbEntities, e));
        }

        #endregion
    }
}
