using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CkgDomainLogic.General.Database.Models
{
    [Table("vwGroupArchivAssigned")]
    public class GroupArchiveAssigned
    {
        public int CustomerID { get; set; }

        public int GroupID { get; set; }

        [Key]
        public int ArchivID { get; set; }

        public string EasyLagerortName { get; set; }

        public string EasyArchivName { get; set; }

        public int EasyQueryIndex { get; set; }

        public string EasyQueryIndexName { get; set; }

        public string EasyTitleName { get; set; }

        public string DefaultQuery { get; set; }

        public string Archivetype { get; set; }

        public int SortOrder { get; set; }
    }
}
