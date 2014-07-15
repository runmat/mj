namespace LogMaintenance.Models
{
    public class DbMaintenanceStep
    {
        public string Description { get; set; }

        public string Sql { get; set; }

        public bool IgnoreSqlException { get; set; }
    }
}
