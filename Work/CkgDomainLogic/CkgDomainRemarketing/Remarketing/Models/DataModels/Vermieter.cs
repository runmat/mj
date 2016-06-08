using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Remarketing.Models
{
    public class Vermieter
    {
        [SelectListKey]
        [LocalizedDisplay(LocalizeConstants.CarRentalCompany)]
        public string VermieterId { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarRentalCompany)]
        public string VermieterName { get; set; }

        [SelectListText]
        [LocalizedDisplay(LocalizeConstants.CarRentalCompany)]
        public string VermieterText { get { return string.Format("{0} {1}", VermieterId, VermieterName); } }
    }
}
