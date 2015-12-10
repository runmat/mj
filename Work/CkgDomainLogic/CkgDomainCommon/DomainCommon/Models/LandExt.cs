using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.DomainCommon.Models
{
    public class LandExt
    {
        [LocalizedDisplay(LocalizeConstants.Code)]
        public string Code { get; set; }

        [LocalizedDisplay(LocalizeConstants.Code)]
        [SelectListKey]
        public string CodeExt { get; set; }

        [LocalizedDisplay(LocalizeConstants.Country)]
        public string Bezeichnung { get; set; }

        [LocalizedDisplay(LocalizeConstants.Country)]
        [SelectListText]
        public string LandAsText
        {
            get
            {
                if (string.IsNullOrEmpty(Code) && string.IsNullOrEmpty(CodeExt))
                    return Bezeichnung;

                return string.Format("{0}, {1} ({2})", Code, Bezeichnung, CodeExt);
            }
        }
    }
}
