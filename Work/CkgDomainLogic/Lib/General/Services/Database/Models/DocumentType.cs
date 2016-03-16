using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.General.Database.Models
{
    [Table("DocumentType")]
    public class DocumentType
    {
        [Key]
        public int DocumentTypeID { get; set; }

        [Required]
        [MaxLength(50)]
        [LocalizedDisplay(LocalizeConstants.DocumentType)]
        public string DocTypeName { get; set; }

        [Required]
        public int CustomerID { get; set; }
    }
}
