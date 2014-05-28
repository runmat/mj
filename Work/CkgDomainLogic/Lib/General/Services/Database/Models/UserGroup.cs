using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CkgDomainLogic.General.Database.Models
{
    [Table("WebGroup")]
    public class UserGroup
    {
        [Key]
        public int GroupID { get; set; }

        public string GroupName { get; set; }

        public int CustomerID { get; set; }
    }
}
