using GeneralTools.Models;

namespace CkgDomainLogic.General.Models
{
    public class ZulassungsOption
    {
        [SelectListKey]
        public string ID { get; set; }

        [SelectListText]
        public string Name { get; set; }

        public string ZulassungsCode { get; set; }
    }
}
