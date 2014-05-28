using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GeneralTools.Models;

namespace GeneralTools.Log.Models.MultiPlatform
{
    [Table("ApplicationTranslated")]
    public class MpApplicationTranslated
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [SelectListKey]
        public int AppID { get; set; }

        public string AppName { get; set; }

        public string AppFriendlyName { get; set; }

        [NotMapped]
        [SelectListText]
        public string AppDropdownName { get { return AppFriendlyName.FormatIfNotNull("{this}, {0}", PortalType); } }

        public string PortalType { get; set; }
    }
}
