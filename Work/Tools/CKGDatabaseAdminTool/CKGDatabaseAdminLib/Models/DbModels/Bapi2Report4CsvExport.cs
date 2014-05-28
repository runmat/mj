using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CKGDatabaseAdminLib.Models.DbModels
{
    [Table("vwBapi2Report4CsvExport")]
    public class Bapi2Report4CsvExport
    {
        /// <summary>
        /// Dummy-Key-Spalte im View (rownumber...), damit Entity Framework zufrieden ist
        /// </summary>
        [Key]
        public long RowID { get; set; }

        public string KUNNR { get; set; }

        public string Customername { get; set; }

        public string AppFriendlyName { get; set; }

        public string AppName { get; set; }

        public string AppURL { get; set; }

        public string BAPI { get; set; }
    }
}
