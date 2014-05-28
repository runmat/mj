using GeneralTools.Models;

namespace CkgDomainLogic.General.Models
{
    public class VersandOption
    {
        [SelectListKey]
        public string ID { get; set; }

        [SelectListText]
        public string Name { get; set; }

        public string MaterialCode { get; set; }

        public bool IstEndgueltigerVersand { get; set; }
    }
}
