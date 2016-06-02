using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CkgDomainLogic.General.Database.Models
{
    [Table("Rights")]
    public class ApplicationGroupRight
    {
        [Key, Column(Order = 0)]
        public int GroupID { get; set; }

        [Key, Column(Order = 1)]
        public int AppID { get; set; }

        public int AuthorizationLevel { get; set; }

        public int WithAuthorization { get; set; }

        public string NewLevel { get; set; }
    }
}
