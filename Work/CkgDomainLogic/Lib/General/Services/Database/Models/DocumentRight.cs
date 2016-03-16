using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CkgDomainLogic.General.Database.Models
{
    [Table("DocumentRights")]
    public class DocumentRight
    {
        [Key, Column(Order = 0)]
        public int DocumentID { get; set; }

        [Key, Column(Order = 1)]
        public int GroupID { get; set; }
    }
}
