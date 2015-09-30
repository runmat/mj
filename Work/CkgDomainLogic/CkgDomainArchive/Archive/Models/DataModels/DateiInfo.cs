using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Archive.Models
{
    public class DateiInfo
    {
        public string DateiPfad { get; set; }

        [LocalizedDisplay(LocalizeConstants.FileName)]
        public string DateiName { get; set; }
    }
}
