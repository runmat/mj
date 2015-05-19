using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    public class FahrzeugSperrenVerschieben
    {
        public bool Sperren { get; set; }

        [LocalizedDisplay(LocalizeConstants.Comment)]
        public string Sperrtext { get; set; }

        [LocalizedDisplay(LocalizeConstants.TargetPdi)]
        public string ZielPdi { get; set; }

        [LocalizedDisplay(LocalizeConstants.CommentInternal)]
        public string BemerkungIntern { get; set; }

        [LocalizedDisplay(LocalizeConstants.CommentExternal)]
        public string BemerkungExtern { get; set; }

        public string Message { get; set; }
    }
}
