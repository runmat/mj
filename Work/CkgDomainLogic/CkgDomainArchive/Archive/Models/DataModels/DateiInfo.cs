using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Archive.Models
{
    public class DateiInfo
    {
        public string DateiPfad { get; set; }

        [LocalizedDisplay(LocalizeConstants.Directory)]
        public string Unterverzeichnis { get; set; }

        [LocalizedDisplay(LocalizeConstants.File)]
        public string DateiName { get; set; }

        [LocalizedDisplay(LocalizeConstants.Size)]
        public string DateiGroesse { get; set; }
    }
}
