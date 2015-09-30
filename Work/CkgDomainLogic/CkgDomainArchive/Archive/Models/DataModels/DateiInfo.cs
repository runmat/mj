using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Archive.Models
{
    public class DateiInfo
    {
        [LocalizedDisplay(LocalizeConstants.FileName)]
        public string DateiName { get; set; }
    }
}
