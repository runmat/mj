using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Ueberfuehrung.Models
{
    public class WebUploadProtokoll
    {
        [LocalizedDisplay(LocalizeConstants._Protokollart)]
        public string Protokollart { get; set; }

        [LocalizedDisplay(LocalizeConstants._Kategorie)]
        public string Kategorie { get; set; }

        public string FahrtIndex { get; set; }
    }
}
