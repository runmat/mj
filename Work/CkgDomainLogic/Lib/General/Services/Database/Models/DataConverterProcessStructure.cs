using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CkgDomainLogic.General.Database.Models
{
    [Table("ProcessStructure")]
    public class DataConverterProcessStructure
    {
        [Key]
        public string ProcessName { get; set; }

        public string DestinationStructure { get; set; }
    }
}
