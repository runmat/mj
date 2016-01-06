using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CkgDomainLogic.General.Database.Models
{
    [Table("vwDataMapping")]
    public class DataConverterDataMapping
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public int CustomerId { get; set; }

        public string Customername { get; set; }

        public string KUNNR { get; set; }

        public string Process { get; set; }

        public string Mapping { get; set; }
    }
}
