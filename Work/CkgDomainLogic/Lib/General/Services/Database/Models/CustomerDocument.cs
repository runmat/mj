using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.General.Database.Models
{
    /// <summary>
    /// Datenobjekt für Kundendokument-Einträge, muss ggf. nochmal gewrappt werden, um 
    /// für den jeweiligen Anwendungsfall die passenden Feldbezeichnungen etc. zu haben.
    /// </summary>
    [Table("CustomerDocument")]
    public class CustomerDocument
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// Über dieses Feld kann eine Verknüpfung zu SAP hergestellt werden, z.B. über die VIN etc.
        /// </summary>
        [MaxLength(50)]
        public string ReferenceField { get; set; }

        [Required]
        [MaxLength(4)]
        [LocalizedDisplay(LocalizeConstants.FileType)]
        public string FileType { get; set; }

        [Required]
        [MaxLength(100)]
        [LocalizedDisplay(LocalizeConstants.FileName)]
        public string FileName { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.Categorie)]
        public int? CategoryID { get; set; }

        [NotMapped]
        [LocalizedDisplay(LocalizeConstants.Categorie)]
        public string Category { get; set; }

        [LocalizedDisplay(LocalizeConstants.Uploaded)]
        public DateTime Uploaded { get; set; }

        /// <summary>
        /// Feld für zusätzliche Informationen für den jeweiligen Anwendungsfall
        /// </summary>
        [MaxLength(200)]
        public string AdditionalData { get; set; }

        [Required]
        public int CustomerID { get; set; }

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
