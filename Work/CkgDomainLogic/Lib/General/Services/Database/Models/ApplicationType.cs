using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CkgDomainLogic.General.Database.Models
{
    [Table("ApplicationType")]
    public class ApplicationType
    {
        [Key]
        public string AppType { get; set; }

        public int Rank { get; set; }

        public string DisplayName { get; set; }

        public string ButtonPath { get; set; }
    }
}
