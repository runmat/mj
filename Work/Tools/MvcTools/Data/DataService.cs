using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web.Script.Serialization;
using GeneralTools.Models;

namespace MvcTools.Data
{
    public class DataService
    {
        private static IEnumerable<PropertyInfo> GeValidProperties(Type type)
        {
            return type.GetProperties()
                .Where(p => p.GetCustomAttributes(true)
                    .All(c =>    c.GetType() != typeof(GridExportIgnoreAttribute) && !c.GetType().IsSubclassOf(typeof(GridExportIgnoreAttribute))
                              && c.GetType() != typeof(ScriptIgnoreAttribute)));
        }

        public static DataTable ToTable(IEnumerable list)
        {
            var elementType = list.GetType().GetGenericArguments()[0];
            if (elementType == typeof (object))
            {
                // if (type == object) => try to take the "real" type from the first object:
                var objectList = list.Cast<object>();
                if (objectList.Any())
                {
                    var firstObject = objectList.Take(1).FirstOrDefault();
                    if (firstObject != null)
                        elementType = firstObject.GetType();
                }
            }

            var t = new DataTable();

            //add a column to table for each public property on T  
            foreach (var propInfo in GeValidProperties(elementType))
            {
                if (propInfo.PropertyType.Name.ToLower().Contains("nullable"))
                    t.Columns.Add(propInfo.Name, Nullable.GetUnderlyingType(propInfo.PropertyType));
                else
                    t.Columns.Add(propInfo.Name, propInfo.PropertyType);
            }

            //go through each property on T and add each value to the table  
            foreach (var item in list)
            {
                var row = t.NewRow();
                foreach (var propInfo in GeValidProperties(elementType))
                {
                    var val = propInfo.GetValue(item, null);
                    row[propInfo.Name] = (val ?? DBNull.Value);
                }
                t.Rows.Add(row);
            }
            return t;
        }

        public static void Apply<T>(IEnumerable<T> list, DataTable dtDst)
        {
            var dtSrc = ToTable(list);

            foreach (DataRow rowSrc in dtSrc.Rows)
            {
                var rowDst = dtDst.NewRow();
                foreach (DataColumn column in dtDst.Columns)
                {
                    if (dtSrc.Columns.Contains(column.ColumnName))
                        rowDst[column.ColumnName] = rowSrc[column.ColumnName];
                }
                dtDst.Rows.Add(rowDst);
            }
        }
    }
}
