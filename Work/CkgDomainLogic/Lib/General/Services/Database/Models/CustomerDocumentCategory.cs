using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.General.Database.Models
{
    [Table("CustomerDocumentCategory")]
    public class CustomerDocumentCategory
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        [LocalizedDisplay(LocalizeConstants.Categorie)]
        public string CategoryName { get; set; }

        [Required]
        public int CustomerID { get; set; }

        [Required]
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Hier muss jede Anwendung, die dieses Model nutzt, einen eindeutigen Key (z.B. Anwendungsname) 
        /// setzen, um ggf. aus einer Vielzahl von Dokumenten eines Kunden die richtigen für den jeweiligen
        /// Anwendungsfall herausfiltern zu können
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string ApplicationKey { get; set; }
    }
}
