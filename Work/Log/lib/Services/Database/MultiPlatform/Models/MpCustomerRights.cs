using System.ComponentModel.DataAnnotations.Schema;

namespace GeneralTools.Log.Models.MultiPlatform
{
    [Table("CustomerRights")]
    public class MpCustomerRights
    {
        public int CustomerID { get; set; }

        public int AppID { get; set; }
    }
}
