using GeneralTools.Models;

namespace CkgDomainLogic.ZldPartner.Models
{
    public class StornoGrund
    {
        public string Status { get; set; }

        [SelectListKey]
        public string GrundId { get; set; }

        [SelectListText]
        public string GrundText { get; set; }
    }
}
