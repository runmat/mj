using System;
using System.Configuration;
using System.Data;

namespace GeodeService
{
    public class Common
    {
        public static string BingKey { get { return ConfigurationManager.AppSettings["BingKey"]; } }

        public static string LogTableName { get { return ConfigurationManager.AppSettings["LogTableName"]; } }

        public static string GetTableAsString(DataTable tbl)
        {
            string erg = "";

            if (tbl != null)
            {
                // Header
                erg += tbl.TableName + Environment.NewLine;
                erg += "[";
                for (int i = 0; i < tbl.Columns.Count; i++)
                {
                    erg += tbl.Columns[i].ColumnName + "|";
                }
                if (erg.EndsWith("|"))
                    erg = erg.Substring(0, erg.Length - 1);
                erg += "]" + Environment.NewLine;

                // Daten
                for (int j = 0; j < tbl.Rows.Count; j++)
                {
                    erg += "[";
                    for (int i = 0; i < tbl.Columns.Count; i++)
                    {
                        erg += tbl.Rows[j][tbl.Columns[i]].ToString() + "|";
                    }
                    if (erg.EndsWith("|"))
                        erg = erg.Substring(0, erg.Length - 1);
                    erg += "]" + Environment.NewLine;
                }
            }

            return erg;
        }
    }
}
