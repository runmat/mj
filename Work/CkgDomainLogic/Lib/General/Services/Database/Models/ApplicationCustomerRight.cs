using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CkgDomainLogic.General.Database.Models
{
    [Table("CustomerRights")]
    public class ApplicationCustomerRight
    {
        [Key, Column(Order = 0)]
        public int CustomerID { get; set; }

        [Key, Column(Order = 1)]
        public int AppID { get; set; }

        public bool AppIsMvcDefaultFavorite { get; set; }
    }
}
