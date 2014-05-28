using GeneralTools.Models;

namespace CkgDomainLogic.General.Models
{
    public class FahrzeugStatus 
    {
        [SelectListKey]
        public string ID { get; set; }

        public string Name { get; set; }

        [SelectListText]
        public string FullName { get { return ID.IsNullOrEmpty() ? Name : string.Format("{0} - {1}", ID.TrimStart('0').PadLeft(2, '0') , Name); } }
    }
}
