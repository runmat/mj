using System.Collections;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using MvcTools.Data;
using Telerik.Web.Mvc.Extensions;
using Newtonsoft.Json.Linq;

namespace Telerik.Web.Mvc
{
    public static class TelerikExtensions
    {
        #region Filtered Data

        public static IEnumerable<dynamic> GetGridColumns(this string columnJsonArray)
        {
            // this is very very COOL:
            return JArray.Parse(columnJsonArray);
        }

        public static IEnumerable GetGridFilteredData(this IEnumerable list, string orderBy, string filterBy)
        {
            return list.AsQueryable().ToGridModel(1, 9999999, orderBy, string.Empty, filterBy).Data;
        }

        public static DataTable GetGridFilteredDataTable(this IEnumerable list, string columnJsonArray)
        {
            return list.GetGridFilteredDataTable("", "", columnJsonArray);
        }

        public static DataTable GetGridFilteredDataTable(this IEnumerable list, string orderBy, string filterBy, string columnJsonArray)
        {
            var jCols = columnJsonArray.GetGridColumns();

            // select only the columnNames here:    (columnName <=> member)
            //var colMembers = jCols.Select(j => j.member).ToList();
            // select only the columnHeaders here:    (columnHeader <=> title)
            //var colHeaders = jCols.Select(j => j.title).ToList();

            var listFiltered = list.GetGridFilteredData(orderBy, filterBy);

            var dt = DataService.ToTable(listFiltered);
            var dtColumns = dt.Columns.OfType<DataColumn>();

            // remove unused DataColumns in our DataTable (if they don't occur in our columnJsonArray)
            dtColumns.ToList()
                .ForEach(c =>
                {
                    if (jCols.All(cm => cm.member.Value.ToLower() != c.ColumnName.ToLower()))
                        dt.Columns.Remove(c);
                });
            var arr = dtColumns.ToArray();
            var arrLength = arr.Length;

            // reorder the DataColumns of our DataTable according to the ordering of our columnJsonArray
            var newColumnIndex = 0;
            jCols.ToList()
                .ForEach(c =>
                             {
                                 for (var i = 0; i < arrLength; i++)
                                 {
                                     var column = dt.Columns[i];
                                     if (c.member.Value.ToLower() == column.ColumnName.ToLower() && newColumnIndex < dt.Columns.Count)
                                     {
                                         column.Caption = c.title.Value;
                                         column.SetOrdinal(newColumnIndex);
                                         newColumnIndex++;
                                     }
                                 }
                             });
            return dt;
        }

        #endregion
    }
}
