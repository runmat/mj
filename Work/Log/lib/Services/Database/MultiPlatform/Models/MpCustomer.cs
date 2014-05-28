using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GeneralTools.Models;

namespace GeneralTools.Log.Models.MultiPlatform
{
    [Table("Customer")]
    public class MpCustomer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [SelectListKey]
        public int CustomerID { get; set; }

        public string Customername { get; set; }

        [NotMapped]
        [SelectListText]
        public string CustomerDropdownName { get { return Customername.FormatIfNotNull("{this}, {0}", KUNNR); } }

// ReSharper disable InconsistentNaming
        public string KUNNR { get; set; }
// ReSharper restore InconsistentNaming
    }
}
