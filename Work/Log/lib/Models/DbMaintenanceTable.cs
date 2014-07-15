using GeneralTools.Models;
using System.Linq;

namespace LogMaintenance.Models
{
    public class DbMaintenanceTable
    {
        public string SourceTableName { get; set; }

        public string DestTableName { get; set; }

        public DbMaintenanceStep[] Steps { get; set; }

        public string TableIndexColumnNames { get; set; }

        
        public string[] GetTableIndexColumnNames()
        {
            return TableIndexColumnNames.NotNullOrEmpty().Split(',').Select(s => s.Trim()).ToArray();
        }

        public string PrepareStatement(string sql, string indexColumnName="")
        {
            sql = sql.Replace("[SourceTableName]", SourceTableName);
            sql = sql.Replace("[DestTableName]", DestTableName);
            sql = sql.Replace("[IndexColumnName]", indexColumnName);
            
            return sql;
        }
    }
}
