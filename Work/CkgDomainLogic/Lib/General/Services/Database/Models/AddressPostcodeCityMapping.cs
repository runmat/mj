using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CkgDomainLogic.General.Database.Models
{
    [Table("AddressPostcodeCityMapping")]
    public class AddressPostcodeCityMapping
    {
        [Key]
        public int ID { get; set; }

        public int PLZ { get; set; }

        public string Ort { get; set; }
    }
}
