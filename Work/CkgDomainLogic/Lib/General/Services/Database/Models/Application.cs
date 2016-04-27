using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.General.Database.Models
{
    [Table("Application")]
    public class Application
    {
        [SelectListKey]
        [Key]
        public int AppID { get; set; }

        [LocalizedDisplay(LocalizeConstants.Application)]
        public string AppName { get; set; }

        [SelectListText]
        [LocalizedDisplay(LocalizeConstants.Name)]
        public string AppFriendlyName { get; set; }

        [LocalizedDisplay(LocalizeConstants.Type)]
        public string AppType { get; set; }

        [LocalizedDisplay(LocalizeConstants.Url)]
        public string AppURL { get; set; }

        public bool AppInMenu { get; set; }

        [LocalizedDisplay(LocalizeConstants.Comment)]
        public string AppComment { get; set; }

        public int AppParent { get; set; }

        public int AppRank { get; set; }

        public int AuthorizationLevel { get; set; }

        public bool BatchAuthorization { get; set; }

        public bool LogDuration { get; set; }

        public int? AppSchwellwert { get; set; }

        public int? MaxLevel { get; set; }

        public int? MaxLevelsPerGroup { get; set; }

        [LocalizedDisplay(LocalizeConstants.Technology)]
        public string AppTechType { get; set; }

        [LocalizedDisplay(LocalizeConstants.Description)]
        public string AppDescription { get; set; }
    }
}
