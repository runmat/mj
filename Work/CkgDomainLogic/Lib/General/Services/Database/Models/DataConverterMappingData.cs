using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.General.Database.Models
{
    [Table("DataMapping")]
    public class DataConverterMappingData
    {
        [Key]
        public int Id { get; set; }

        [LocalizedDisplay(LocalizeConstants.Title)]
        public string Title { get; set; }

        public int CustomerId { get; set; }

        [LocalizedDisplay(LocalizeConstants.Process)]
        public string Process { get; set; }

        [LocalizedDisplay(LocalizeConstants.Process)]
        public string Mapping { get; set; }
    }
}
