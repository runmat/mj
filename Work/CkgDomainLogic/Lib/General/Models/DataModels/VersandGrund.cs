using GeneralTools.Models;

namespace CkgDomainLogic.General.Models
{
    public class VersandGrund
    {
        [SelectListKey]
        public string Code { get; set; }

        [SelectListText]
        public string Bezeichnung { get; set; }

        public bool IstEndgueltigerVersand { get; set; }
    }
}
