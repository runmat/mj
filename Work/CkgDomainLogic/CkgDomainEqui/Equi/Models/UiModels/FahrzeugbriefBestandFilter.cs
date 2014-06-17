using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    public class FahrzeugbriefBestandFilter
    {
        [LocalizedDisplay(LocalizeConstants.Stock)]
        public bool SelektionsfilterLagerbestand { get; set; }

        [LocalizedDisplay(LocalizeConstants.TempDispatchedPlur)]
        public bool SelektionsfilterTempVersendete { get; set; }
    }
}
