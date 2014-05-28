using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace SapORM.Contracts
{
    public class SapDataServiceExtensions
    {
        public static DataTable ToTable<T>(IEnumerable<T> list)
        {
            var elementType = typeof(T);
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
                    Exception exception = null;
                    var val = propInfo.GetValue(item, null);
                    try { row[propInfo.Name] = val; }
                    catch (Exception e) { exception = e;   }
                    if (exception != null && val == null)
                        row[propInfo.Name] = DBNull.Value;
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
                    var value = rowSrc[column.ColumnName];

                    if (DBNull.Value.Equals(value) && column.DataType == typeof(string))
                        value = string.Empty;
                    
                    rowDst[column.ColumnName] = value;
                }
                dtDst.Rows.Add(rowDst);
            }
        }

        private static IEnumerable<PropertyInfo> GeValidProperties(Type type)
        {
            return
                type.GetProperties().Where(
                    p => p.GetCustomAttributes(true).All(c => c.GetType() != typeof(SapIgnoreAttribute)));
        }
    }
}
