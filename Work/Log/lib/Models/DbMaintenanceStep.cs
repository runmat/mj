using System.Xml.Serialization;

namespace LogMaintenance.Models
{
    public class DbMaintenanceStep
    {
        public string Description { get; set; }

        public string Sql { get; set; }

        [XmlAttribute]
        public bool IgnoreSqlException { get; set; }

        [XmlAttribute]
        public bool IsSqlIndexStatement { get; set; }
    }
}
