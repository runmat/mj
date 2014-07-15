namespace LogMaintenance.Models
{
    public class DbMaintenanceTable
    {
        public string SourceTableName { get; set; }

        public string DestTableName { get; set; }

        public DbMaintenanceStep[] Steps { get; set; }

        public string PrepareStatement(string sql)
        {
            sql = sql.Replace("[SourceTableName]", SourceTableName);
            sql = sql.Replace("[DestTableName]", DestTableName);
            
            return sql;
        }
    }
}
