using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GeneralTools.Models;

namespace GeneralTools.Log.Models.MultiPlatform
{
    [Table("WebUser")]
    public class MpWebUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [SelectListKey]
        public int UserID { get; set; }

        public string Username { get; set; }

        [NotMapped]
        [SelectListText]
        public string UserDropdownName { get { return Username.FormatIfNotNull("{this}, {0}", UserID); } }

        public int CustomerID { get; set; }
    }
}
