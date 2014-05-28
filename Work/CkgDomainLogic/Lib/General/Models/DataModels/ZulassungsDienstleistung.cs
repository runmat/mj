using GeneralTools.Models;

namespace CkgDomainLogic.General.Models
{
    public class ZulassungsDienstleistung
    {
        [SelectListKey]
        public string ID { get; set; }

        [SelectListText]
        public string Name { get; set; }

        public string Code { get; set; }

        //public bool IstStandard { get; set; }

        public bool IstGewaehlt { get; set; }

        public string SelectedAsString { get { return IstGewaehlt ? "selected" : ""; } }
    }
}
