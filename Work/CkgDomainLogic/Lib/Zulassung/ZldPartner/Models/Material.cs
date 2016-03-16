using GeneralTools.Models;

namespace CkgDomainLogic.ZldPartner.Models
{
    public class Material
    {
        [SelectListKey]
        public string MaterialNr { get; set; }

        public string MaterialText { get; set; }

        [SelectListText]
        public string MaterialNrUndText
        {
            get { return string.Format("{0} ~ {1}", MaterialNr.NotNullOrEmpty().TrimStart('0'), MaterialText); }
        }

        public bool In1010Hinzufuegbar { get; set; }

        public bool In1510Hinzufuegbar { get; set; }

        public bool PreisEingebbar { get; set; }
    }
}
