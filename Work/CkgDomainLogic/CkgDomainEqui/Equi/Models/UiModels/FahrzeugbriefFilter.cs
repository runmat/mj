using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    public class FahrzeugbriefFilter
    {
        [LocalizedDisplay(LocalizeConstants.Stock)]
        public bool SelektionsfilterLagerbestand { get; set; }

        [LocalizedDisplay(LocalizeConstants.TempDispatchedPlur)]
        public bool SelektionsfilterTempVersendete { get; set; }

        public string SelektionsfilterReferenz1 { get; set; }
    }
}
