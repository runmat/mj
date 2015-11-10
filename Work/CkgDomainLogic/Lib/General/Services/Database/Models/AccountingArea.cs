using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CkgDomainLogic.General.Database.Models
{
    [Table("AccountingArea")]
    public class AccountingArea
    {
        [Key]
        public int Area { get; set; }

        public string Description { get; set; }
    }
}
