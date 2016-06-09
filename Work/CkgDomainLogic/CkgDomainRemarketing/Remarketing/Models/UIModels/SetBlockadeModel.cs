using System.ComponentModel.DataAnnotations;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Remarketing.Models
{
    public class SetBlockadeModel
    {
        [Required]
        [StringLength(255)]
        [LocalizedDisplay(LocalizeConstants.BlockText)]
        public string BlockadeText { get; set; }
    }
}
