using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.General.Database.Models
{
    [Table("Document")]
    public class Document
    {
        [Key]
        public int DocumentID { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.FileName)]
        public string FileName { get; set; }

        [Required]
        [MaxLength(4)]
        [LocalizedDisplay(LocalizeConstants.FileType)]
        public string FileType { get; set; }

        public int? DocTypeID { get; set; }

        [NotMapped]
        [LocalizedDisplay(LocalizeConstants.DocumentType)]
        public string DocTypeName { get; set; }

        [Required]
        public int CustomerID { get; set; }

        [LocalizedDisplay(LocalizeConstants.LastEdited)]
        public DateTime LastEdited { get; set; }

        [LocalizedDisplay(LocalizeConstants.Size)]
        public int FileSize { get; set; }

        [LocalizedDisplay(LocalizeConstants.Uploaded)]
        public DateTime Uploaded { get; set; }

        [MaxLength(100)]
        [LocalizedDisplay(LocalizeConstants.Tags)]
        public string Tags { get; set; }
    }
}
