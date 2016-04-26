using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GeneralTools.Models;

namespace CkgDomainLogic.General.Database.Models
{
    [Table("Application")]
    public class Application
    {
        [SelectListKey]
        [Key]
        public int AppID { get; set; }

        public string AppName { get; set; }

        [SelectListText]
        public string AppFriendlyName { get; set; }

        public string AppType { get; set; }

        public string AppURL { get; set; }

        public bool AppInMenu { get; set; }

        public string AppComment { get; set; }

        public int AppParent { get; set; }

        public int AppRank { get; set; }

        public int AuthorizationLevel { get; set; }

        public bool BatchAuthorization { get; set; }

        public bool LogDuration { get; set; }

        public int? AppSchwellwert { get; set; }

        public int? MaxLevel { get; set; }

        public int? MaxLevelsPerGroup { get; set; }

        public string AppTechType { get; set; }

        public string AppDescription { get; set; }
    }
}
